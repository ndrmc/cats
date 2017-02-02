using System.Linq;
using System.Web.Http;
using Cats.Services.Procurement;
using Cats.Rest.Models;


namespace Cats.Rest.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class TransportOrderDetailController : ApiController
    {
        private readonly ITransportOrderDetailService _transportOrderDetailService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transportOrderDetailService"></param>
        public TransportOrderDetailController(ITransportOrderDetailService transportOrderDetailService)
        {
            _transportOrderDetailService = transportOrderDetailService;
        }
        // GET api/<controller>
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public dynamic GetTransportOrderDetails()
        {
            try
            {
                return _transportOrderDetailService.GetAllTransportOrderDetail().Select(d => new TransportOrderDetail()
                {
                    CommodityName = d.Commodity == null ? string.Empty : d.Commodity.Name,
                    CommodityID = d.FdpID,
                    FdpName = d.FDP == null ? string.Empty : d.FDP.Name,
                    BidID = d.BidID,
                    TransportOrderID = d.TransportOrderID,
                    DonorID = d.DonorID,
                    FdpID = d.FdpID,
                    RequisitionNo = d.ReliefRequisition == null ? string.Empty : d.ReliefRequisition.RequisitionNo,
                    ZoneName = d.AdminUnit == null ? string.Empty : d.AdminUnit.Name,
                    RequisitionID = d.RequisitionID,
                    DistanceFromOrigin = d.DistanceFromOrigin,
                    DonorNamae = d.Donor == null ? string.Empty : d.Donor.Name,
                    IsChanged = d.IsChanged,
                    QuantityQuintal = d.QuantityQtl,
                    SourceWarehouseID = d.SourceWarehouseID,
                    WinnerAssignedByLogistics = d.WinnerAssignedByLogistics,
                    SourceWarehouseName = string.Empty,
                    TariffPerQuintal = d.TariffPerQtl,
                    TransportOrderDetailID = d.TransportOrderDetailID,
                    ZoneID = d.ZoneID
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
        public dynamic GetTransportOrderDetail(int id)
        {
            var d = _transportOrderDetailService.FindById(id);
            return new TransportOrderDetail()
            {
                CommodityName = d.Commodity == null ? string.Empty : d.Commodity.Name,
                CommodityID = d.FdpID,
                FdpName = d.FDP == null ? string.Empty : d.FDP.Name,
                BidID = d.BidID,
                TransportOrderID = d.TransportOrderID,
                DonorID = d.DonorID,
                FdpID = d.FdpID,
                RequisitionNo = d.ReliefRequisition == null ? string.Empty : d.ReliefRequisition.RequisitionNo,
                ZoneName = d.AdminUnit == null ? string.Empty : d.AdminUnit.Name,
                RequisitionID = d.RequisitionID,
                DistanceFromOrigin = d.DistanceFromOrigin,
                DonorNamae = d.Donor == null ? string.Empty : d.Donor.Name,
                IsChanged = d.IsChanged,
                QuantityQuintal = d.QuantityQtl,
                SourceWarehouseID = d.SourceWarehouseID,
                WinnerAssignedByLogistics = d.WinnerAssignedByLogistics,
                SourceWarehouseName = string.Empty,
                TariffPerQuintal = d.TariffPerQtl,
                TransportOrderDetailID = d.TransportOrderDetailID,
                ZoneID = d.ZoneID
            };
        }
    }
}