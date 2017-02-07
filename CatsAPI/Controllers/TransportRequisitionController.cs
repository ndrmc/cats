using System.Collections.Generic;
using Cats.Models.Constant;
using Cats.Services.EarlyWarning;
using Cats.Services.Logistics;
using System.Linq;
using System.Web.Http;
using Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class TransportRequisitionController : ApiController
    {
        private readonly ITransportRequisitionService _iTransportRequisitionService;
        private readonly IAdminUnitService _iAdminUnitService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="iTransportRequisitionService"></param>
        public TransportRequisitionController(ITransportRequisitionService iTransportRequisitionService,IAdminUnitService iAdminUnitService)
        {
            _iTransportRequisitionService = iTransportRequisitionService;
            _iAdminUnitService = iAdminUnitService;
        }
        /// <summary>
        /// Returns list of TransportRequisition objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<TransportRequisition> GetTransportRequisitions()
        {
            var regions = _iAdminUnitService.GetRegions();

            var results = (from item in _iTransportRequisitionService.GetAllTransportRequisition()
                select new TransportRequisition()
                {
                    TransportRequisitionId = item.TransportRequisitionID,
                    TransportRequisitionNo = item.TransportRequisitionNo,
                    RegionId = item.RegionID,
                    RegionName = regions.Find(r => r.AdminUnitID == item.RegionID).Name,
                    ProgramId = item.ProgramID,
                    ProgramName = item.Program.Name,
                    RequestedBy = item.RequestedBy,
                    RequestedDate = item.RequestedDate,
                    CertifiedBy = item.CertifiedBy,
                    CertifiedDate = item.CertifiedDate,
                    Remark = item.Remark,
                    Status = item.Status,
                    StatusName = System.Enum.GetName(typeof (TransportRequisitionStatus), item.Status),
                    TransportRequisitionDetails = (from trd in item.TransportRequisitionDetails
                        select new TransportRequisitionDetail
                        {
                            RequisitionID = trd.RequisitionID,
                            RequisitionNo = trd.ReliefRequisition.RequisitionNo,
                            TransportRequisitionDetailID = trd.TransportRequisitionDetailID,
                            TransportRequisitionID = trd.TransportRequisitionID
                        }).ToList()
                }).ToList();

            return results;
        }
        /// <summary>
        /// Given an id returns a TransportRequisition object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public TransportRequisition GetTransportRequisition(int id)
        {
            var regions = _iAdminUnitService.GetRegions();
            var item = _iTransportRequisitionService.FindById(id);
                           return new TransportRequisition()
                           {
                               TransportRequisitionId = item.TransportRequisitionID,
                               TransportRequisitionNo = item.TransportRequisitionNo,
                               RegionId = item.RegionID,
                               RegionName = regions.Find(r => r.AdminUnitID == item.RegionID).Name,
                               ProgramId = item.ProgramID,
                               ProgramName = item.Program.Name,
                               RequestedBy = item.RequestedBy,
                               RequestedDate = item.RequestedDate,
                               CertifiedBy = item.CertifiedBy,
                               CertifiedDate = item.CertifiedDate,
                               Remark = item.Remark,
                               Status = item.Status,
                               StatusName = System.Enum.GetName(typeof(TransportRequisitionStatus), item.Status),
                               TransportRequisitionDetails = (from trd in item.TransportRequisitionDetails
                                                              select new TransportRequisitionDetail
                                                              {
                                                                  RequisitionID = trd.RequisitionID,
                                                                  RequisitionNo = trd.ReliefRequisition.RequisitionNo,
                                                                  TransportRequisitionDetailID = trd.TransportRequisitionDetailID,
                                                                  TransportRequisitionID = trd.TransportRequisitionID
                                                              }).ToList()
                           };
        }
    }
}


