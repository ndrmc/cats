using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Rest.Models;
using Cats.Services.EarlyWarning;
namespace Cats.Rest.Controllers
{
    public class RegionalRequestController : ApiController
    {
        private readonly IRegionalRequestService _regionalRequestService;

        public RegionalRequestController(IRegionalRequestService regionalRequestService)
        {
            _regionalRequestService = regionalRequestService;
        }
        [HttpGet]
        public IEnumerable<RegionalRequest> GetRegionalRequests()
        {
            return (from rr in _regionalRequestService.GetAllRegionalRequest()
                select new Models.RegionalRequest()
                {
                    PlanID = rr.PlanID,
                    Year = rr.Year,
                    RationID = rr.RationID,
                    RegionalRequestDetails = (from rd in rr.RegionalRequestDetails
                        select new RegionalRequestDetail()
                        {
                            Beneficiaries = rd.Beneficiaries,
                            FdpId = rd.Fdpid,
                            FdpName = rd.Fdp.Name,
                            RegionalRequestDetailId = rd.RegionalRequestDetailID,
                            RegionalRequestId = rd.RegionalRequestID
                        }).ToList(),
                    RegionalRequestID = rr.RegionalRequestID,
                    RegionID = rr.RegionID,
                    ApprovedBy = rr.ApprovedBy,
                    Contingency = rr.Contingency,
                    DonorID = rr.DonorID,
                    DonorName = "",
                    IDPSReasonType = rr.IDPSReasonType,
                    Month = rr.Month,
                    PartitionId = rr.PartitionId,
                    ProgramId = rr.ProgramId,
                    ProgramName = rr.Program.Name,
                    RationDetail = new Ration()
                    {
                        CreatedBy = rr.Ration.CreatedBy,
                        CreatedDate = rr.Ration.CreatedDate,
                        IsDefaultRation = rr.Ration.IsDefaultRation,
                        RationID = rr.RationID,
                        RefrenceNumber = rr.Ration.RefrenceNumber,
                        UpdatedBy = rr.Ration.UpdatedBy,
                        UpdatedDate = rr.Ration.UpdatedDate
                    },
                    ReferenceNumber = rr.ReferenceNumber,
                    RegionId = rr.RegionID,
                    //RegionName = rr.re
                    Remark = rr.Remark,
                    RequestedBy = rr.RequestedBy,
                    //SeasonId = rr.Seaso
                    StatusId = rr.Status,
                    //StatusName = rr.bu

                }).ToList();
        }

        [HttpGet]
        public RegionalRequest GetRegionalRequest(int id)
        {
            var regionalRequest = _regionalRequestService.FindById(id);
            if (regionalRequest == null) return null;
            return new Models.RegionalRequest()
            {
                PlanID = regionalRequest.PlanID,
                Year = regionalRequest.Year,
                RationID = regionalRequest.RationID,
                RegionalRequestDetails = (from rd in regionalRequest.RegionalRequestDetails
                    select new RegionalRequestDetail()
                    {
                        Beneficiaries = rd.Beneficiaries,
                        FdpId = rd.Fdpid,
                        FdpName = rd.Fdp.Name,
                        RegionalRequestDetailId = rd.RegionalRequestDetailID,
                        RegionalRequestId = rd.RegionalRequestID
                    }).ToList(),
                RegionalRequestID = regionalRequest.RegionalRequestID,
                RegionID = regionalRequest.RegionID,
                ApprovedBy = regionalRequest.ApprovedBy,
                Contingency = regionalRequest.Contingency,
                DonorID = regionalRequest.DonorID,
                DonorName = "",
                IDPSReasonType = regionalRequest.IDPSReasonType,
                Month = regionalRequest.Month,
                PartitionId = regionalRequest.PartitionId,
                ProgramId = regionalRequest.ProgramId,
                ProgramName = regionalRequest.Program.Name,
                RationDetail = new Ration()
                {
                    CreatedBy = regionalRequest.Ration.CreatedBy,
                    CreatedDate = regionalRequest.Ration.CreatedDate,
                    IsDefaultRation = regionalRequest.Ration.IsDefaultRation,
                    RationID = regionalRequest.RationID,
                    RefrenceNumber = regionalRequest.Ration.RefrenceNumber,
                    UpdatedBy = regionalRequest.Ration.UpdatedBy,
                    UpdatedDate = regionalRequest.Ration.UpdatedDate
                },
                ReferenceNumber = regionalRequest.ReferenceNumber,
                RegionId = regionalRequest.RegionID,
                //RegionName = rr.re
                Remark = regionalRequest.Remark,
                RequestedBy = regionalRequest.RequestedBy,
                //SeasonId = rr.Seaso
                StatusId = regionalRequest.Status,
                //StatusName = rr.bu

            };
        }

        [HttpGet]
        public List<Models.Request> Request(int year, int programId, int planId, int? month,int? round)
        {
            var listOfRequests = new List<Models.Request>();
            List<Cats.Models.RegionalRequest> requests = null;

            if (year != 0 && programId != 0)
            {

               if (programId == 1 || programId == 3) //relief or idps
                {
                     requests = _regionalRequestService.FindBy(r => r.Year == year && r.ProgramId == programId && r.PlanID == planId && r.Round ==round).ToList();

                }
               else if (programId==2)
               {
                   requests = _regionalRequestService.FindBy(r => r.Year == year && r.ProgramId == programId && r.PlanID == planId && r.Month == month).ToList();
               }

            }


            if (requests != null)
            {
                foreach (var request in requests)
                {
                    var newRequest = new Models.Request(request.RegionID, request.AdminUnit.Name, "",
                                                        request.RegionalRequestDetails.Sum(b => b.Beneficiaries),
                                                        request.RequistionDate.Date,
                                                        request.RegionalRequestDetails.Count(f => f.Fdpid != 0),
                                                        request.RegionalRequestDetails.Select(w => w.Fdp.AdminUnitID).Distinct().Count());

                    listOfRequests.Add(newRequest);
                }

                var requestsGroupByRegion = listOfRequests.GroupBy(r => r.RegionId)
                    .Select(r =>
                                {
                                    var firstOrDefault = r.FirstOrDefault();
                                    return firstOrDefault != null
                                               ? new Models.Request(firstOrDefault.RegionId,
                                                                    firstOrDefault.RegionName, "",
                                                                    r.Sum(b=>b.BeneficiaryNo),
                                                                    firstOrDefault.RequestedDate.Date,
                                                                    firstOrDefault.NumberOfFPDs,
                                                                    firstOrDefault.NumberOfWoredas)
                                               : null;
                                });
                return requestsGroupByRegion.ToList();
            }
            return null;

          

        } 
    }
}