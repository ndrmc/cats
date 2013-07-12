﻿using System;
using Cats.Models;
using Cats.Data.Repository;

namespace Cats.Data.UnitWork
{
    public interface IUnitOfWork : IDisposable
    {
        // TODO: Add properties to be implemented by UnitOfWork class for each repository
        IGenericRepository<RegionalRequest> RegionalRequestRepository { get; }
        IGenericRepository<RegionalRequestDetail> RegionalRequestDetailRepository { get; }
        IGenericRepository<AdminUnit> AdminUnitRepository { get; }
        IGenericRepository<AdminUnitType> AdminUnitTypeRepository { get; }
        IGenericRepository<Commodity> CommodityRepository { get; }
        IGenericRepository<CommodityType> CommodityTypeRepository { get; }
        IGenericRepository<FDP> FDPRepository { get; }
        IGenericRepository<Program> ProgramRepository { get; }
        IGenericRepository<Hub> HubRepository { get; }
        IGenericRepository<ReliefRequisitionDetail> ReliefRequisitionDetailRepository { get; }
        IGenericRepository<ReliefRequisition> ReliefRequisitionRepository { get; }
       
        IGenericRepository<HubAllocation> HubAllocationRepository { get;}
        IGenericRepository<BidWinner> BidWinnerRepository { get; } 
        //IGenericRepository<HubAllocation> HubAllocationRepository { get; }
        IGenericRepository<ProjectCodeAllocation> ProjectCodeAllocationRepository { get; }

        IGenericRepository<Transporter> TransporterRepository { get; } 
        IGenericRepository<TransportBidPlan> TransportBidPlanRepository { get; }
        IGenericRepository<TransportBidPlanDetail> TransportBidPlanDetailRepository { get; } 
       
        IGenericRepository<Bid> BidRepository { get; }
        IGenericRepository<BidDetail> BidDetailRepository { get; } 
        IGenericRepository<Status> StatusRepository { get; }
        

        //IGenericRepository<DispatchAllocationDetail> DispatchAllocationRepository { get; }
        IGenericRepository<DispatchAllocation> DispatchAllocationRepository { get; }
        IGenericRepository<TransportOrder> TransportOrderRepository { get; }
        IGenericRepository<TransportBidWinnerDetail> TransportBidWinnerDetailRepository { get; }
        IGenericRepository<vwTransportOrder> VwTransportOrderRepository { get;  } 
        void Save();

    }
}