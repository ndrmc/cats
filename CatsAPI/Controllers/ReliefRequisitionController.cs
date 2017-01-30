using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Rest.Models;
using Cats.Services.EarlyWarning;
using Cats.Services.Hub;
using Cats.Services.Procurement;
using ReliefRequisitionDetail = Cats.Models.ReliefRequisitionDetail;

namespace Cats.Rest.Controllers
{
    public class ReliefRequisitionController : ApiController
    {
        //
        // GET: /ReleifRequisition/

        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly IRegionalRequestService _regionalRequestService;
        private Cats.Services.Administration.IUserProfileService _profileService;
        private IStatusService _statusService;
        private IRationDetailService _rationDetail;

        public ReliefRequisitionController(IReliefRequisitionService reliefRequisitionService, Cats.Services.Administration.IUserProfileService profileService, IStatusService statusService, IRationDetailService rationDetail, IRegionalRequestService regionalRequestService)
        {
            _reliefRequisitionService = reliefRequisitionService;
            _profileService = profileService;
            _statusService = statusService;
            _rationDetail = rationDetail;
            _regionalRequestService = regionalRequestService;
        }

        public  List<Models.ReliefRequisition> Get()
        {
            var listOfReliefRequisitions = new List<Models.ReliefRequisition>();
            var ReliefRequisitions = _reliefRequisitionService.GetAllReliefRequisition();
            foreach (var reliefRequisition in ReliefRequisitions)
            {
                var newReliefRequisition = new Models.ReliefRequisition(reliefRequisition.RequisitionID,
                                                                        reliefRequisition.CommodityID,
                                                                        reliefRequisition.Commodity.Name,
                                                                        reliefRequisition.RegionID,
                                                                        reliefRequisition.AdminUnit.Name,
                                                                        reliefRequisition.ZoneID,
                                                                        reliefRequisition.AdminUnit.Name,
                                                                        reliefRequisition.Round, reliefRequisition.Month,
                                                                        reliefRequisition.RequisitionNo,
                                                                        reliefRequisition.RequestedBy,
                                                                        GetUser( reliefRequisition.RequestedBy),
                                                                        reliefRequisition.RequestedDate,
                                                                        reliefRequisition.ApprovedBy,
                                                                        GetUser(reliefRequisition.ApprovedBy),
                                                                        reliefRequisition.ApprovedDate,
                                                                        reliefRequisition.Status,
                                                                        GetStatusName(reliefRequisition.Status),
                                                                        reliefRequisition.ProgramID,
                                                                        reliefRequisition.Program.Name,
                                                                        reliefRequisition.RegionalRequestID,
                                                                        reliefRequisition.RegionalRequestID,
                                                                        GetRationDetail(reliefRequisition.RationID),
                                                                     GetReliefRequisition((List<Cats.Models.ReliefRequisitionDetail>) reliefRequisition.ReliefRequisitionDetails));

                listOfReliefRequisitions.Add(newReliefRequisition);

            }
            return listOfReliefRequisitions;
        }

        private string GetUser(int? id)
        {
            var user = _profileService.FindBy(i=>i.UserProfileID == id).FirstOrDefault();
            if (user !=null)
            {
                return user.FirstName + ' ' + user.LastName;
            }
            return "";
        }

        private string GetStatusName(int? id)
        {
            if (id != null)
            {
                var status = _statusService.FindBy(i=>i.StatusID==id).FirstOrDefault();
                if (status != null)
                {
                    return status.Name;
                }
                return "";
            }
            return "";
        }

        private List<Models.RationDetail> GetRationDetail(int? id)
        {
            var rationDetail = _rationDetail.FindBy(d => d.RationID == id).ToList();
            var listOfRationDetails = new List<Models.RationDetail>();
            if (rationDetail.Count > 0)
            {
                listOfRationDetails.AddRange(rationDetail.Select(detail => detail.Unit != null ? new Models.RationDetail(detail.RationDetailID, detail.RationID, detail.CommodityID, detail.Commodity.Name, detail.Amount, detail.UnitID, detail.Unit.Name) : null));
                return listOfRationDetails;
            }
            return listOfRationDetails;
        }

        private List<Models.ReliefRequisitionDetail> GetReliefRequisition(List<Cats.Models.ReliefRequisitionDetail> reliefRequisitionDetail)
        {
            var listOfReliefRequisitions = new List<Models.ReliefRequisitionDetail>();
            foreach (var requisitionDetail in reliefRequisitionDetail)
            {
                var newReliefRequisitionDetail = new Models.ReliefRequisitionDetail(requisitionDetail.RequisitionDetailID,
                                                                          requisitionDetail.RequisitionID,
                                                                          requisitionDetail.CommodityID,
                                                                          requisitionDetail.Commodity.Name,
                                                                          requisitionDetail.BenficiaryNo,
                                                                          requisitionDetail.Amount,
                                                                          requisitionDetail.FDPID,
                                                                          requisitionDetail.FDP.Name,
                                                                          requisitionDetail.DonorID,
                                                                         null,
                                                                          requisitionDetail.Contingency);

                listOfReliefRequisitions.Add(newReliefRequisitionDetail);
                
            }
            return listOfReliefRequisitions;
        }

        public Cats.Rest.Models.ReliefRequisition Get(int id)
        {
            var reliefRequisition = _reliefRequisitionService.FindById(id);
            if (reliefRequisition!=null)
            {
                var newReliefRequisition = new ReliefRequisition(reliefRequisition.RequisitionID,
                                                                        reliefRequisition.CommodityID,
                                                                        reliefRequisition.Commodity.Name,
                                                                        reliefRequisition.RegionID,
                                                                        reliefRequisition.AdminUnit.Name,
                                                                        reliefRequisition.ZoneID,
                                                                        reliefRequisition.AdminUnit.Name,
                                                                        reliefRequisition.Round, reliefRequisition.Month,
                                                                        reliefRequisition.RequisitionNo,
                                                                        reliefRequisition.RequestedBy,
                                                                        GetUser(reliefRequisition.RequestedBy),
                                                                        reliefRequisition.RequestedDate,
                                                                        reliefRequisition.ApprovedBy,
                                                                        GetUser(reliefRequisition.ApprovedBy),
                                                                        reliefRequisition.ApprovedDate,
                                                                        reliefRequisition.Status,
                                                                        GetStatusName(reliefRequisition.Status),
                                                                        reliefRequisition.ProgramID,
                                                                        reliefRequisition.Program.Name,
                                                                        reliefRequisition.RegionalRequestID,
                                                                        reliefRequisition.RegionalRequestID,
                                                                        GetRationDetail(reliefRequisition.RationID),
                                                                     GetReliefRequisition((List<Cats.Models.ReliefRequisitionDetail>)reliefRequisition.ReliefRequisitionDetails));
                return newReliefRequisition;
            }
            return null;
        }
      /// <summary>
        /// Returns list of requsitions given year,programId,PlanId,round and month.
        /// If program is relief, parameter 'round' will be used or else if program is PSNP, parameter 'month' will be used
      /// </summary>
      /// <param name="year"></param>
      /// <param name="program"></param>
      /// <param name="planId"></param>
      /// <param name="round"></param>
      /// <param name="month"></param>
      /// <returns></returns>
        [System.Web.Http.HttpGet]
        public List<Models.RequisitionNo> RequisitionNos(int year, int program, int planId, int? round, int? month)
        {
            if (year != 0 && program != 0)
            {
                if (program == 1 || program == 3) //relief or idps
                {
                    var reqNo =
                        _reliefRequisitionService.FindBy(
                            r => r.Round == round && r.ProgramID == program && r.RegionalRequest.PlanID == planId && r.RegionalRequest.Year == year).ToList();
                    return
                        reqNo.Select(
                            reliefRequisition =>
                            new Models.RequisitionNo(reliefRequisition.RequisitionNo, reliefRequisition.RequisitionID)).
                            ToList();
                }
                if (program == 2) //psnp
                {
                    var reqNo =
                        _reliefRequisitionService.FindBy(
                            r => r.Month == month && r.ProgramID == program && r.RegionalRequest.PlanID == planId &&  r.RegionalRequest.Year == year).ToList();
                    return
                        reqNo.Select(
                            reliefRequisition =>
                            new Models.RequisitionNo(reliefRequisition.RequisitionNo, reliefRequisition.RequisitionID)).
                            ToList();

                }
            }
            return null;
        }
        /// <summary>
        /// Returns early warning allocation given Year,program, plan and Month/Round depending on the program
        /// </summary>
        /// <param name="year"></param>
        /// <param name="program"></param>
        /// <param name="planId"></param>
        /// <param name="round"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        [System.Web.Http.HttpGet]
        public IEnumerable<Models.Allocation> Allocation(int year, int program, int planId, int? round, int? month)
        {
            if (year != 0 && program != 0)
            {
             
              var regions = new List<int>();
              var listOfCommodityAllocations = new List<Allocation>();
                
                if (program == 1 || program == 3) //relief or idps
                {
                    var reqNo =
                        _regionalRequestService.FindBy(
                            r => r.Round == round && r.ProgramId == program && r.PlanID == planId && r.Year == year && r.Status > 1).ToList();

                    regions.AddRange(reqNo.Select(reliefRequisition => (int) reliefRequisition.RegionID).Distinct());

                    foreach (var region in regions)
                    {
                      var temp   = reqNo.Where(t => t.RegionID == region).ToList();
                        var commodities = new List<CommodityAllocation>();
                        decimal beneficiay = 0;
                        foreach (var regionalRequest in temp)
                        {

                            foreach (var rel in regionalRequest.RegionalRequestDetails)
                            {
                                foreach (var requestDetailCommodity in rel.RequestDetailCommodities)
                                {
                                    var commodity =
                                        new Models.CommodityAllocation(
                                            requestDetailCommodity.RegionalRequestDetail.RegionalRequest.RegionID,
                                            requestDetailCommodity.Amount,
                                            requestDetailCommodity.Commodity.Name,
                                            requestDetailCommodity.CommodityID);

                                    if (
                                        commodities.Any(
                                            c =>
                                            c.CommodityId == commodity.CommodityId))
                                    {
                                        var index = commodities.FindIndex(c => c.CommodityId == commodity.CommodityId);
                                        commodities[index].AllocatedAmount = commodities[index].AllocatedAmount +
                                                                             commodity.AllocatedAmount;

                                    }
                                    else
                                    {
                                        commodities.Add(commodity);
                                    }
                                }
                            }
                        }

                        var Allocatations =
                            temp.Select(
                                reliefRequisition =>
                                new Models.Allocation(reliefRequisition.RegionID, reliefRequisition.AdminUnit.Name,
                                                     commodities)).ToList();

                        listOfCommodityAllocations.AddRange(Allocatations);
                    }
                    var allocationGorupByRegion = listOfCommodityAllocations.GroupBy(g => g.RegionId)
                        .Select(r =>
                                    {
                                        var firstOrDefault = r.FirstOrDefault();
                                        return firstOrDefault != null ? new Models.Allocation(

                                                                                 firstOrDefault.RegionId,
                                                                                 firstOrDefault.RegionName,
                                                                                 r.FirstOrDefault().Commodities)

                                                                              : null;
                                    });






                    return  allocationGorupByRegion;
                }
                if (program == 2) //psnp
                {

                    var reqNo =
                         _regionalRequestService.FindBy(
                             r => r.Month == month && r.ProgramId == program && r.PlanID == planId && r.Year == year && r.Status > 1).ToList();

                    regions.AddRange(reqNo.Select(reliefRequisition => (int)reliefRequisition.RegionID).Distinct());

                    foreach (var region in regions)
                    {
                        var temp = reqNo.Where(t => t.RegionID == region).ToList();
                        var commodities = new List<CommodityAllocation>();
                        decimal beneficiay = 0;
                        foreach (var regionalRequest in temp)
                        {

                            foreach (var rel in regionalRequest.RegionalRequestDetails)
                            {
                                foreach (var requestDetailCommodity in rel.RequestDetailCommodities)
                                {
                                    var commodity =
                                        new Models.CommodityAllocation(
                                            requestDetailCommodity.RegionalRequestDetail.RegionalRequest.RegionID,
                                            requestDetailCommodity.Amount,
                                            requestDetailCommodity.Commodity.Name,
                                            requestDetailCommodity.CommodityID);

                                    if (
                                        commodities.Any(
                                            c =>
                                            c.CommodityId == commodity.CommodityId))
                                    {
                                        var index = commodities.FindIndex(c => c.CommodityId == commodity.CommodityId);
                                        commodities[index].AllocatedAmount = commodities[index].AllocatedAmount +
                                                                             commodity.AllocatedAmount;

                                    }
                                    else
                                    {
                                        commodities.Add(commodity);
                                    }
                                }
                            }
                        }

                        var Allocatations =
                            temp.Select(
                                reliefRequisition =>
                                new Models.Allocation(reliefRequisition.RegionID, reliefRequisition.AdminUnit.Name,
                                                      commodities)).ToList();

                        listOfCommodityAllocations.AddRange(Allocatations);
                    }
                    var allocationGorupByRegion = listOfCommodityAllocations.GroupBy(g => g.RegionId)
                        .Select(r =>
                        {
                            var firstOrDefault = r.FirstOrDefault();
                            return firstOrDefault != null ? new Models.Allocation(

                                                                     firstOrDefault.RegionId,
                                                                     firstOrDefault.RegionName,
                                                                   r.FirstOrDefault().Commodities)

                                                                  : null;
                        });






                    return allocationGorupByRegion;
                }
            }
            return null;
        }
    }
}
