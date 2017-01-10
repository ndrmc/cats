using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.EarlyWarning;
using Cats.Rest.Models;
namespace Cats.Rest.Controllers
{
    public class HRDDetailController : ApiController
    {
        private readonly IHRDDetailService _hrdDetailService;
        public HRDDetailController(IHRDDetailService hrdDetailService)
        {
            _hrdDetailService = hrdDetailService;
        }
        /// <summary>
        /// Get all hrd details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<HRDDetail> GetHrdDetails()
        {
            return (from h in _hrdDetailService.GetAllHRDDetail()
                    select new Models.HRDDetail()
                    {
                        AdminUnitId = h.AdminUnit.AdminUnitID,
                        Beneficiaries = h.NumberOfBeneficiaries,
                        Duration = h.DurationOfAssistance,
                        HRDDetailID = h.HRDDetailID,
                        StartingMonth = h.StartingMonth,
                        WoredaName = h.AdminUnit.Name
                    }).ToList();
        }

       /// <summary>
       /// Get Hrd detail by id
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        public HRDDetail GetHrdDetail(int id)
        {
          var h =  _hrdDetailService.FindById(id);
            return new Models.HRDDetail()
            {
                AdminUnitId = h.AdminUnit.AdminUnitID,
                Beneficiaries = h.NumberOfBeneficiaries,
                Duration = h.DurationOfAssistance,
                HRDDetailID = h.HRDDetailID,
                StartingMonth = h.StartingMonth,
                WoredaName = h.AdminUnit.Name
            };
        }
        
    }
}