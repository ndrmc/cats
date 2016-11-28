using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;

namespace Cats.Services.Procurement
{

    public class TransportOrderService : ITransportOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransporterService _transporterService;
        private readonly INotificationService _notificationService;
        private readonly IBusinessProcessService _businessProcessService;
        private readonly IApplicationSettingService _applicationSettingService;
        private readonly IStateTemplateService _stateTemplateService;
        private readonly IBusinessProcessStateService _businessProcessStateService;


        public TransportOrderService(IUnitOfWork unitOfWork, ITransporterService transporterService, INotificationService notificationService, IBusinessProcessService businessProcessService, IApplicationSettingService applicationSettingService, IStateTemplateService stateTemplateService, IBusinessProcessStateService paramBusinessProcessStateService)
        {
            this._unitOfWork = unitOfWork;
            this._transporterService = transporterService;
            _notificationService = notificationService;
            _businessProcessService = businessProcessService;

            _applicationSettingService = applicationSettingService;
            _stateTemplateService = stateTemplateService;
            _businessProcessStateService = paramBusinessProcessStateService;

        }

        #region Default Service Implementation
        public bool AddTransportOrder(TransportOrder transportOrder)
        {
            _unitOfWork.TransportOrderRepository.Add(transportOrder);
            _unitOfWork.Save();
            return true;

        }
        public bool EditTransportOrder(TransportOrder transportOrder)
        {
            _unitOfWork.TransportOrderRepository.Edit(transportOrder);
            _unitOfWork.Save();
            return true;

        }
        public bool DeleteTransportOrder(TransportOrder transportOrder)
        {
            if (transportOrder == null) return false;
            _unitOfWork.TransportOrderRepository.Delete(transportOrder);
            _unitOfWork.Save();
            return true;
        }
        public bool DeleteById(int id)
        {
            var entity = _unitOfWork.TransportOrderRepository.FindById(id);
            if (entity == null) return false;
            _unitOfWork.TransportOrderRepository.Delete(entity);
            _unitOfWork.Save();
            return true;
        }
        public List<TransportOrder> GetAllTransportOrder()
        {
            return _unitOfWork.TransportOrderRepository.GetAll();
        }
        public TransportOrder FindById(int id)
        {
            return _unitOfWork.TransportOrderRepository.FindById(id);
        }
        public List<TransportOrder> FindBy(Expression<Func<TransportOrder, bool>> predicate)
        {
            return _unitOfWork.TransportOrderRepository.FindBy(predicate);
        }

        public IEnumerable<TransportOrder> Get(
            Expression<Func<TransportOrder, bool>> filter = null,
            Func<IQueryable<TransportOrder>, IOrderedQueryable<TransportOrder>> orderBy = null,
            string includeProperties = "")
        {
            var test = _unitOfWork.TransportOrderRepository.Get(filter, orderBy, includeProperties);
            return _unitOfWork.TransportOrderRepository.Get(filter, orderBy, includeProperties);
        }

        public IEnumerable<TransportOrder> GetByHub(Expression<Func<TransportOrder, bool>> filter = null,
            Func<IQueryable<TransportOrder>, IOrderedQueryable<TransportOrder>> orderBy = null,
            string includeProperties = "", int hubId = 0, int statusId = 0)
        {
            //    var transportOrderDetail =
            //        ;
            var transportOrder = (
                from c in _unitOfWork.TransportOrderDetailRepository.FindBy(x => x.SourceWarehouseID == hubId)
                select c.TransportOrder).Where(x => x.StatusID == statusId).Distinct().ToList();
            return transportOrder;
        }
        public IEnumerable<TransportOrder> GetFilteredTransportOrder(IEnumerable<TransportRequisitionDetail> transportRequsitionDetails, string [] stateNames)
        {
            var transportRequistionDetail = transportRequsitionDetails.Select(m => m.RequisitionID).Distinct();
            var transportOrder =
             (from order in
                 _unitOfWork.TransportOrderDetailRepository.Get(
                     m =>
                         transportRequistionDetail.Contains(m.RequisitionID) &&
                         stateNames.Contains(m.TransportOrder.BusinessProcess.CurrentState.BaseStateTemplate.Name), null,
                     "FDP, FDP.AdminUnit, FDP.AdminUnit.AdminUnit2, FDP.AdminUnit.AdminUnit2.AdminUnit2").ToList()
              select order.TransportOrder).Distinct().ToList();

            return transportOrder;
        }
        public IEnumerable<TransportOrder> GetFilteredTransportOrder(IEnumerable<TransportOrderDetail> transportOrderDetails, int statusId)
        {

            var transportOrder =
                (from order in
                     transportOrderDetails
                 select order.TransportOrder).Where(m => m.StatusID == statusId).Distinct().ToList();
            return transportOrder;
        }
        public List<Program> GetPrograms()
        {
            return _unitOfWork.ProgramRepository.GetAll();
        }

        #endregion

        public IOrderedEnumerable<RequisiionNoViewModel> GetZone(int transReqNo)
        {
            switch (transReqNo)
            {
                case 0:
                    {
                        var requisition =
                            _unitOfWork.TransReqWithoutTransporterRepository.FindBy(m => m.IsAssigned == false).
                                OrderByDescending(
                                    t => t.TransportRequisitionDetailID).Select(s => new
                                    {
                                        ZoneId =
                                                                                         s.ReliefRequisitionDetail.
                                                                                         ReliefRequisition.ZoneID,
                                        ZoneName =
                                                                                         s.ReliefRequisitionDetail.
                                                                                         ReliefRequisition.AdminUnit1.Name
                                    }).Distinct().ToList();
                        return requisition.Select(req => new RequisiionNoViewModel
                        {
                            ZoneId = (int)req.ZoneId,
                            ZoneName = req.ZoneName
                        }).OrderBy(r => r.ZoneId);
                    }
                default:
                    {
                        var requisition =
                            _unitOfWork.TransReqWithoutTransporterRepository.FindBy(
                                m =>
                                m.IsAssigned == false &&
                                m.TransportRequisitionDetail.TransportRequisition.TransportRequisitionID == transReqNo).
                                OrderByDescending(
                                    t => t.TransportRequisitionDetailID).Select(s => new
                                    {
                                        ZoneId =
                                                                                         s.ReliefRequisitionDetail.
                                                                                         ReliefRequisition.ZoneID,
                                        ZoneName =
                                                                                         s.ReliefRequisitionDetail.
                                                                                         ReliefRequisition.AdminUnit1.Name
                                    }).Distinct().ToList();
                        return requisition.Select(req => new RequisiionNoViewModel
                        {
                            ZoneId = (int)req.ZoneId,
                            ZoneName = req.ZoneName
                        }).OrderBy(r => r.ZoneId);
                    }
            }
        }

        public IOrderedEnumerable<WoredaViewModelInTransReqWithoutWinner> GetWoredas(int zoneId, int transReqNo)
        {
            var requisition =
                _unitOfWork.TransReqWithoutTransporterRepository.FindBy(m => m.IsAssigned == false &&
                    m.ReliefRequisitionDetail.ReliefRequisition.ZoneID == zoneId &&
                    m.TransportRequisitionDetail.TransportRequisition.TransportRequisitionID == transReqNo).
                    OrderByDescending(
                        t => t.TransportRequisitionDetailID).Select(s => new
                        {
                            woredaId = s.ReliefRequisitionDetail.FDP.AdminUnitID,
                            woredaName = s.ReliefRequisitionDetail.FDP.AdminUnit.Name
                        }).Distinct().ToList();
            return requisition.Select(req => new WoredaViewModelInTransReqWithoutWinner()
            {
                WoredaId = (int)req.woredaId,
                WoredaName = req.woredaName
            }).OrderBy(r => r.WoredaId);


        }
        public IOrderedEnumerable<RegionsViewModel> GetRegions()
        {
            var regions = _unitOfWork.AdminUnitRepository.FindBy(t => t.AdminUnitTypeID == 2).ToList();

            return regions.Select(adminUnit => new RegionsViewModel
            {
                Name = adminUnit.Name,
                AdminUnitID = adminUnit.AdminUnitID
            }).OrderBy(e => e.Name);

        }

        public void Dispose()
        {
            _unitOfWork.Dispose();

        }





        //public IEnumerable<RequisitionToDispatch> GetRequisitionToDispatch()
        //{
        //    var requisitions = GetProjectCodeAssignedRequisitions();

        //    var result = (from requisition in requisitions
        //                  select new RequisitionToDispatch
        //                             {
        //                                 HubID =requisition.HubAllocations.FirstOrDefault().HubID,
        //                                 RequisitionID = requisition.RequisitionID,
        //                                 RequisitionNo = requisition.RequisitionNo,
        //                                 RequisitionStatus = requisition.Status.Value,
        //                                 ZoneID = requisition.ZoneID.Value,
        //                                 QuanityInQtl = requisition.ReliefRequisitionDetails.Sum(m => m.Amount),
        //                                 OrignWarehouse = requisition.HubAllocations.FirstOrDefault().Hub.Name,
        //                                 CommodityID = requisition.CommodityID.Value,
        //                                 CommodityName = requisition.Commodity.Name,
        //                                 Zone = requisition.AdminUnit.Name,
        //                                 RegionID = requisition.RegionID.Value,
        //                                 RegionName = requisition.AdminUnit1.Name,



        //                             });


        //    return result;
        //}


        //public IEnumerable<ReliefRequisition> GetProjectCodeAssignedRequisitions()
        //{
        //    return _unitOfWork.ReliefRequisitionRepository.Get(t => t.Status == (int)REGIONAL_REQUEST_STATUS.HubAssigned, null,
        //                                                  "HubAllocations,ReliefRequisitionDetails,Program,AdminUnit1,AdminUnit,Commodity");
        //}

        //public IEnumerable<ReliefRequisitionDetail> GetProjectCodeAssignedRequisitionDetails()
        //{
        //    return _unitOfWork.ReliefRequisitionDetailRepository.Get(t => t.ReliefRequisition.Status == (int)REGIONAL_REQUEST_STATUS.HubAssigned, null,
        //                                                  "ReliefRequisition");
        //}

        public IEnumerable<TransportOrderDetail> GetTransportOrderDetail(int requisitionId)
        {
            return _unitOfWork.TransportOrderDetailRepository.Get(t => t.RequisitionID == requisitionId);
        }

        public IEnumerable<ReliefRequisition> GetTransportOrderReleifRequisition(int status)
        {
            return _unitOfWork.ReliefRequisitionRepository.Get(r => r.Status == 6); //This will return reuisitions where thier transport order is made
        }

        public IEnumerable<TransportOrderDetail> GetTransportOrderDetailByTransportId(int transportId)
        {
            return _unitOfWork.TransportOrderDetailRepository.Get(t => t.TransportOrderID == transportId);
        }
        //TODO:Factor Out  to single responiblity Principle 
        public bool CreateTransportOrder(int transportRequisitionId, int bidId, string requesterName)
        {
            //var requId=_unitOfWork.TransportRequisitionDetailRepository.FindBy(t=>t.TransportRequisitionID==)
            var transporterAssignedRequisionDetails = AssignTransporterForEachWoreda(transportRequisitionId, bidId);

            var transporters = (from item in transporterAssignedRequisionDetails select item.TransporterID).Distinct().ToList();

            //If we reached here all location got transporter 
            var transportOrders = new List<TransportOrder>();


            foreach (var transporter in transporters)
            {
                //TODO:Check what all number should come from and implement
                var transportOrder = new TransportOrder();
                transportOrder.TransporterID = transporter;
                transportOrder.OrderDate = DateTime.Today;
                transportOrder.StartDate = DateTime.Today;
                transportOrder.EndDate = DateTime.Today;
                transportOrder.TransportOrderNo = Guid.NewGuid().ToString();
                transportOrder.OrderExpiryDate = DateTime.Today.AddDays(10);

                transportOrder.PerformanceBondReceiptNo = "PERFORMANCE-BOND-NO";
                //var transporterName = _unitOfWork.TransporterRepository.FindById(transporter).Name;
                transportOrder.ContractNumber = Guid.NewGuid().ToString();
                //string.Format("{0}/{1}/{2}/{3}", "LTCD", DateTime.Today.day, DateTime.Today.Year, transporterName.Substring(0, 1));
                transportOrder.TransporterSignedDate = DateTime.Today;
                transportOrder.RequestedDispatchDate = DateTime.Today;
                transportOrder.ConsignerDate = DateTime.Today;
                transportOrder.StartDate = DateTime.Today.AddDays(3);
                transportOrder.EndDate = DateTime.Today.AddDays(13);
                transportOrder.StatusID = (int)TransportOrderStatus.Draft;
                transportOrder.TransportRequiqsitionId = transportRequisitionId;
                int businessProcessID = 0;
                int BP_PR = _applicationSettingService.getTransportOrderWorkflow();
                if (BP_PR != 0)
                {
                    BusinessProcessState createdstate = new BusinessProcessState
                    {
                        DatePerformed = DateTime.Now,
                        PerformedBy = requesterName,
                        Comment = "Transport Order Generated"

                    };
                    //_PaymentRequestservice.Create(request);

                    BusinessProcess bp = _businessProcessService.CreateBusinessProcess(BP_PR, 0,
                                                                                    "TransportOrder", createdstate);
                    if (bp != null)
                        businessProcessID = bp.BusinessProcessID;


                }

                transportOrder.BusinessProcessID = businessProcessID;
                var transportLocations = transporterAssignedRequisionDetails.FindAll(t => t.TransporterID == transporter).Distinct();



                foreach (var transporterRequisition in transportLocations)
                {

                    var transReq = transporterRequisition;

                    //var activeBidStatusID = int.Parse(BidStatus.Active.ToString());
                    //var bidWinner =
                    //_unitOfWork.BidWinnerRepository.Get(
                    //    t => t.SourceID == transReq.HubID && t.DestinationID == transReq.WoredaID && t.Position == 1 &&
                    //        t.Bid.StatusID == 5).FirstOrDefault();
                    var bidWinner = _unitOfWork.BidWinnerRepository.Get(t => t.SourceID == transReq.HubID && t.DestinationID == transReq.WoredaID && t.Position == 1 && t.BidID == bidId).FirstOrDefault();


                    if (bidWinner != null)
                    {
                        transportOrder.BidDocumentNo = _unitOfWork.BidRepository.FindById(bidWinner.BidID).BidNumber;
                    }
                    else
                    {
                        transportOrder.BidDocumentNo = "Bid-Number";
                        //_unitOfWork.BidWinnerRepository.FindById(transporter).Bid.BidNumber;
                    }

                    var requisionsDetails =
                        _unitOfWork.ReliefRequisitionDetailRepository.Get(
                            t =>
                            t.RequisitionID == transReq.RequisitionID &&
                            t.FDP.AdminUnitID == transReq.WoredaID, null, "ReliefRequisition").ToList();

                    foreach (var reliefRequisitionDetail in requisionsDetails)
                    {
                        var transportOrderDetail = new TransportOrderDetail();
                        //transportOrderDetail.ZoneID = reliefRequisitionDetail.ReliefRequisition.ZoneID;
                        transportOrderDetail.CommodityID = reliefRequisitionDetail.CommodityID;
                        transportOrderDetail.FdpID = reliefRequisitionDetail.FDPID;
                        transportOrderDetail.RequisitionID = reliefRequisitionDetail.RequisitionID;
                        if (bidWinner != null) transportOrderDetail.BidID = bidWinner.BidID;
                        // divide Commodity amount equaly if there is more than one winner for the same woreda
                        if (transReq.noOfWinners > 1)

                            transportOrderDetail.QuantityQtl = (reliefRequisitionDetail.Amount / transReq.noOfWinners);

                        else
                            transportOrderDetail.QuantityQtl = reliefRequisitionDetail.Amount;
                        transportOrderDetail.TariffPerQtl = transReq.TariffPerQtl;
                        transportOrderDetail.SourceWarehouseID = transReq.HubID;
                        transportOrder.TransportOrderDetails.Add(transportOrderDetail);
                    }

                }
                _unitOfWork.TransportOrderRepository.Add(transportOrder);
                transportOrders.Add(transportOrder);


            }

            var requisition = _unitOfWork.TransportRequisitionRepository.Get(t => t.TransportRequisitionID == transportRequisitionId).FirstOrDefault();
            //var requisition = _unitOfWork.TransportRequisitionRepository.Get(t => t.TransportRequisitionID == transportRequisitionId).FirstOrDefault();

            var transportRequisition = _unitOfWork.TransportRequisitionRepository.Get(t => t.TransportRequisitionID == transportRequisitionId, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (transportRequisition != null)
            {
                var closeFlowTemplate = transportRequisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Close");
                if (closeFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = closeFlowTemplate.FinalStateID,
                        PerformedBy = requesterName,
                        DatePerformed = DateTime.Now,
                        Comment = "Transport requisition has been closed.",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = transportRequisition.BusinessProcessID
                    };
                    //return 
                    _businessProcessService.PromotWorkflow(businessProcessState);
                }
            }


            //requisition.Status = (int)TransportRequisitionStatus.Closed;

            var transportRequisitionDetails =
                _unitOfWork.TransportRequisitionDetailRepository.Get(t => t.TransportRequisitionID == transportRequisitionId).ToList();
            foreach (var transportRequisitionDetail in transportRequisitionDetails)
            {
                var reliefRequisition = _unitOfWork.ReliefRequisitionRepository.Get(t => t.RequisitionID == transportRequisitionDetail.RequisitionID, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
                var approveFlowTemplate = reliefRequisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Create Transport Order");
                if (approveFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = approveFlowTemplate.FinalStateID,
                        PerformedBy = requesterName,
                        DatePerformed = DateTime.Now,
                        Comment = "Transport order has been created for the requisition.",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = reliefRequisition.BusinessProcessID
                    };
                    //return 
                    _businessProcessService.PromotWorkflow(businessProcessState);
                }
            }

            _unitOfWork.Save();
            //TODO:Identity if Transport order number to be auto generated , and where to get contract number.

            foreach (var transportOrder in transportOrders)
            {
                var transporterName = _unitOfWork.TransporterRepository.FindById(transportOrder.TransporterID).Name;
                transportOrder.TransportOrderNo = string.Format("TRN-ORD-{0}", transportOrder.TransportOrderID);
                transportOrder.ContractNumber = string.Format("{0}/{1}/{2}/{3}/{4}", "LTCD", transportRequisition.RegionID, DateTime.Today.Year, transporterName.Substring(0, 2), transportRequisition.TransportRequisitionNo);
            }

            _unitOfWork.Save();





            return true;
        }




        private List<TransporterRequisition> AssignTransporterForEachWoreda(int transportRequisitionId, int bidId)
        {

            var transportRequision = _unitOfWork.TransportRequisitionDetailRepository.Get(
                t => t.TransportRequisitionID == transportRequisitionId, null, null).Select(t => t.RequisitionID);
            var reqDetails = _unitOfWork.ReliefRequisitionDetailRepository.Get(t => transportRequision.Contains(t.RequisitionID));
            var transportSourceDestination = new List<TransporterRequisition>();
            foreach (var reliefRequisitionDetail in reqDetails)
            {
                //var transportRequisition = new TransporterRequisition();
                //var requi =
                //    _unitOfWork.ReliefRequisitionRepository.Get(
                //        t => t.RequisitionID == reliefRequisitionDetail.RequisitionID, null, "HubAllocations").FirstOrDefault();
                var transRequisDetailId = reliefRequisitionDetail.ReliefRequisition.TransportRequisitionDetails.First().TransportRequisitionDetailID;
                var reqId = reliefRequisitionDetail.RequisitionID;
                //var storeId = _unitOfWork.HubAllocationRepository.FindBy(t => t.RequisitionID == reliefRequisitionDetail.RequisitionID).FirstOrDefault().StoreId;
                var hubId = _unitOfWork.SIPCAllocationRepository.FindBy(t => t.ReliefRequisitionDetail.RequisitionID == reliefRequisitionDetail.RequisitionID).FirstOrDefault().HubID;//requi.HubAllocations.FirstOrDefault().HubID;
                var sWarehouseID =
                    _unitOfWork.HubAllocationRepository.FindBy(
                        t => t.RequisitionID == reliefRequisitionDetail.RequisitionID)
                        .FirstOrDefault()
                        .SatelliteWarehouseID;
                int temp = hubId;
                var woredaId = reliefRequisitionDetail.FDP.AdminUnitID;
                var regionId = reliefRequisitionDetail.ReliefRequisition.RegionID;

                //transportRequisition.TransportRequisitionDetailID = reliefRequisitionDetail.ReliefRequisition.TransportRequisitionDetails.First().TransportRequisitionDetailID;
                //transportRequisition.RequisitionID = reliefRequisitionDetail.RequisitionID;
                //transportRequisition.HubID = _unitOfWork.HubAllocationRepository.FindBy(t => t.RequisitionID == reliefRequisitionDetail.RequisitionID).FirstOrDefault().HubID;//requi.HubAllocations.FirstOrDefault().HubID;
                //transportRequisition.WoredaID = reliefRequisitionDetail.FDP.AdminUnitID;
                if (sWarehouseID != null && sWarehouseID != 0)
                {
                    temp = (int)sWarehouseID;
                }
                var transportBidWinners = _transporterService.GetBidWinner(hubId, woredaId, bidId);


                //_unitOfWork.BidWinnerRepository.Get(
                //    t => t.SourceID == transportRequisition.HubID && t.DestinationID == transportRequisition.WoredaID).FirstOrDefault();
                if (transportBidWinners.Count == 0)
                {
                    var transReqWithoutTransporter = new TransReqWithoutTransporter();
                    transReqWithoutTransporter.TransportRequisitionDetailID = transRequisDetailId;
                    transReqWithoutTransporter.RequisitionDetailID = reliefRequisitionDetail.RequisitionDetailID;
                    transReqWithoutTransporter.IsAssigned = false;
                    _unitOfWork.TransReqWithoutTransporterRepository.Add(transReqWithoutTransporter);
                    _unitOfWork.Save();
                    //throw new Exception(string.Format("Transporter Couldn't be found for from {0} to {1}", transportRequisition.HubID, transportRequisition.WoredaID));
                }
                else
                {
                    //TODO: these commented lines should be figured out how they affect the rest of the code
                    foreach (var transportBidWinner in transportBidWinners)
                    {
                        var transportRequisition = new TransporterRequisition();
                        transportRequisition.TransportRequisitionDetailID = transRequisDetailId;
                        transportRequisition.RequisitionID = reqId;
                        transportRequisition.HubID = hubId;
                        transportRequisition.WoredaID = woredaId;
                        //transportRequisition.BidID = transportBidWinner.BidID;
                        transportRequisition.TransporterID = transportBidWinner.TransporterID;
                        transportRequisition.TariffPerQtl = transportBidWinner.Tariff != null ? (decimal)transportBidWinner.Tariff : 0;
                        transportRequisition.noOfWinners = transportBidWinners.Count;
                        transportRequisition.RegionID = (int)regionId;

                        transportSourceDestination.Add(transportRequisition);
                    }

                }

            }
            var groupedTransportSourceDestination = transportSourceDestination.GroupBy(ac => new
            {
                ac.HubID,
                ac.RequisitionID,
                ac.TransporterID,
                ac.TransportRequisitionDetailID,
                ac.WoredaID
            }).Select(ac => new TransporterRequisition
            {
                HubID = ac.Key.HubID,
                RequisitionID = ac.Key.RequisitionID,
                TariffPerQtl = ac.FirstOrDefault().TariffPerQtl,
                noOfWinners = ac.FirstOrDefault().noOfWinners,
                TransporterID = ac.Key.TransporterID,
                TransportRequisitionDetailID = ac.Key.TransportRequisitionDetailID,
                WoredaID = ac.Key.WoredaID
            }).ToList();
            return groupedTransportSourceDestination;
        }

        public List<vwTransportOrder> GeTransportOrderRpt(int id)
        {
            return _unitOfWork.VwTransportOrderRepository.Get(t => t.TransportOrderID == id).ToList();
        }
        public List<Transporter> GetTransporter()
        {
            return _unitOfWork.TransporterRepository.GetAll();
        }


        private int AddToCurrentTransport(IEnumerable<TransportRequisitionWithoutWinnerModel> transReqWithTransporter, int transporterId)
        {
            try
            {

                var transportOrder = new TransportOrder();


                var transReq = transReqWithTransporter as List<TransportRequisitionWithoutWinnerModel> ?? transReqWithTransporter.ToList();
                foreach (var detail in transReq)
                {
                    var transportReq = _unitOfWork.TransReqWithoutTransporterRepository.FindById(detail.TransReqWithoutTransporterID);
                    //transportOrder =
                    //    _unitOfWork.TransportOrderDetailRepository.FindBy(
                    //        t => t.RequisitionID == transportReq.ReliefRequisitionDetail.RequisitionID && t.TransportOrder.TransporterID == transporterId &&
                    //    t.TransportOrder.StatusID == (int)TransportOrderStatus.Draft).Select(
                    //            t => t.TransportOrder).FirstOrDefault();
                    TransportRequisitionWithoutWinnerModel detail1 = detail;
                    transportOrder =
                      _unitOfWork.TransportOrderDetailRepository.FindBy(t => t.TransportOrder.TransporterID == transporterId && t.TransportOrder.TransportRequiqsitionId == transportReq.TransportRequisitionDetail.TransportRequisitionID &&
                      t.TransportOrder.StatusID == (int)TransportOrderStatus.Draft).Select(
                              t => t.TransportOrder).FirstOrDefault();


                    if (transportOrder == null) continue;
                    var transportOrderDetail = new TransportOrderDetail
                    {
                        CommodityID = detail.CommodityID,
                        FdpID = detail.FdpID,
                        RequisitionID = detail.RequisitionID,
                        QuantityQtl = detail.QuantityQtl,
                        TariffPerQtl = 0,
                        SourceWarehouseID = detail.HubID,
                        WinnerAssignedByLogistics = true
                    };
                    transportOrder.TransportOrderDetails.Add(transportOrderDetail);

                }

                bool isSaved = _unitOfWork.TransportOrderRepository.Edit(transportOrder);
                _unitOfWork.Save();
                if (isSaved)
                {

                    foreach (var item in transReq)

                    {
                        var withoutTransporter =
                            _unitOfWork.TransReqWithoutTransporterRepository.FindById(item.TransReqWithoutTransporterID);
                        withoutTransporter.IsAssigned = true;
                        _unitOfWork.TransReqWithoutTransporterRepository.Edit(withoutTransporter);
                        _unitOfWork.Save();
                    }

                }
                if (transportOrder != null) return transportOrder.TransportOrderID;
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public int ReAssignTransporter(IEnumerable<TransportRequisitionWithoutWinnerModel> transReqWithTransporter, int transporterID)
        {
            if (transReqWithTransporter != null && transporterID != 0)
            {

                var result = AddToCurrentTransport(transReqWithTransporter, transporterID);
                if (result == -1)

                {

                    var transportOrder = new TransportOrder();
                    transportOrder.TransporterID = transporterID;
                    transportOrder.OrderDate = DateTime.Today;
                    transportOrder.StartDate = DateTime.Today;
                    transportOrder.EndDate = DateTime.Today;
                    transportOrder.TransportOrderNo = Guid.NewGuid().ToString();
                    transportOrder.OrderExpiryDate = DateTime.Today.AddDays(10);
                    var currentBid = _unitOfWork.BidRepository.FindBy(t => t.StatusID == 3).FirstOrDefault();
                    var transporterName = _unitOfWork.TransporterRepository.FindById(transportOrder.TransporterID).Name;
                    if (currentBid != null)
                    {
                        var bidID = currentBid.BidID;
                        transportOrder.BidDocumentNo = _unitOfWork.BidRepository.FindById(bidID).BidNumber;
                    }
                    else
                    {
                        transportOrder.BidDocumentNo = "Bid-Number";

                    }

                    var transRequisition =
                        _unitOfWork.TransportRequisitionDetailRepository.FindById(
                            transReqWithTransporter.FirstOrDefault().TransportRequisitionID).TransportRequisition;
                    transportOrder.TransportRequiqsitionId = transRequisition.TransportRequisitionID;
                    transportOrder.PerformanceBondReceiptNo = "PERFORMANCE-BOND-NO";
                    //var transporterName = _unitOfWork.TransporterRepository.FindById(transporter).Name;
                    transportOrder.ContractNumber = Guid.NewGuid().ToString();
                    //string.Format("{0}/{1}/{2}/{3}", "LTCD", DateTime.Today.day, DateTime.Today.Year, transporterName.Substring(0, 1));
                    transportOrder.TransporterSignedDate = DateTime.Today;
                    transportOrder.RequestedDispatchDate = DateTime.Today;
                    transportOrder.ConsignerDate = DateTime.Today;
                    transportOrder.StatusID = (int)TransportOrderStatus.Draft;
                    var lastOrder = _unitOfWork.TransportOrderRepository.GetAll();
                    if (lastOrder.Count != 0)
                    {
                        transportOrder.TransportOrderNo = string.Format("TRN-ORD-{0}",
                                                                        lastOrder.Last().TransportOrderID + 1);
                    }
                    else
                    {
                        transportOrder.TransportOrderNo = string.Format("TRN-ORD-{0}", 1);
                    }
                    transportOrder.ContractNumber = string.Format("{0}/{1}/{2}/{3}/{4}", "LTCD",
                                                                  transRequisition.RegionID,
                                                                  DateTime.Today.Year, transporterName.Substring(0, 3),
                                                                  transRequisition.TransportRequisitionNo);

                    foreach (var detail in transReqWithTransporter)
                    {
                        var transportOrderDetail = new TransportOrderDetail();
                        transportOrderDetail.CommodityID = detail.CommodityID;
                        transportOrderDetail.FdpID = detail.FdpID;
                        transportOrderDetail.RequisitionID = detail.RequisitionID;
                        transportOrderDetail.QuantityQtl = detail.QuantityQtl;
                        //since users don't specify tariff value
                        transportOrderDetail.TariffPerQtl = 0;
                        transportOrderDetail.SourceWarehouseID = detail.HubID;
                        transportOrderDetail.WinnerAssignedByLogistics = true;
                        transportOrder.TransportOrderDetails.Add(transportOrderDetail);
                    }
                    bool isSaved = _unitOfWork.TransportOrderRepository.Add(transportOrder);
                    _unitOfWork.Save();
                    if (isSaved)
                    {
                        foreach (var item in transReqWithTransporter)
                        {
                            var withoutTransporter =
                                _unitOfWork.TransReqWithoutTransporterRepository.FindById(
                                    item.TransReqWithoutTransporterID);
                            withoutTransporter.IsAssigned = true;
                            _unitOfWork.TransReqWithoutTransporterRepository.Edit(withoutTransporter);
                            _unitOfWork.Save();
                        }

                    }

                    return transportOrder.TransportOrderID;

                }
                return result;
            }
            return 0;

        }

        private class TransporterRequisition
        {
            public int HubID { get; set; }
            public int WoredaID { get; set; }
            public int RequisitionID { get; set; }
            public int TransporterID { get; set; }
            public decimal TariffPerQtl { get; set; }
            public int TransportRequisitionDetailID { get; set; }
            public int noOfWinners { get; set; }
            public int? RegionID { get; set; }
            //public int? BidID { get; set; }

            //public TransporterRequisition()
            //{
            //    TransporterIDs = new List<int>();
            //}

        }

        public List<Hub> GetHubs()
        {
            return _unitOfWork.HubRepository.GetAll();
        }
        public bool ApproveTransportOrder(TransportOrder transportOrder, string userName)
        {
            if (transportOrder != null)
            {
                try
                {
                    var hubId = new List<int>();
                    var transport = transportOrder.TransportOrderDetails.Select(c => c.SourceWarehouseID).ToList().Distinct();
                    {
                        hubId.AddRange(transport);
                    }
                    AddToNotification(transportOrder.TransportOrderID, transportOrder.TransportOrderNo, hubId);
                }
                catch
                {




                }

                //  transportOrder.StatusID = (int)TransportOrderStatus.Approved;

                var approvedStateId =
                              _stateTemplateService
                                  .GetAll().FirstOrDefault(s => s.ParentProcessTemplateID == transportOrder.BusinessProcess.CurrentState.BaseStateTemplate.ParentProcessTemplateID && s.Name == "Approved");

                var bp = _businessProcessService.GetAll().FirstOrDefault(t => t.BusinessProcessID == transportOrder.BusinessProcessID);

                if (approvedStateId != null)
                {
                    var createdstate3 = new BusinessProcessState
                    {
                        DatePerformed = DateTime.Now,
                        PerformedBy = userName,
                        Comment = " TransportOrder Approved on multiple approval",
                        StateID = approvedStateId.StateTemplateID,
                        ParentBusinessProcessID = transportOrder.BusinessProcessID,

                    };
                    //_businessProcessStateService.Add(createdstate3);

                    //if (bp != null)
                    //{
                    //    bp.CurrentState = createdstate3;
                    //    _businessProcessService.Update(bp);
                    //}
                    _businessProcessService.PromotWorkflow(createdstate3);
                }

                //_unitOfWork.TransportOrderRepository.Edit(transportOrder);
                //_unitOfWork.Save();

                return true;
            }
            return false;

        }

        public bool SignTransportOrder(TransportOrder transportOrder)
        {
            if (transportOrder != null)
            {
                transportOrder.StatusID = (int)TransportOrderStatus.Signed;
                _unitOfWork.TransportOrderRepository.Edit(transportOrder);
                _unitOfWork.Save();

                return true;
            }
            return false;

        }
        public List<DispatchAllocation> GeneratedDispatchAllocations = new List<DispatchAllocation>();

        public List<DispatchAllocation> GetGeneratedDispatchAllocations()
        {
            return GeneratedDispatchAllocations;
        }

        public void ClearGeneratedDispatchAllocations()
        {
              GeneratedDispatchAllocations.Clear();
        }
        public bool GeneratDispatchPlan(int transportOrderId, string UserName)
        {

            var transportOrder =
                            _unitOfWork.TransportOrderRepository.Get(
                                t =>
                                t.TransportOrderID == transportOrderId && t.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Signed", null, "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (transportOrder == null) return false;
            int appServId = _applicationSettingService.GetDispatchWorkflow();


            var transportOrderDetails =
                _unitOfWork.TransportOrderDetailRepository.Get(t => t.TransportOrderID == transportOrderId, null, "ReliefRequisition").ToList();

            foreach (var transportOrderDetail in transportOrderDetails)
            {
                var requisition = transportOrderDetail.ReliefRequisition;
                var fdpId = transportOrderDetail.FdpID;
                var requisitionDetail =
                    _unitOfWork.ReliefRequisitionDetailRepository.Get(
                        t => t.RequisitionID == requisition.RequisitionID && t.FDPID == fdpId).FirstOrDefault();



                var sipc =
                   _unitOfWork.SIPCAllocationRepository.FindBy(
                       t => t.RequisitionDetailID == requisitionDetail.RequisitionDetailID);
                foreach (var t in sipc)
                {
                    /****** BEGIN: This Calculates the already dispatched amount of the FDP by former transporter if there is any **********/
                    decimal sumOfdispatchedQty = 0.00M;
                    var prevDispatchAllocation = _unitOfWork.DispatchAllocationRepository.FindBy(d => d.RequisitionId == requisition.RequisitionID && d.FDPID == fdpId &&
                                                        d.ShippingInstructionID == t.Code && d.HubID == t.HubID).FirstOrDefault();


                    if (prevDispatchAllocation != null)
                    {
                        sumOfdispatchedQty = prevDispatchAllocation.Dispatches.Select(dispatch => dispatch?.DispatchDetails).Where(result => result?.Count > 0)
                            .Aggregate(0.00M, (current1, result) => result.Aggregate(current1, (current, dispatchDetail) => current + dispatchDetail.RequestedQuantityInMT));

                        //var prevDispatchAllocationChildren = _unitOfWork.DispatchAllocationRepository.FindBy(d=>d.ParentDispatchAllocationID == prevDispatchAllocation.DispatchAllocationID && 
                        //                                    d.RequisitionId == requisition.RequisitionID && d.FDPID == fdpId && d.HubID == t.HubID).ToList();

                        //if (prevDispatchAllocationChildren.Count > 0)
                        //{
                        //    foreach (var prevDispatchAllocationChild in prevDispatchAllocationChildren)
                        //    {
                        //        var orDefault = prevDispatchAllocationChild.Dispatches;
                        //        foreach (var dispatch in orDefault)
                        //        {
                        //            var result =
                        //                dispatch?.DispatchDetails;

                        //            if (result?.Count > 0)
                        //            {
                        //                sumOfdispatchedQty = result.Aggregate(sumOfdispatchedQty, (current, dispatchDetail) => current + dispatchDetail.RequestedQuantityInMT);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                    /****** END: This Calculates the already dispatched amount of the FDP by former transporter if there is any **********/
                    if (t.AllocatedAmount - sumOfdispatchedQty > 0.1M)
                    {
                        var dispatchAllocation = new DispatchAllocation();
                        dispatchAllocation.DispatchAllocationID = Guid.NewGuid();

                        dispatchAllocation.Beneficiery = requisitionDetail != null ? requisitionDetail.BenficiaryNo : 0;
                        dispatchAllocation.Amount = t.AllocatedAmount - sumOfdispatchedQty;// transportOrderDetail.QuantityQtl;
                        dispatchAllocation.BidRefNo = transportOrder.BidDocumentNo;
                        dispatchAllocation.CommodityID = transportOrderDetail.CommodityID;
                        dispatchAllocation.ContractEndDate = transportOrder.StartDate;
                        dispatchAllocation.ContractEndDate = transportOrder.EndDate;
                        dispatchAllocation.DonorID = transportOrderDetail.DonorID;
                        dispatchAllocation.FDPID = transportOrderDetail.FdpID;
                        dispatchAllocation.HubID = t.HubID;
                        dispatchAllocation.TransporterID = transportOrder.TransporterID;
                        // dispatchAllocation.IsClosed = false;
                        dispatchAllocation.Month = requisition.Month;
                        dispatchAllocation.Round = requisition.Round;

                        dispatchAllocation.TransportOrderID = transportOrderId;
                        dispatchAllocation.ProgramID = requisition.ProgramID;
                        dispatchAllocation.RequisitionNo = requisition.RequisitionNo;
                        dispatchAllocation.RequisitionId = requisition.RequisitionID;
                        dispatchAllocation.PartitionId = 0;

                        //var si = sipc.Find(t => t.AllocationType == "SI");
                        //if (si != null)
                        //var pc = sipc.Find(t => t.AllocationType == "PC");
                        //if (pc != null)
                        if (t.AllocationType == "SI")
                            dispatchAllocation.ShippingInstructionID = t.Code;
                        else if (t.AllocationType == "PC")
                            dispatchAllocation.ProjectCodeID = t.Code;

                        BusinessProcessState createdstate = new BusinessProcessState
                        {
                            DatePerformed = DateTime.Now,
                            PerformedBy = "System Generated",
                            Comment = "Dispatch Allocation has been  Generated"

                        };
                        //_PaymentRequestservice.Create(request);

                        BusinessProcess businessPro = _businessProcessService.CreateBusinessProcess(appServId, 0,
                                                                                        "Dispatch", createdstate);

                        dispatchAllocation.BusinessProcessId = businessPro.BusinessProcessID;

                        //dispatchAllocation.Unit //i have no idea where to get it
                        // dispatchAllocation.StoreID  //Would be set null and filled by user later
                        //dispatchAllocation.Year= requisition.Year ; //Year is not available 
                        _unitOfWork.DispatchAllocationRepository.Add(dispatchAllocation);

                        GeneratedDispatchAllocations.Add(dispatchAllocation);
                    }
                }
            }
            var closedStateId =
                                _stateTemplateService
                                    .GetAll().FirstOrDefault(s => s.ParentProcessTemplateID == transportOrder.BusinessProcess.CurrentState.BaseStateTemplate.ParentProcessTemplateID && s.Name == "Closed");
            
            var bp = _businessProcessService.FindBy(t => t.BusinessProcessID == transportOrder.BusinessProcessID).FirstOrDefault();
            if (closedStateId != null)
            {
                var createdstate3 = new BusinessProcessState
                {
                    DatePerformed = DateTime.Now,
                    PerformedBy = UserName,
                    Comment = " TransportOrder Closed on Dispatch Plan Creation",
                    StateID = closedStateId.StateTemplateID,
                    ParentBusinessProcessID = transportOrder.BusinessProcessID
                };
                _businessProcessStateService.Add(createdstate3);

                if (bp != null)
                {
                    bp.CurrentState = createdstate3;
                    _businessProcessService.Update(bp);
                }
            }
            transportOrder.StatusID = (int)TransportOrderStatus.Closed;
            _unitOfWork.Save();

            return true;


        }

        public List<Dispatch> ReverseDispatchAllocation(int transportOrderId)
        {
            try
            {
                var dispatchAllocation =
                    _unitOfWork.DispatchAllocationRepository.FindBy(d => d.TransportOrderID == transportOrderId);
                if (dispatchAllocation != null)
                {

                    List<Dispatch> listDispatches = new List<Dispatch>();

                    foreach (var allocation in dispatchAllocation)
                    {
                        DispatchAllocation allocation1 = allocation;
                        var dispatch =
                        _unitOfWork.DispatchRepository.FindBy(
                            t => t.DispatchAllocationID == allocation1.DispatchAllocationID);
                        if (dispatch.Count > 0)
                        {
                            listDispatches.AddRange(dispatch);
                        }
                    }

                    if (listDispatches.Count > 0)
                        return listDispatches;
                    foreach (var allocation in dispatchAllocation)
                    {
                        _unitOfWork.DispatchAllocationRepository.Delete(allocation);
                    }



                    var transportOrder = _unitOfWork.TransportOrderRepository.FindById(transportOrderId);
                    if (transportOrder != null)
                    {
                        transportOrder.StatusID = (int)TransportOrderStatus.Draft;
                    }

                    _unitOfWork.Save();
                    return new List<Dispatch>();
                }
                return new List<Dispatch>();
            }
            catch (Exception)
            {

                return new List<Dispatch>();
            }
        }

        private void AddToNotification(int transportOrderId, string transportOrderNo, List<int> hubId)
        {
            try
            {
                if (HttpContext.Current == null) return;
                string destinationURl;
                if (HttpContext.Current.Request.Url.Host == "localhost")
                {
                    destinationURl = "http://" + HttpContext.Current.Request.Url.Authority +
                                     "/Hub/TransportOrder/NotificationIndex?recordId=" + transportOrderId;
                    return;
                }
                destinationURl = "http://" + HttpContext.Current.Request.Url.Authority +
                                 HttpContext.Current.Request.ApplicationPath +
                                 "/Hub/TransportOrder/NotificationIndex?recordId=" + transportOrderId;
                _notificationService.AddNotificationForHubManagersFromTransportOrder(destinationURl, transportOrderId,
                                                                                     transportOrderNo, hubId);
            }
            catch (Exception)
            {

                ;
            }

        }
        public bool RevertRequsition(int requisitionID)
        {

            var transportOrderDetails = _unitOfWork.TransportOrderDetailRepository.FindBy(m => m.RequisitionID == requisitionID).ToList();
            if (transportOrderDetails.Count != 0)
            {
                var transportOrderIDs = transportOrderDetails.Select(m => m.TransportOrderID).Distinct();

                var transportOrderDetailToDelete = new List<TransportOrderDetail>();
                foreach (var transportOrderDetail in transportOrderDetails)
                {
                    if (transportOrderDetail != null)
                    {
                        transportOrderDetailToDelete.Add(transportOrderDetail);
                    }
                }
                var transportRequsitionDetails = _unitOfWork.TransportRequisitionDetailRepository.FindBy(m => m.RequisitionID == requisitionID);
                if (transportRequsitionDetails.Count != 0)
                {
                    var transportRequsitionIDs = transportRequsitionDetails.Select(m => m.TransportRequisitionID).Distinct();
                    var transportRequisitionToDelete = new List<TransportRequisitionDetail>();
                    foreach (var transportRequisitionDetail in transportRequsitionDetails)
                    {
                        if (transportRequisitionDetail != null)
                        {
                            transportRequisitionToDelete.Add(transportRequisitionDetail);
                        }
                    }
                    var hubAllocations = _unitOfWork.HubAllocationRepository.FindBy(m => m.RequisitionID == requisitionID);
                    if (hubAllocations.Count != 0)
                    {
                        var hubAllocationtoDelete = new List<HubAllocation>();
                        foreach (var hubAllocation in hubAllocations)
                        {
                            if (hubAllocation != null)
                            {
                                hubAllocationtoDelete.Add(hubAllocation);
                            }

                        }
                        // delete SIPC Allocation table
                        var requisitionDetails =
                            _unitOfWork.ReliefRequisitionDetailRepository.FindBy(
                                m => m.RequisitionID == requisitionID).Select(
                                    m => m.RequisitionDetailID);
                        var sIPcAllocations =
                            _unitOfWork.SIPCAllocationRepository.FindBy(
                                m => requisitionDetails.Contains(m.RequisitionDetailID));
                        if (sIPcAllocations.Count != 0)
                        {

                            foreach (var sipcAllocation in sIPcAllocations)
                            {
                                if (sipcAllocation != null)
                                {
                                    var transactionGroup = _unitOfWork.TransactionGroupRepository.FindBy(m => m.TransactionGroupID == sipcAllocation.TransactionGroupID).FirstOrDefault();
                                    if (transactionGroup != null)
                                    {
                                        var transactions = _unitOfWork.TransactionRepository.FindBy(m => m.TransactionGroupID == transactionGroup.TransactionGroupID);
                                        if (transactions.Count != 0)
                                        {
                                            foreach (var transaction in transactions)
                                            {
                                                if (transaction != null)
                                                {
                                                    _unitOfWork.TransactionRepository.Delete(transaction);
                                                    _unitOfWork.Save();

                                                }

                                            }
                                        }

                                        _unitOfWork.TransactionGroupRepository.Delete(transactionGroup);
                                        _unitOfWork.Save();

                                    }

                                }

                            }

                        }
                        DeleteHubAllocations(hubAllocationtoDelete);
                    }

                    DeleteTransporRequsitionDetails(transportRequisitionToDelete);
                    foreach (var transportRequsition in _unitOfWork.TransportRequisitionRepository.FindBy(m => transportRequsitionIDs.Contains(m.TransportRequisitionID)))
                    {
                        if (transportRequsition.TransportRequisitionDetails.Count == 0)
                        {
                            _unitOfWork.TransportRequisitionRepository.Delete(transportRequsition);
                        }

                    }
                }

                DeleteTransportOrderDetails(transportOrderDetailToDelete);
                foreach (var transportOrder in _unitOfWork.TransportOrderRepository.FindBy(m => transportOrderIDs.Contains(m.TransportOrderID)))
                {
                    if (transportOrder.TransportOrderDetails.Count == 0)
                    {
                        _unitOfWork.TransportOrderRepository.Delete(transportOrder);
                    }

                }

                _unitOfWork.Save();
                UpdateRequsitionStatus(requisitionID);
                return true;
            }


            return false;
        }
        public bool ReverseTransportOrder(int transportOrderID)
        {
            var transportOrder = _unitOfWork.TransportOrderRepository.FindById(transportOrderID);
            if (transportOrder != null)
            {
                var requisitions = (from detail in transportOrder.TransportOrderDetails
                                    select detail.RequisitionID).Distinct();

                //check if other Transport Orders are created from this requisition
                var transportOrderDetails =
                    _unitOfWork.TransportOrderDetailRepository.FindBy(m => requisitions.Contains(m.RequisitionID) && m.TransportOrderID != transportOrderID);
                if (transportOrderDetails.Count == 0)
                {
                    //Delete TransportOrder and its Detail

                    var transportOrderDetailToDelete = new List<TransportOrderDetail>();
                    foreach (var transportOrderDetail in transportOrder.TransportOrderDetails)
                    {
                        if (transportOrderDetail != null)
                        {
                            transportOrderDetailToDelete.Add(transportOrderDetail);
                        }
                    }
                    var transportRequsitionDetails = _unitOfWork.TransportRequisitionDetailRepository.FindBy(m => requisitions.Contains(m.RequisitionID));
                    if (transportRequsitionDetails.Count != 0)
                    {
                        var transportRequisitionToDelete = new List<TransportRequisitionDetail>();
                        foreach (var transportRequisitionDetail in transportRequsitionDetails)
                        {
                            if (transportRequisitionDetail != null)
                            {
                                transportRequisitionToDelete.Add(transportRequisitionDetail);
                            }
                        }
                        var hubAllocations = _unitOfWork.HubAllocationRepository.FindBy(m => requisitions.Contains(m.RequisitionID));
                        if (hubAllocations.Count != 0)
                        {
                            var hubAllocationtoDelete = new List<HubAllocation>();
                            foreach (var hubAllocation in hubAllocations)
                            {
                                if (hubAllocation != null)
                                {
                                    hubAllocationtoDelete.Add(hubAllocation);
                                }

                            }
                            // delete SIPC Allocation table
                            var requisitionDetails =
                                _unitOfWork.ReliefRequisitionDetailRepository.FindBy(
                                    m => requisitions.Contains(m.RequisitionID)).Select(
                                        m => m.RequisitionDetailID);
                            var sIPcAllocations =
                                _unitOfWork.SIPCAllocationRepository.FindBy(
                                    m => requisitionDetails.Contains(m.RequisitionDetailID));
                            if (sIPcAllocations.Count != 0)
                            {
                                var sIPcAllocationToDelete = new List<SIPCAllocation>();
                                foreach (var sipcAllocation in sIPcAllocations)
                                {
                                    if (sipcAllocation != null)
                                    {
                                        var transactionGroup = _unitOfWork.TransactionGroupRepository.FindBy(m => m.TransactionGroupID == sipcAllocation.TransactionGroupID).FirstOrDefault();
                                        if (transactionGroup != null)
                                        {
                                            var transactions = _unitOfWork.TransactionRepository.FindBy(m => m.TransactionGroupID == transactionGroup.TransactionGroupID);
                                            if (transactions.Count != 0)
                                            {
                                                foreach (var transaction in transactions)
                                                {
                                                    if (transaction != null)
                                                    {
                                                        _unitOfWork.TransactionRepository.Delete(transaction);
                                                        _unitOfWork.Save();

                                                    }

                                                }
                                            }

                                            _unitOfWork.TransactionGroupRepository.Delete(transactionGroup);
                                            _unitOfWork.Save();

                                        }
                                        sIPcAllocationToDelete.Add(sipcAllocation);

                                    }

                                }
                                DeleteSiPcAllocations(sIPcAllocationToDelete);

                            }
                            DeleteHubAllocations(hubAllocationtoDelete);
                        }

                        DeleteTransporRequsitionDetails(transportRequisitionToDelete);
                    }

                    DeleteTransportOrderDetails(transportOrderDetailToDelete);
                    _unitOfWork.TransportOrderRepository.Delete(transportOrder);
                    _unitOfWork.Save();
                    UpdateRequsitionStatus(requisitions);
                    return true;
                }

                return false;
            }
            return false;
        }

        public void UpdateTransporterOrder(int transportorderId, int woredaId)
        {
            var transportOrder = _unitOfWork.TransportOrderRepository.FindById(transportorderId);
            foreach (var transportOrderDetail in transportOrder.TransportOrderDetails.Where(transportOrderDetail => transportOrderDetail.FDP.AdminUnitID == woredaId))
            {
                transportOrderDetail.IsChanged = true;
                _unitOfWork.TransportOrderDetailRepository.Edit(transportOrderDetail);

            }
            _unitOfWork.Save();
        }

        public decimal? CheckIfCommodityIsDipatchedToThisFdp(int fdpId, string bidNo, int transporterId, int transportOrderId, int commodityId, int requisitionID)
        {
            var dispatchAllocations = _unitOfWork.DispatchAllocationRepository.FindBy(d => d.TransportOrderID == transportOrderId && d.TransporterID == transporterId &&
                                    d.BidRefNo == bidNo && d.FDPID == fdpId && d.CommodityID == commodityId && d.RequisitionId == requisitionID).ToList();
            decimal sumOfdispatchedQty = 0.00M;
            decimal sumOfAllocatedQty = 0.00M;
            if (dispatchAllocations.Count > 0)
            {
                foreach (var dispatchAllocation in dispatchAllocations)
                {
                    var orDefault = dispatchAllocation.Dispatches;
                    foreach (var dispatch in orDefault)
                    {
                        var result =
                            dispatch?.DispatchDetails;

                        if (result?.Count > 0)
                        {
                            sumOfdispatchedQty = result.Aggregate(sumOfdispatchedQty, (current, dispatchDetail) => current + dispatchDetail.RequestedQuantityInMT);
                        }
                    }
                    sumOfAllocatedQty += dispatchAllocation.Amount;
                    dispatchAllocation.IsClosed = true;
                    _unitOfWork.DispatchAllocationRepository.Edit(dispatchAllocation);
                }
                return sumOfAllocatedQty - sumOfdispatchedQty;
            }
            return null;

        }
        public List<ReliefRequisition> GetRequsitionsToBeReverted()
        {

            var requsitions = new List<ReliefRequisition>();
            var dispatchAllocations = _unitOfWork.DispatchAllocationRepository.GetAll().Select(m => m.RequisitionId).Distinct().ToList();
            var allRequsitions = _unitOfWork.ReliefRequisitionRepository.Get(m => m.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Transport Order Created", null,
                "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").ToList();
            if (allRequsitions.Count != 0)
            {
                //var disReq =
                //    _unitOfWork.ReliefRequisitionRepository.FindBy(
                //        m => dispatchAllocations.Contains(m.RequisitionID)).ToList();
                requsitions = allRequsitions.Where(x => !dispatchAllocations.Contains(x.RequisitionID)).ToList();

            }

            return requsitions;
        }
        public void DeleteSiPcAllocations(List<SIPCAllocation> sIPcAllocations)
        {
            foreach (var sIPcAllocation in sIPcAllocations)
            {
                if (sIPcAllocation != null)
                {
                    _unitOfWork.SIPCAllocationRepository.Delete(sIPcAllocation);
                    _unitOfWork.Save();
                }
            }
        }

        public void DeleteHubAllocations(List<HubAllocation> hubAllocations)
        {
            foreach (var hubAllocation in hubAllocations)
            {
                if (hubAllocation != null)
                {
                    _unitOfWork.HubAllocationRepository.Delete(hubAllocation);
                    _unitOfWork.Save();
                }
            }
        }
        public void DeleteTransporRequsitionDetails(List<TransportRequisitionDetail> transportRequisitionDetails)
        {
            foreach (var transportRequisitionDetail in transportRequisitionDetails)
            {
                if (transportRequisitionDetail != null)
                {
                    _unitOfWork.TransportRequisitionDetailRepository.Delete(transportRequisitionDetail);
                    _unitOfWork.Save();
                }
            }
        }
        public void DeleteTransportOrderDetails(List<TransportOrderDetail> transportOrderDetails)
        {
            foreach (var transportOrdrDetail in transportOrderDetails)
            {
                if (transportOrdrDetail != null)
                {
                    _unitOfWork.TransportOrderDetailRepository.Delete(transportOrdrDetail);
                    _unitOfWork.Save();
                }
            }
        }
        public void UpdateRequsitionStatus(IEnumerable<int> requisitionIDs)
        {
            var requsitions = _unitOfWork.ReliefRequisitionRepository.Get(m => requisitionIDs.Contains(m.RequisitionID), null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").ToList();
            if (requsitions.Count != 0)
            {
                foreach (var requisition in requsitions)
                {
                    if (requisition != null)
                    {
                        var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Create Transport Requisition");
                        if (approveFlowTemplate != null)
                        {
                            var businessProcessState = new BusinessProcessState()
                            {
                                StateID = approveFlowTemplate.FinalStateID,
                                PerformedBy = "Performed by the system",
                                DatePerformed = DateTime.Now,
                                Comment = "Requisition has been reverted from 'transport order created' state to 'Approved' state.",
                                //AttachmentFile = fileName,
                                ParentBusinessProcessID = requisition.BusinessProcessID
                            };
                            //return 
                            _businessProcessService.PromotWorkflow(businessProcessState);
                        }
                        //requisition.Status = (int) ReliefRequisitionStatus.Approved;
                        //_unitOfWork.ReliefRequisitionRepository.Edit(requisition);
                        //_unitOfWork.Save();
                    }
                }
            }

        }
        public void UpdateRequsitionStatus(int requisitionID)
        {

            var requisition = _unitOfWork.ReliefRequisitionRepository.Get(t => t.RequisitionID == requisitionID, null,
                            "BusinessProcess, BusinessProcess.CurrentState, BusinessProcess.CurrentState.BaseStateTemplate").FirstOrDefault();
            if (requisition != null)
            {
                var approveFlowTemplate = requisition.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.FirstOrDefault(t => t.Name == "Create Transport Requisition");
                if (approveFlowTemplate != null)
                {
                    var businessProcessState = new BusinessProcessState()
                    {
                        StateID = approveFlowTemplate.FinalStateID,
                        PerformedBy = "Performed by the system",
                        DatePerformed = DateTime.Now,
                        Comment = "Requisition has been reverted from 'transport order created' state to 'Approved' state.",
                        //AttachmentFile = fileName,
                        ParentBusinessProcessID = requisition.BusinessProcessID
                    };
                    //return 
                    _businessProcessService.PromotWorkflow(businessProcessState);
                }
                // requisition.Status = (int)ReliefRequisitionStatus.Approved;
                //_unitOfWork.ReliefRequisitionRepository.Edit(requisition);
                //_unitOfWork.Save();

            }

        }

        public string GetTransportRequisitionNo(string transportRequisitionNo)
        {
            string input = transportRequisitionNo;
            const char searchChar = '/';
            const int occurrencePosition = 3; // third occurrence of the char
            var result = input.Select((c, i) => new { Char = c, Index = i })
                              .Where(item => item.Char == searchChar)
                              .Skip(occurrencePosition - 1)
                              .FirstOrDefault();

            if (result == null)
            {
                return string.Empty;
            }
            var str = input.Substring(result.Index + 1);
            return str;
        }

    }
}




