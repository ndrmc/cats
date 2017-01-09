using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.Hub;
using Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    public class CommodityGradeController : ApiController
    {
        private readonly ICommodityGradeService _commodityGradeService;
        public CommodityGradeController(ICommodityGradeService commodityService)
        {
            _commodityGradeService = commodityService;
        }

        [HttpGet]
        public List<CommodityGrade> Get()
        {
            var data = _commodityGradeService.GetAllCommodityGrade();
            return (from cg in data
                    select new CommodityGrade()
                    {
                        CommodityGradeID = cg.CommodityGradeID,
                        Name = cg.Name,
                        Description = cg.Description
                    }).ToList();
        }
    }
}