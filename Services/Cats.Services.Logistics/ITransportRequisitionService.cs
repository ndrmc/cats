﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cats.Models;
using Cats.Models.ViewModels;

namespace Cats.Services.Logistics
{
    public interface ITransportRequisitionService : IDisposable
    {
        bool AddTransportRequisition(TransportRequisition transportRequisition);
        bool DeleteTransportRequisition(TransportRequisition transportRequisition);
        bool DeleteById(int id);
        bool EditTransportRequisition(TransportRequisition transportRequisition);
        TransportRequisition FindById(int id);
        List<TransportRequisition> GetAllTransportRequisition();
        List<TransportRequisition> FindBy(Expression<Func<TransportRequisition, bool>> predicate);
        IEnumerable<TransportRequisition> Get(
                   Expression<Func<TransportRequisition, bool>> filter = null,
                   Func<IQueryable<TransportRequisition>, IOrderedQueryable<TransportRequisition>> orderBy = null,
                   string includeProperties = "");
        bool CreateTransportRequisition(List<List<int>> reliefRequisitions,int requestedBy, string requesterName);
        bool CheckIfBidIsCreatedForAnOrder(int transportRequisitionId);
        IEnumerable<RequisitionToDispatch> GetRequisitionToDispatch();
        bool ApproveTransportRequisition(int id,int approvedBy);
        List<RequisitionToDispatch> GetTransportRequisitionDetail(List<int> requIds);
        List<TransportRequisitionDetail> GetTransportRequsitionDetails(int programId);
        List<TransportRequisitionDetail> GetTransportRequsitionDetails();
        List<BidNumber> ReturnBids(int transportRequisitionId);
        //string GetStoreName(int hubId, int requisitionId);
    }
}