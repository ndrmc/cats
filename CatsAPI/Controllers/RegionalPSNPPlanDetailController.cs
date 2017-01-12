using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.PSNP;
using Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    public class RegionalPSNPPlanDetailController : ApiController
    {
        private readonly IRegionalPSNPPlanDetailService _regionalPsnpPlanDetailService;

        public RegionalPSNPPlanDetailController(IRegionalPSNPPlanDetailService regionalPsnpPlanDetailService)
        {
            _regionalPsnpPlanDetailService = regionalPsnpPlanDetailService;
        }
        public IEnumerable<RegionalPSNPPlanDetail> GetRegionalPsnpPlanDetails()
        {
            return (from d in _regionalPsnpPlanDetailService.GetAllRegionalPSNPPlanDetail()
                select new RegionalPSNPPlanDetail()
                {
                    Contingency = d.Contingency,
                    WoredaName = d.PlanedWoreda.Name,
                    BeneficiaryCount = d.BeneficiaryCount,
                    CashRatio = d.CashRatio,
                    FoodRatio = d.FoodRatio,
                    Item3Ratio = d.Item3Ratio,
                    Item4Ratio = d.Item4Ratio,
                    PlanedWoredaID = d.PlanedWoredaID,
                    RegionalPSNPPlanDetailID = d.RegionalPSNPPlanDetailID,
                    RegionalPSNPPlanID = d.RegionalPSNPPlanID,
                    StartingMonth = d.StartingMonth
                }).ToList();
        }

        // GET api/<controller>/5
        public RegionalPSNPPlanDetail GetRegionalPsnpPlanDetail(int id)
        {
            var regionalPsnpPlanDetail = _regionalPsnpPlanDetailService.FindById(id);
            if (regionalPsnpPlanDetail == null) return null;
            return new RegionalPSNPPlanDetail()
            {
                Contingency = regionalPsnpPlanDetail.Contingency,
                WoredaName = regionalPsnpPlanDetail.PlanedWoreda.Name,
                BeneficiaryCount = regionalPsnpPlanDetail.BeneficiaryCount,
                CashRatio = regionalPsnpPlanDetail.CashRatio,
                FoodRatio = regionalPsnpPlanDetail.FoodRatio,
                Item3Ratio = regionalPsnpPlanDetail.Item3Ratio,
                Item4Ratio = regionalPsnpPlanDetail.Item4Ratio,
                PlanedWoredaID = regionalPsnpPlanDetail.PlanedWoredaID,
                RegionalPSNPPlanDetailID = regionalPsnpPlanDetail.RegionalPSNPPlanDetailID,
                RegionalPSNPPlanID = regionalPsnpPlanDetail.RegionalPSNPPlanID,
                StartingMonth = regionalPsnpPlanDetail.StartingMonth
            };
        }
    }
}