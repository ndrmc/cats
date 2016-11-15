﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Services.EarlyWarning;
using Cats.Areas.PSNP.Models;
using Cats.Services.Logistics;
using Cats.Services.PSNP;

namespace Cats.Areas.PSNP.Controllers
{
    public class DashboardController : Controller
    {
        //
        // GET: /PSNP/Dashboard/
        private readonly IRegionalRequestService _regionalRequestService;
        private readonly IRegionalRequestDetailService _regionalRequestDetailService;
        private readonly IReliefRequisitionService _reliefRequisitionService;
        private readonly IHRDService _hrdService;
        private readonly IRegionalPSNPPlanService _regionalPsnpPlanService;
        private readonly IUtilizationHeaderSerivce _utilizationService;
        private readonly IUtilizationDetailSerivce _utilizationDetailSerivce;
        

        public DashboardController(IRegionalRequestService regionalRequestService,
            IRegionalRequestDetailService reliefRequisitionDetailService,
            IReliefRequisitionService reliefRequisitionService,
            IHRDService hrdService, IRegionalPSNPPlanService regionalPsnpPlanService,
            IUtilizationHeaderSerivce utilization, IUtilizationDetailSerivce utilizationDetail
            )
        {
            _regionalRequestService = regionalRequestService;
            _regionalRequestDetailService = reliefRequisitionDetailService;
            _reliefRequisitionService = reliefRequisitionService;
            _hrdService = hrdService;
            _regionalPsnpPlanService = regionalPsnpPlanService;
            _utilizationService = utilization;
            _utilizationDetailSerivce = utilizationDetail;
        }
        public JsonResult GetPsnpRequests()
        {
            var requests = _regionalRequestService.GetAllRegionalRequest().OrderByDescending(t=>t.RequistionDate);
            var r = new List<PSNPRequetViewModel>();
            foreach (var regionalRequest in requests)
            {
                var f = new PSNPRequetViewModel();
                f.Number = regionalRequest.ReferenceNumber;
                f.fdps = regionalRequest.RegionalRequestDetails.Count;
                f.beneficiaries = regionalRequest.RegionalRequestDetails.Sum(t => t.Beneficiaries);
                f.status = regionalRequest.Status;
                f.RequestId = regionalRequest.RegionalRequestID;
                f.PlanId = regionalRequest.PlanID;
                f.PlanName = regionalRequest.Plan.PlanName;
                r.Add(f);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RequestPie(int planId=-1)
        {
           

            var requests = _regionalRequestService.FindBy(t => t.PlanID == planId);

            var req = (from request in requests
                     group request by request.AdminUnit.AdminUnitID into g 
                     let firstOrDefault = g.FirstOrDefault() 
                     where firstOrDefault != null select new
                     {
                         Region = g.First().AdminUnit.Name,
                         Count = g.Count(),
                         PlanId = g.First().PlanID,
                         firstOrDefault.Plan.PlanName
                     });
            return Json(req, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RequestPieByStatus(int planId = -1)
        {
            
             var requests = _regionalRequestService.FindBy(t => t.PlanID == planId);

            var r = (from request in requests
                     group request by request.BusinessProcess.CurrentState.BaseStateTemplate.Name into g
                      let firstOrDefault = g.FirstOrDefault() 
                     where firstOrDefault != null 
                     select new
                     {
                         g.First().Status,
                         Count = g.Count(),
                         PlanId = g.First().PlanID,
                         firstOrDefault.Plan.PlanName
                     });

            Dictionary<string, int> _request = new Dictionary<string, int>();
            

            foreach (var req in r )
            {
                if (req.Status == (decimal)Cats.Models.Constant.RegionalRequestStatus.Draft)
                    _request.Add("Draft", req.Count);
                else if (req.Status == (decimal)Cats.Models.Constant.RegionalRequestStatus.Approved)
                    _request.Add("Approved", req.Count);
                else if (req.Status == (decimal)Cats.Models.Constant.RegionalRequestStatus.Closed)
                    _request.Add("Closed", req.Count);
               
            }
            return Json(_request, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPlans()
        {
            var plans =
                _regionalPsnpPlanService.FindBy(p => p.Plan.Status == (int) Cats.Models.Constant.PlanStatus.PSNPCreated)
                    .Select(q => new
                                     {
                                         planId = q.PlanId,
                                         planName = q.Plan.PlanName
                                     }).Distinct();
            return Json(plans, JsonRequestBehavior.AllowGet);
        }

       
        
        public ActionResult GetPsnpRequisitions()
        {
            var requests = _reliefRequisitionService.GetAllReliefRequisition().OrderByDescending(t => t.RequestedDate);
            var r = new List<PSNPRequisitionViewModel>();
            foreach (var regionalRequsition in requests)
            {
                var psnpReqistions = new PSNPRequisitionViewModel
                            {
                                Number = regionalRequsition.RequisitionNo,
                                Commodity = regionalRequsition.Commodity.Name,
                                Beneficicaries = regionalRequsition.ReliefRequisitionDetails.Sum(t => t.BenficiaryNo),
                                Amount = regionalRequsition.ReliefRequisitionDetails.Sum(t => t.Amount),
                                Status = regionalRequsition.Status,
                                RequisitionId = regionalRequsition.RequisitionID
                            };
                if (regionalRequsition.RegionalRequest != null)
                {
                    psnpReqistions.PlanId = regionalRequsition.RegionalRequest.PlanID;
                    psnpReqistions.PlanName = regionalRequsition.RegionalRequest.Plan.PlanName;
                }
                r.Add(psnpReqistions);
            }
            return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUtilizationReport(int planId = -1)
        {

            var utilizations = _utilizationService.FindBy(t => t.PlanID == planId);
            var r = (from util in utilizations
                     group util by util.PlanID into g
                     let firstOrDefault = g.FirstOrDefault()
                     where firstOrDefault != null
                     select new
                     {
                         
                         Above18 = g.Sum(t=>t.MaleAbove18Years) + g.Sum(t=>t.FemaleAbove18Years),
                         Bet5and18 = g.Sum(t=>t.MaleBetween5And18Years) + g.Sum(t=>t.FemaleBetween5And18Years),
                         Below5 = g.Sum(t => t.FemaleLessThan5Years) + g.Sum(t => t.MaleLessThan5Years)
                         
                     });
                return Json(r, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUtilizationByRegion(int planId = -1)
        {

            var utilizations = _utilizationService.GetAllHeaderDistribution();
            var r = new List<UtilizationViewModel>();
            foreach (var utilization in utilizations)
            {
                foreach (var utilizationdetail in utilization.WoredaStockDistributionDetails)
                {
                    var regionalutilization  = new UtilizationViewModel()
                                             {
                                                 Region =  utilization.AdminUnit.AdminUnit2.AdminUnit2.Name,
                                                 //Commodity = utilizationdetail.CommodityID.ToString(),
                                                 //ActualBeneficicaries = utilization.ActualBeneficairies,
                                                 Amount = utilizationdetail.DistributedAmount,
                                                 
                                             };
                    r.Add(regionalutilization);
                }
               
                
            }
            
            var grouped = (from total in r
                           group total by total.Region
                           into g
                           select new
                                      {

                                          Region = g.First().Region,
                                          total = g.Sum(t => t.Amount)
                                      });
            return Json(grouped, JsonRequestBehavior.AllowGet );

            
        
            }

    }
}