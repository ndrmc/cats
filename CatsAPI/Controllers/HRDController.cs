using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.EarlyWarning;
namespace Cats.Rest.Controllers
{
    public class HRDController : ApiController
    {
        private readonly IHRDService _hrdService;
        public HRDController(IHRDService hrdService)
        {
            _hrdService = hrdService;
        }
        // GET api/<controller>
        public IEnumerable<Models.HRD> GetHRDs()
        {
            var hrds = _hrdService.GetAllHRD();
            var hrdList = new List<Models.HRD>();
            Models.HRD hrdModel;
            foreach(var hrd in hrds)
            {
                hrdModel = new Models.HRD()
                {
                    CreatedBY = hrd.CreatedBY,
                    CreatedDate = hrd.CreatedDate,
                    HRDDetails = (from d in hrd.HRDDetails
                                  select new Models.HRDDetail()
                                  {
                                      WoredaName = d.AdminUnit.Name,
                                      DurationOfAssistance = d.DurationOfAssistance,
                                      HRDDetailID = d.HRDDetailID,
                                      NumberOfBeneficiaries = d.NumberOfBeneficiaries,
                                      StartingMonth = d.StartingMonth,
                                      WoredaID = d.WoredaID
                                  }).ToList(),
                    HRDID = hrd.HRDID,
                    PartitionId = hrd.PartitionId,
                    PlanID = hrd.PlanID,
                    PublishedDate = hrd.PublishedDate,
                    Ration = new Models.Ration()
                    {
                        CreatedBy = hrd.Ration.CreatedBy,
                        CreatedDate = hrd.Ration.CreatedDate,
                        IsDefaultRation = hrd.Ration.IsDefaultRation,
                        RationID = hrd.RationID,
                        RefrenceNumber = hrd.Ration.RefrenceNumber,
                        UpdatedBy = hrd.Ration.UpdatedBy,
                        UpdatedDate = hrd.Ration.UpdatedDate
                    }
                };
                hrdList.Add(hrdModel);        
            }
            return hrdList;
        }

        // GET api/<controller>/5
        public Models.HRD GetHRD(int id)
        {
            var hrd = _hrdService.GetAllHRD().Where(h => h.HRDID == id).FirstOrDefault();
          
            Models.HRD hrdModel;

            hrdModel = new Models.HRD()
            {
                CreatedBY = hrd.CreatedBY,
                CreatedDate = hrd.CreatedDate,
                HRDDetails = (from d in hrd.HRDDetails
                              select new Models.HRDDetail()
                              {
                                  WoredaName = d.AdminUnit.Name,
                                  DurationOfAssistance = d.DurationOfAssistance,
                                  HRDDetailID = d.HRDDetailID,
                                  NumberOfBeneficiaries = d.NumberOfBeneficiaries,
                                  StartingMonth = d.StartingMonth,
                                  WoredaID = d.WoredaID
                              }).ToList(),
                HRDID = hrd.HRDID,
                PartitionId = hrd.PartitionId,
                PlanID = hrd.PlanID,
                PublishedDate = hrd.PublishedDate,
                Ration = new Models.Ration()
                {
                    CreatedBy = hrd.Ration.CreatedBy,
                    CreatedDate = hrd.Ration.CreatedDate,
                    IsDefaultRation = hrd.Ration.IsDefaultRation,
                    RationID = hrd.RationID,
                    RefrenceNumber = hrd.Ration.RefrenceNumber,
                    UpdatedBy = hrd.Ration.UpdatedBy,
                    UpdatedDate = hrd.Ration.UpdatedDate
                }
            };

            return hrdModel;
        }

    }
}