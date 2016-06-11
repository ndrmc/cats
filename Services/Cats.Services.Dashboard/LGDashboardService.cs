﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Cats.Data.UnitWork;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;

namespace Cats.Services.Dashboard
{
    public class LgDashboardService:ILgDashboardService
    {

        private readonly IUnitOfWork _unitOfWork;
        public LgDashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<DashboardDispAlloRequisition> DispatchAllocatedRequisitions(int id = 0)
        {
            var currentHrd = _unitOfWork.HRDRepository.Get(m => m.Status == (int)HRDStatus.Published, null,
                    "HRDDetails, HRDDetails.AdminUnit, HRDDetails.AdminUnit.AdminUnit2, HRDDetails.AdminUnit.AdminUnit2.AdminUnit2," +
                    "Ration, Ration.RationDetails")
                                .OrderByDescending(t => t.PublishedDate).FirstOrDefault();
            
            if (currentHrd != null)
            {
                var dashboardDispAlloRequisitions = new List<DashboardDispAlloRequisition>();
                var regions =
                    _unitOfWork.AdminUnitRepository.Get(
                        t => t.AdminUnitTypeID == (int) Models.Constant.AdminUnitType.Region);
                if (id == 0)
                {
                    var firstOrDefault = GetRounds().FirstOrDefault();
                    if (firstOrDefault != null) id = firstOrDefault.Value;
                }
                var approvedReliefRequisitions = new List<ReliefRequisition>();
                var hubAlloReliefRequisitions = new List<ReliefRequisition>();
                var pcAlloReliefRequisitions = new List<ReliefRequisition>();
                var committedReliefRequisitions = new List<ReliefRequisition>();
                foreach (var region in regions)
                {
                    var dashboardDispAlloRequisition = new DashboardDispAlloRequisition();
                    var zones = (from hrdDetail in currentHrd.HRDDetails
                                where hrdDetail.AdminUnit.AdminUnit2.AdminUnit2.AdminUnitID == region.AdminUnitID
                                select hrdDetail.AdminUnit.AdminUnit2.AdminUnitID).Distinct().ToList();
                    var commodities = currentHrd.Ration.RationDetails.Distinct().ToList();

                    if (id != 0)
                    {
                        approvedReliefRequisitions =
                            _unitOfWork.ReliefRequisitionRepository.Get(
                                t => t.RegionalRequest.PlanID == currentHrd.PlanID
                                     && t.AdminUnit.AdminUnit2.AdminUnitID == region.AdminUnitID
                                     && t.RegionalRequest.Round == id
                                     && t.Status == (int) ReliefRequisitionStatus.Approved).ToList();

                        hubAlloReliefRequisitions =
                            _unitOfWork.ReliefRequisitionRepository.Get(
                                t => t.RegionalRequest.PlanID == currentHrd.PlanID
                                     && t.AdminUnit.AdminUnit2.AdminUnitID == region.AdminUnitID
                                     && t.RegionalRequest.Round == id
                                     && t.Status == (int) ReliefRequisitionStatus.HubAssigned).ToList();

                        pcAlloReliefRequisitions =
                            _unitOfWork.ReliefRequisitionRepository.Get(
                                t => t.RegionalRequest.PlanID == currentHrd.PlanID
                                     && t.AdminUnit.AdminUnit2.AdminUnitID == region.AdminUnitID
                                     && t.RegionalRequest.Round == id
                                     && t.Status == (int) ReliefRequisitionStatus.ProjectCodeAssigned).ToList();

                        committedReliefRequisitions =
                            _unitOfWork.ReliefRequisitionRepository.Get(
                                t => t.RegionalRequest.PlanID == currentHrd.PlanID
                                     && t.AdminUnit.AdminUnit2.AdminUnitID == region.AdminUnitID
                                     && t.RegionalRequest.Round == id
                                     && t.Status == (int) ReliefRequisitionStatus.SiPcAllocationApproved).ToList();
                    }
                    else
                    {
                        approvedReliefRequisitions =
                            _unitOfWork.ReliefRequisitionRepository.Get(
                                t => t.RegionalRequest.PlanID == currentHrd.PlanID
                                     && t.AdminUnit.AdminUnit2.AdminUnitID == region.AdminUnitID
                                     && t.Status == (int)ReliefRequisitionStatus.Approved).ToList();

                        hubAlloReliefRequisitions =
                            _unitOfWork.ReliefRequisitionRepository.Get(
                                t => t.RegionalRequest.PlanID == currentHrd.PlanID
                                     && t.AdminUnit.AdminUnit2.AdminUnitID == region.AdminUnitID
                                     && t.Status == (int)ReliefRequisitionStatus.HubAssigned).ToList();

                        pcAlloReliefRequisitions =
                            _unitOfWork.ReliefRequisitionRepository.Get(
                                t => t.RegionalRequest.PlanID == currentHrd.PlanID
                                     && t.AdminUnit.AdminUnit2.AdminUnitID == region.AdminUnitID
                                     && t.Status == (int)ReliefRequisitionStatus.ProjectCodeAssigned).ToList();

                        committedReliefRequisitions =
                            _unitOfWork.ReliefRequisitionRepository.Get(
                                t => t.RegionalRequest.PlanID == currentHrd.PlanID
                                     && t.AdminUnit.AdminUnit2.AdminUnitID == region.AdminUnitID
                                     && t.Status == (int)ReliefRequisitionStatus.SiPcAllocationApproved).ToList();
                    }
                    dashboardDispAlloRequisition.RegionId = region.AdminUnitID;
                    dashboardDispAlloRequisition.RegionName = region.Name;
                    dashboardDispAlloRequisition.NumberOfEstimatedRequisitions = zones.Count()*commodities.Count();
                    dashboardDispAlloRequisition.NumberOfApprovedRequisitions = approvedReliefRequisitions.Count();
                    dashboardDispAlloRequisition.NumberOfHubAllocatedRequisitions = hubAlloReliefRequisitions.Count();
                    dashboardDispAlloRequisition.NumberOfSipcAlloRequisitions = pcAlloReliefRequisitions.Count();
                    dashboardDispAlloRequisition.NumberOfCommitedRequisitions = committedReliefRequisitions.Count();
                    dashboardDispAlloRequisitions.Add(dashboardDispAlloRequisition);
                }
                return dashboardDispAlloRequisitions;
            }
            return null; 
        }

        public List<int?> GetRounds()
        {
            var roundList = new List<int?>();
            var currentHrd = _unitOfWork.HRDRepository.Get(m => m.Status == (int)HRDStatus.Published, null,
                    "HRDDetails, HRDDetails.AdminUnit, HRDDetails.AdminUnit.AdminUnit2, HRDDetails.AdminUnit.AdminUnit2.AdminUnit2," +
                    "Ration, Ration.RationDetails")
                                .OrderByDescending(t => t.PublishedDate).FirstOrDefault();

            if (currentHrd != null)
            {
                roundList = _unitOfWork.ReliefRequisitionRepository.Get(t => t.RegionalRequest.PlanID == currentHrd.PlanID
                            && t.Round.HasValue).Select(t=>t.Round).Distinct().ToList();
            }
            return roundList;
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        
    }
}
