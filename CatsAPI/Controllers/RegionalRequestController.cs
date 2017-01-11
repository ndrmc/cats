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
                    DonorName = rr.Donor.Name,
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
                DonorName = regionalRequest.Donor.Name,
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


    }
}