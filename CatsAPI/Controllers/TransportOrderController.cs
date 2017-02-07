using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.Procurement;
using Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    public class TransportOrderController : ApiController
    {
        private readonly ITransportOrderService _transportOrderService;

        public TransportOrderController(ITransportOrderService transportOrderService)
        {
            _transportOrderService = transportOrderService;
        }
        // GET api/<controller>
        public IEnumerable<TransportOrder> GetTransportOrders()
        {
            return (from to in _transportOrderService.GetAllTransportOrder()
                select new TransportOrder()
                {
                    BidDocumentNo = to.BidDocumentNo,
                    ConsignerDate = to.ConsignerDate,
                    ConsignerName = to.ConsignerName,
                    ContractNumber = to.ContractNumber,
                    EndDate = to.EndDate,
                    OrderDate = to.OrderDate,
                    OrderExpiryDate = to.OrderExpiryDate,
                    PerformanceBondAmount = to.PerformanceBondAmount,
                    PerformanceBondReceiptNo = to.PerformanceBondReceiptNo,
                    RequestedDispatchDate = to.RequestedDispatchDate,
                    StatusID = to.StatusID,
                    StatusName = "",
                    TransporterID = to.TransporterID,
                    StartDate = to.StartDate,
                    TransportOrderID = to.TransportOrderID,
                    TransportOrderNo = to.TransportOrderNo,
                    TransportRequiqsitionId = to.TransportRequiqsitionId,
                    TransporterSignedDate = to.TransporterSignedDate,
                    TransporterSignedName = to.TransporterSignedName,
                    TransportOrderDetails = (from d in to.TransportOrderDetails
                        select new TransportOrderDetail()
                        {
                            CommodityName = d.Commodity!=null?d.Commodity.Name:string.Empty,
                            CommodityID = d.FdpID,
                            FdpName = d.FDP!=null?d.FDP.Name:string.Empty,
                            BidID = d.BidID,
                            TransportOrderID = d.TransportOrderID,
                            DonorID = d.DonorID,
                            FdpID = d.FdpID,
                            RequisitionNo = d.ReliefRequisition.RequisitionNo,
                            ZoneName = d.AdminUnit!=null?d.AdminUnit.Name:string.Empty,
                            RequisitionID = d.RequisitionID,
                            DistanceFromOrigin = d.DistanceFromOrigin,
                            DonorNamae = d.Donor!=null?d.Donor.Name:String.Empty,
                            IsChanged = d.IsChanged,
                            QuantityQuintal = d.QuantityQtl,
                            SourceWarehouseID = d.SourceWarehouseID,
                            WinnerAssignedByLogistics = d.WinnerAssignedByLogistics,
                            SourceWarehouseName = "",
                            TariffPerQuintal = d.TariffPerQtl,
                            TransportOrderDetailID = d.TransportOrderDetailID,
                            ZoneID = d.ZoneID
                        }).ToList(),
                }).ToList();
        }

        // GET api/<controller>/5
        public TransportOrder GetTransportOrder(int id)
        {
            var to = _transportOrderService.FindById(id);
            if (to == null) return null;
            return new TransportOrder()
            {
                BidDocumentNo = to.BidDocumentNo,
                ConsignerDate = to.ConsignerDate,
                ConsignerName = to.ConsignerName,
                ContractNumber = to.ContractNumber,
                EndDate = to.EndDate,
                OrderDate = to.OrderDate,
                OrderExpiryDate = to.OrderExpiryDate,
                PerformanceBondAmount = to.PerformanceBondAmount,
                PerformanceBondReceiptNo = to.PerformanceBondReceiptNo,
                RequestedDispatchDate = to.RequestedDispatchDate,
                StatusID = to.StatusID,
                StatusName = "",
                TransporterID = to.TransporterID,
                StartDate = to.StartDate,
                TransportOrderID = to.TransportOrderID,
                TransportOrderNo = to.TransportOrderNo,
                TransportRequiqsitionId = to.TransportRequiqsitionId,
                TransporterSignedDate = to.TransporterSignedDate,
                TransporterSignedName = to.TransporterSignedName,
                TransportOrderDetails = (from d in to.TransportOrderDetails
                    select new TransportOrderDetail()
                    {
                        CommodityName = d.Commodity != null ? d.Commodity.Name : string.Empty,
                        CommodityID = d.FdpID,
                        FdpName = d.FDP != null ? d.FDP.Name : string.Empty,
                        BidID = d.BidID,
                        TransportOrderID = d.TransportOrderID,
                        DonorID = d.DonorID,
                        FdpID = d.FdpID,
                        RequisitionNo = d.ReliefRequisition.RequisitionNo,
                        ZoneName = d.AdminUnit != null ? d.AdminUnit.Name : string.Empty,
                        RequisitionID = d.RequisitionID,
                        DistanceFromOrigin = d.DistanceFromOrigin,
                        DonorNamae = d.Donor != null ? d.Donor.Name : String.Empty,
                        IsChanged = d.IsChanged,
                        QuantityQuintal = d.QuantityQtl,
                        SourceWarehouseID = d.SourceWarehouseID,
                        WinnerAssignedByLogistics = d.WinnerAssignedByLogistics,
                        SourceWarehouseName = "",
                        TariffPerQuintal = d.TariffPerQtl,
                        TransportOrderDetailID = d.TransportOrderDetailID,
                        ZoneID = d.ZoneID
                    }).ToList(),
            };
        }

    }
}