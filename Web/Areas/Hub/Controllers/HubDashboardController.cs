﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Areas.Hub.Models;
using Cats.Services.Hub;
using Cats.Services.Hub.Interfaces;
using Cats.Models.Hubs;
using Cats.Helpers;

namespace Cats.Areas.Hub.Controllers
{
    public class HubDashboardController : Controller
    {
        //
        // GET: /Hub/HubDashboard/
        private readonly IStockStatusService _stockStatusService;
        private readonly IDispatchService _dispatchService;
        private readonly IDispatchAllocationService _dispatchAllocationService;

        public HubDashboardController(IStockStatusService stockStatusService,IDispatchService dispatchService,
                                      IDispatchAllocationService dispatchAllocationService)
        {
            _stockStatusService = stockStatusService;
            _dispatchService = dispatchService;
            _dispatchAllocationService = dispatchAllocationService;
        }

        public JsonResult StockStatus(int hub, int program)
        {
            if(hub!=0)
            {
                var st = _stockStatusService.GetStockSummaryHubDahsBoard(hub, DateTime.Now);

                //st.Take()
                if(st.Count > 0)
                {
                    var value = st.Find(t => t.HubID == hub);

                    var free = (value.TotalPhysicalStock == 0) ? 0 : ((value.TotalFreestock / (value.TotalPhysicalStock)) * 100);
                    var commited = ((value.TotalPhysicalStock - value.TotalFreestock) / ((value.TotalPhysicalStock == 0) ? 1.0M : value.TotalPhysicalStock )) * 100;

                    
                    var j = new StockStatusViewModel()
                    {
                        freeStockAmount = value.TotalFreestock,
                        freestockPercent = free,
                        physicalStockAmount = (value.TotalPhysicalStock - value.TotalFreestock),
                        physicalStockPercent = commited,
                        totalStock = value.TotalPhysicalStock
                    };

                    return Json(j, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new StockStatusViewModel(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult CommodityStockStatus(int hub, int program)
        {
            var st = _stockStatusService.GetFreeStockStatusD(hub, program, DateTime.Now);
            var q = (from s in st
                     select new HubFreeStockView
                     {
                         CommodityName = s.CommodityName,
                         FreeStock = s.FreeStock.ToPreferedWeightUnit(),
                         PhysicalStock = (((s.PhysicalStock - s.FreeStock) / ((s.PhysicalStock == 0) ? 1.0M : s.PhysicalStock )) * 100).ToPreferedWeightUnit()
                     });

            return Json(q, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RecentDispatches()
        {
            var currentUser = UserAccountHelper.GetUser(HttpContext.User.Identity.Name);
            if (currentUser.DefaultHub!=null)
            {
                var result = _dispatchService.FindBy(m => m.HubID == currentUser.DefaultHub).OrderByDescending(m=>m.DispatchID);
                var dispatched = GetDispatch(result);
                return Json(dispatched, JsonRequestBehavior.AllowGet);
            }
            
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        public JsonResult RecentDispatchAllocation()
        {
            var currentUser = UserAccountHelper.GetUser(HttpContext.User.Identity.Name);
            if (currentUser.DefaultHub != null)
            {
                var result = _dispatchAllocationService.FindBy(m => m.HubID == currentUser.DefaultHub).OrderByDescending(m => m.DispatchAllocationID);
                var dispached = GetDispatchAllocation(result);
                return Json(dispached, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<HubRecentDispachesViewModel> GetDispatch(IEnumerable<Dispatch> dispatchs)
        {
            return (from dispatch in dispatchs
                    select new HubRecentDispachesViewModel()
                        {
                            BidNumber = dispatch.BidNumber,
                            GIN=dispatch.GIN,
                            FDPName = dispatch.FDP.Name,
                            RequisitionNo = dispatch.RequisitionNo,
                            Commodity = dispatch.DispatchDetails.Single().Commodity.Name,
                            DispatchedAmount = dispatch.DispatchDetails.Sum(m=>m.DispatchedQuantityInMT),
                            Transporter = dispatch.Transporter.Name


                        }).Take(5);
        }
        private IEnumerable<HubRecentDispachesViewModel> GetDispatchAllocation(IEnumerable<DispatchAllocation> dispatchAllocations)
        {
            return (from dispatchAllocation in dispatchAllocations
                    select new HubRecentDispachesViewModel()
                        {
                            BidNumber = dispatchAllocation.BidRefNo,
                            FDPName = dispatchAllocation.FDP.Name,
                            RequisitionNo = dispatchAllocation.RequisitionNo,
                            //BeneficiaryNumber = dispatchAllocation.Beneficiery,
                            Program = dispatchAllocation.Program.Name,
                            Commodity = dispatchAllocation.Commodity.Name,
                            DispatchedAmount = dispatchAllocation.Amount,
                            Transporter = dispatchAllocation.Transporter.Name
                        }).Take(5);
        }
    }
}