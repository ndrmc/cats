using System.Linq;
using System.Web.Http;
using Cats.Rest.Models;
using Cats.Services.Logistics;

namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TransportRequisitionDetailController : ApiController
    {
        private readonly ITransportRequisitionDetailService _iTransportRequisitionDetailService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iTransportRequisitionDetailService"></param>
        public TransportRequisitionDetailController(ITransportRequisitionDetailService iTransportRequisitionDetailService)
        {
            _iTransportRequisitionDetailService = iTransportRequisitionDetailService;
        }
        // GET api/<controller>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetTransportRequisitionDetails()
        {
            try
            {
                return _iTransportRequisitionDetailService.GetAllTransportRequisitionDetail().Select(d => new TransportRequisitionDetailViewModel()
                {
                    RequisitionId = d.RequisitionID,
                    TransportRequisitionDetailId = d.TransportRequisitionDetailID,
                    RequisitionNo = d.TransportRequisition.TransportRequisitionNo,
                    TransportRequisitionId = d.TransportRequisitionID
                });
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        // GET api/<controller>/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetTransportRequisitionDetail(int id)
        {
            var d = _iTransportRequisitionDetailService.FindById(id);

            return new TransportRequisitionDetailViewModel()
            {
                RequisitionId = d.RequisitionID,
                TransportRequisitionDetailId = d.TransportRequisitionDetailID,
                RequisitionNo = d.TransportRequisition.TransportRequisitionNo,
                TransportRequisitionId = d.TransportRequisitionID
            };
        }
    }
}