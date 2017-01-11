using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.EarlyWarning;
using Cats.Rest.Models;;
namespace Cats.Rest.Controllers
{
    public class SeasonController : ApiController
    {
        private readonly ISeasonService _seasonService;

        public SeasonController(ISeasonService seasonService)
        {
            _seasonService = seasonService;
        }
       [HttpGet]
        public IEnumerable<Season> GetSeasons()
       {
           return (from s in _seasonService.GetAllSeason()
               select new Season
               {
                   SeasonId = s.SeasonID,
                   Name = s.Name
               });
       }
       [HttpGet]
        public Season GetSeason(int id)
       {
           var season = _seasonService.FindById(id);
           if (season == null) return null;
           return new Season
           {
               SeasonId = season.SeasonID,
               Name = season.Name
           };
       }

       
    }
}