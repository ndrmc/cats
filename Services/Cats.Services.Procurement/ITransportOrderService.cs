﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Cats.Models;
using Cats.Models.ViewModels;

namespace Cats.Services.Procurement
{
    public interface ITransportOrderService : IDisposable
    {

        bool AddTransportOrder(TransportOrder transportOrder);
        bool DeleteTransportOrder(TransportOrder transportOrder);
        bool DeleteById(int id);
        bool EditTransportOrder(TransportOrder transportOrder);
        TransportOrder FindById(int id);
        List<TransportOrder> GetAllTransportOrder();
        List<TransportOrder> FindBy(Expression<Func<TransportOrder, bool>> predicate);
        IEnumerable<TransportOrderDetail> GetTransportOrderDetail(int requisitionId);
        IEnumerable<ReliefRequisition> GetTransportOrderReleifRequisition(int status);
        IEnumerable<TransportOrderDetail> GetTransportOrderDetailByTransportId(int transportId);
        IEnumerable<TransportOrder> Get(
                   Expression<Func<TransportOrder, bool>> filter = null,
                   Func<IQueryable<TransportOrder>, IOrderedQueryable<TransportOrder>> orderBy = null,
                   string includeProperties = "");
        IEnumerable<TransportOrder> GetByHub(
                   Expression<Func<TransportOrder, bool>> filter = null,
                   Func<IQueryable<TransportOrder>, IOrderedQueryable<TransportOrder>> orderBy = null,
                   string includeProperties = "", int hubId = 0, int statusId = 0);

        //IEnumerable<RequisitionToDispatch> GetRequisitionToDispatch();
        //IEnumerable<ReliefRequisition> GetProjectCodeAssignedRequisitions();
        bool CreateTransportOrder(int requisitionId, int bidId, string requesterName);
        int ReAssignTransporter(IEnumerable<TransportRequisitionWithoutWinnerModel> transReqWithTransporter, int transporterID);
        bool ApproveTransportOrder(TransportOrder transportOrder);
        bool SignTransportOrder(TransportOrder transportOrder);
        List<vwTransportOrder> GeTransportOrderRpt(int id);
        List<Transporter> GetTransporter();
        List<Hub> GetHubs();
        bool GeneratDispatchPlan(int transportOrderId);
        IOrderedEnumerable<RequisiionNoViewModel> GetZone(int transReqNo);
        IOrderedEnumerable<WoredaViewModelInTransReqWithoutWinner> GetWoredas(int zoneId, int transReqNo);
        IOrderedEnumerable<RegionsViewModel> GetRegions();
        IEnumerable<TransportOrder> GetFilteredTransportOrder(
            IEnumerable<TransportRequisitionDetail> transportRequsitionDetails, string stateName);

        IEnumerable<TransportOrder> GetFilteredTransportOrder(IEnumerable<TransportOrderDetail> transportOrderDetails,
                                                              int statusId);
        List<Program> GetPrograms();
        bool ReverseTransportOrder(int transportOrderID);
        bool RevertRequsition(int requisitionID);
        List<ReliefRequisition> GetRequsitionsToBeReverted();
        List<Dispatch> ReverseDispatchAllocation(int transportOrderId);
        void DeleteTransportOrderDetails(List<TransportOrderDetail> transportOrderDetails);
        string GetTransportRequisitionNo(string transportOrderNo);
        void UpdateTransporterOrder(int transportorderId, int woredaId);

        decimal? CheckIfCommodityIsDipatchedToThisFdp(int fdpId, string bidNo, int transporterId, int transporrtOrderId,
                                                     int commodityId, int requisitionID);
    }
}


