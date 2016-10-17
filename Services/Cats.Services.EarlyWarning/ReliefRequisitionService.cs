

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Web.Mvc;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;
using Cats.Services.Common;

namespace Cats.Services.EarlyWarning
{

    public class ReliefRequisitionService : IReliefRequisitionService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IBusinessProcessService _businessProcessService;
        private readonly IApplicationSettingService _applicationSettingService;

        public ReliefRequisitionService(IUnitOfWork unitOfWork, IApplicationSettingService applicationSettingService, IBusinessProcessService businessProcessService)
        {
            this._unitOfWork = unitOfWork;
            _applicationSettingService = applicationSettingService;
            _businessProcessService = businessProcessService;
        }

        #region Default Service Implementation
        public bool AddReliefRequisition(ReliefRequisition reliefRequisition)
        {
            _unitOfWork.ReliefRequisitionRepository.Add(reliefRequisition);
            _unitOfWork.Save();
            return true;

        }
        public bool EditReliefRequisition(ReliefRequisition reliefRequisition)
        {
            _unitOfWork.ReliefRequisitionRepository.Edit(reliefRequisition);
            _unitOfWork.Save();
            return true;

        }
        public bool DeleteReliefRequisition(ReliefRequisition reliefRequisition)
        {
            if (reliefRequisition == null) return false;
            _unitOfWork.ReliefRequisitionRepository.Delete(reliefRequisition);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.ReliefRequisitionRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.ReliefRequisitionRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<ReliefRequisition> GetAllReliefRequisition()
        {
            return _unitOfWork.ReliefRequisitionRepository.GetAll();
        }
        public ReliefRequisition FindById(int id)
        {
            return _unitOfWork.ReliefRequisitionRepository.FindById(id);
        }
        public List<ReliefRequisition> FindBy(Expression<Func<ReliefRequisition, bool>> predicate)
        {
            return _unitOfWork.ReliefRequisitionRepository.FindBy(predicate);
        }

        public IEnumerable<ReliefRequisition> Get(
          Expression<Func<ReliefRequisition, bool>> filter = null,
          Func<IQueryable<ReliefRequisition>, IOrderedQueryable<ReliefRequisition>> orderBy = null,
          string includeProperties = "")
        {
            return _unitOfWork.ReliefRequisitionRepository.Get(filter, orderBy, includeProperties);
        }


        #endregion



        public List<RegionalRequisitionsSummary> GetRequisitionsSentToLogistics()
        {
            //var app3 = _unitOfWork.ReliefRequisitionRepository.FindBy(r => r.Status == 3); 

            var app = _unitOfWork.ReliefRequisitionRepository.FindBy(t => t.Status >= 2);
            var ds =
                  (
                      from a in app
                      group a by new { a.RegionID, a.AdminUnit.Name } into regionalRequistions
                      select new
                      {
                          regionalRequistions.Key.Name,
                          ProgramBased = from a in regionalRequistions
                                         group a by new { a.ProgramID, a.Program } into final
                                         select new
                                       RegionalRequisitionsSummary()
                                         {
                                             RegionID = regionalRequistions.Key.RegionID,
                                             RegionName = regionalRequistions.Key.Name,
                                             DateLastModified = regionalRequistions.Select(d => d.ApprovedDate).Last(),
                                             NumberOfHubUnAssignedRequisitions =
                                           _unitOfWork.ReliefRequisitionRepository.FindBy(s => (s.RegionID == regionalRequistions.Key.RegionID && s.ProgramID == final.Key.ProgramID && s.Status == 2)).Count,
                                             NumberOfTotalRequisitions = regionalRequistions.Count(p => p.ProgramID == final.Key.ProgramID),
                                             ProgramType = final.Key.Program.Name
                                           //Percentage = (NumberOfHubAssignedRequisitions / regionalRequistions.Count()) * 100,
                                       }
                      }
                  );

            return (from d in ds
                    from e in d.ProgramBased
                    select new RegionalRequisitionsSummary()
                    {
                        RegionID = e.RegionID,
                        RegionName = e.RegionName,
                        DateLastModified = e.DateLastModified,
                        NumberOfHubUnAssignedRequisitions = e.NumberOfHubUnAssignedRequisitions,
                        NumberOfTotalRequisitions = e.NumberOfTotalRequisitions,
                        ProgramType = e.ProgramType,
                        Percentage = e.NumberOfHubUnAssignedRequisitions / e.NumberOfTotalRequisitions * 100
                        //Percentage
                    }).ToList();
        }

        public void AddReliefRequisions(List<ReliefRequisition> reliefRequisitions)
        {
            foreach (var reliefRequisition in reliefRequisitions)
            {
                this._unitOfWork.ReliefRequisitionRepository.Add(reliefRequisition);
            }
        }

        public IEnumerable<ReliefRequisitionNew> CreateRequisition(int requestId, string user)
        {
            //Check if Requisition is created from this request
            //
            var sample = _unitOfWork.RegionalRequestRepository.Get(t => t.RegionalRequestID == requestId);
            var regionalRequest = _unitOfWork.RegionalRequestRepository.Get(t => t.RegionalRequestID == requestId && t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Approved", null, "RegionalRequestDetails,BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (regionalRequest == null) return null;

            var reliefRequistions = CreateRequistionFromRequest(regionalRequest, user);
            //if (reliefRequistions.Count < 1)
            //    return GetRequisitionByRequestId(requestId);
            AddReliefRequisions(reliefRequistions);
            regionalRequest.Status = (int)RegionalRequestStatus.Closed;
            _unitOfWork.Save();

            foreach (var item in reliefRequistions)
            {
                item.RequisitionNo = String.Format("REQ-{0}", item.RequisitionID);
            }
            _unitOfWork.Save();
            return GetRequisitionByRequestId(requestId);
        }

        public ReliefRequisition GenerateRequisition(RegionalRequest regionalRequest, List<RegionalRequestDetail> regionalRequestDetails, int commodityId, int zoneId, string user)
        {
            var relifRequisition = new ReliefRequisition()
            {
                //TODO:Please Include Regional Request ID in Requisition 
                RegionalRequestID = regionalRequest.RegionalRequestID,
                Round = regionalRequest.Round,
                Month = regionalRequest.Month,
                ProgramID = regionalRequest.ProgramId,
                CommodityID = commodityId,
                RequestedDate = regionalRequest.RequistionDate,
                RationID = regionalRequest.RationID

                //TODO:Please find another way how to specify Requistion No
                ,
                RequisitionNo = Guid.NewGuid().ToString(),
                RegionID = regionalRequest.RegionID,
                ZoneID = zoneId,
                //Status = (int)ReliefRequisitionStatus.Draft,
                //RequestedBy =itm.RequestedBy,
                //ApprovedBy=itm.ApprovedBy,
                //ApprovedDate=itm.ApprovedDate,

            };

            int BP_PR = 0;
            List<ApplicationSetting> ret = _applicationSettingService.FindBy(t => t.SettingName == "ReliefRequisitionWorkflow");
            if (ret.Count == 1)
            {
                BP_PR = Int32.Parse(ret[0].SettingValue);
            }
            if (BP_PR != 0)
            {
                BusinessProcessState createdstate = new BusinessProcessState
                {
                    DatePerformed = DateTime.Now,
                    PerformedBy = user,
                    Comment = "New Requisition Created"

                };
                //_PaymentRequestservice.Create(request);

                BusinessProcess bp = _businessProcessService.CreateBusinessProcess(BP_PR, 0,
                    "ReliefRequisition", createdstate);
                if (bp != null)
                {
                    relifRequisition.BusinessProcessID = bp.BusinessProcessID;
                    this._unitOfWork.ReliefRequisitionRepository.Add(relifRequisition);
                }
                else
                {
                    //ModelState.AddModelError("Error", errorMessage: @"Could not create a business process object");
                }
            }
            else
            {
                //ModelState.AddModelError("Error", errorMessage: @"Could not find the application setting for this document process template");
            }

            foreach (var regionalRequestDetail in regionalRequestDetails)
            {
                decimal contengency = 0;
                if (regionalRequestDetail.RegionalRequest.Contingency)
                    contengency = (regionalRequestDetail.RequestDetailCommodities.Sum(a => a.Amount) * (decimal).05);

                var relifRequistionDetail = new ReliefRequisitionDetail();
                var commodity = regionalRequestDetail.RequestDetailCommodities.First(t => t.CommodityID == commodityId);
                relifRequistionDetail.DonorID = regionalRequest.DonorID;
                relifRequistionDetail.FDPID = regionalRequestDetail.Fdpid;
                relifRequistionDetail.BenficiaryNo = regionalRequestDetail.Beneficiaries;
                relifRequistionDetail.CommodityID = commodity.CommodityID;
                relifRequistionDetail.Amount = commodity.Amount + contengency;

                relifRequisition.ReliefRequisitionDetails.Add(relifRequistionDetail);
            }
            return relifRequisition;
        }


        public List<ReliefRequisition> CreateRequistionFromRequest(RegionalRequest regionalRequest, string user)
        {
            //Note Here we are going to create 4 requistion from one request
            //Assumtions Here is ColumnName of the request detail match with commodity name 
            var requestDetails =
                _unitOfWork.RegionalRequestDetailRepository.Get(
                    t => t.RegionalRequestID == regionalRequest.RegionalRequestID && t.Beneficiaries > 0, null, "FDP.AdminUnit");

            var reliefRequisitions = new List<ReliefRequisition>();

            var zones = GetZonesFoodRequested(regionalRequest.RegionalRequestID);

            foreach (var zone in zones)
            {
                var zoneId = zone.HasValue ? zone.Value : -1;
                if (zoneId == -1) continue;
                var zoneRequestDetails = (from item in requestDetails where item.Fdp.AdminUnit.ParentID == zoneId select item).ToList();
                if (zoneRequestDetails.Count < 1) continue;

                var requestDetail = zoneRequestDetails.FirstOrDefault();
                if (requestDetail == null) continue;

                var requestCommodity =
                    (from item in requestDetail.RequestDetailCommodities select item.CommodityID).ToList();
                if (requestCommodity.Count < 1) continue;

                foreach (var commodityId in requestCommodity)
                {
                    reliefRequisitions.Add(GenerateRequisition(regionalRequest, zoneRequestDetails, commodityId, zoneId, user));
                }


            }

            return reliefRequisitions;
        }
        public List<int?> GetZonesFoodRequested(int requestId)
        {
            var regionalRequestDetails =
                _unitOfWork.RegionalRequestDetailRepository.Get(t => t.RegionalRequestID == requestId && t.Beneficiaries > 0, null,
                                                                "FDP,FDP.AdminUnit");
            var zones =
                (from requestDetail in regionalRequestDetails
                 where requestDetail.Fdp.AdminUnit.ParentID != null
                 select requestDetail.Fdp.AdminUnit.ParentID).Distinct();
            return zones.ToList();

        }

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }





        //public bool Save()
        //{
        //    _unitOfWork.Save();
        //    return true;
        //}





        public bool AssignRequisitonNo(Dictionary<int, string> requisitionNumbers)
        {

            foreach (var requisitonNumber in requisitionNumbers)
            {
                var requisition = _unitOfWork.ReliefRequisitionRepository.FindById(requisitonNumber.Key);
                requisition.RequisitionNo = requisitonNumber.Value;
            }
            _unitOfWork.Save();
            return true;
        }

        public IEnumerable<ReliefRequisitionNew> GetRequisitionByRequestId(int requestId)
        {
            var reliefRequistions =
               _unitOfWork.ReliefRequisitionRepository.Get(t => t.RegionalRequestID == requestId, null, "Program,AdminUnit1,AdminUnit.AdminUnit2,Commodity").ToList();

            if (reliefRequistions.Count < 1)
                return null;
            var input = (from itm in reliefRequistions
                         orderby itm.ZoneID
                         select new ReliefRequisitionNew()
                         {
                             //TODO:Include navigation property for commodity on relife requistion
                             Commodity = itm.Commodity.Name,
                             Program = itm.Program.Name,
                             Region = itm.AdminUnit.Name,
                             Round = itm.Round,
                             Month = itm.Month,
                             Zone = itm.AdminUnit1.Name,
                             Status = itm.Status,
                             RequisitionID = itm.RequisitionID,
                             // RequestedBy = itm.UserProfile,
                             // ApprovedBy = itm.ApprovedBy,
                             RequestedDate = itm.RequestedDate,
                             ApprovedDate = itm.ApprovedDate,
                             RequestDatePref = itm.RequestedDate.ToString(),
                             Input = new ReliefRequisitionNew.ReliefRequisitionNewInput()
                             {
                                 Number = itm.RequisitionID,
                                 RequisitionNo = itm.RequisitionNo
                             }
                         });
            return input;
        }


        public bool EditAllocatedAmount(Dictionary<int, decimal> allocations)
        {

            foreach (var alloction in allocations)
            {
                var requisitionDetail = _unitOfWork.ReliefRequisitionDetailRepository.FindById(alloction.Key);
                if (requisitionDetail == null) return false;
                requisitionDetail.Amount = alloction.Value;
            }

            _unitOfWork.Save();

            return true;
        }
    }
}


