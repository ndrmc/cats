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
    public class ReleifRequisitionController : ApiController
    {
        //
        // GET: /ReleifRequisition/

        private readonly IReliefRequisitionService _reliefRequisitionService;
        private IUserProfileService _profileService;
        private IStatusService _statusService;
        private IRationDetailService _rationDetail;

        public ReleifRequisitionController(IReliefRequisitionService reliefRequisitionService, IUserProfileService profileService, IStatusService statusService, IRationDetailService rationDetail)
        {
            _reliefRequisitionService = reliefRequisitionService;
            _profileService = profileService;
            _statusService = statusService;
            _rationDetail = rationDetail;
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
    }
}
