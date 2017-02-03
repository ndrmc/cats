using Cats.Models.Constant;
using Cats.Services.EarlyWarning;
using Cats.Services.Logistics;
using System.Linq;
using System.Web.Http;

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
        public dynamic GetTransportRequisitions()
        {
            var regions = _iAdminUnitService.GetRegions();

            var results = _iTransportRequisitionService.GetAllTransportRequisition().Select(item => new
						   {
							TransportRequisitionId= item.TransportRequisitionID,
							item.TransportRequisitionNo,
                            RegionId=item.RegionID,
                            RegionName=  regions.Find(r=>r.AdminUnitID==item.RegionID).Name,
                            ProgramId=item.ProgramID,
                            ProgramName=item.Program.Name,
							item.RequestedBy,
							item.RequestedDate,
							item.CertifiedBy,
							item.CertifiedDate,
							item.Remark,
							item.Status,
							StatusName= System.Enum.GetName(typeof(TransportRequisitionStatus), item.Status),
							item.TransportRequisitionDetails
						  }).ToList();

            return results;
        }
        /// <summary>
        /// Given an id returns a TransportRequisition object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetTransportRequisition(int id)
        {
            var regions = _iAdminUnitService.GetRegions();
            var obj = _iTransportRequisitionService.FindById(id);
            var element = new
						 {
                TransportRequisitionId = obj.TransportRequisitionID,
                obj.TransportRequisitionNo,
                RegionId = obj.RegionID,
                RegionName = regions.Find(r => r.AdminUnitID == obj.RegionID).Name,
                ProgramId = obj.ProgramID,
                ProgramName = obj.Program.Name,
                obj.RequestedBy,
                obj.RequestedDate,
                obj.CertifiedBy,
                obj.CertifiedDate,
                obj.Remark,
                obj.Status,
                StatusName = System.Enum.GetName(typeof(TransportRequisitionStatus), obj.Status),
                obj.TransportRequisitionDetails
            };

            return element;
        }
    }
}


