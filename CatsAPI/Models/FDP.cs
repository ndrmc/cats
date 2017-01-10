using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class FDP
    {
        public int FDPID { get; set; }

        public string Name { get; set; }
        public string NameAM { get; set; }
        public int AdminUnitID { get; set; }
        //public DbGeography FDPLocation { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<int> HubID { get; set; }

        public FDP(int _fdpId, string _name, string _nameAM, int _adminUnitId, string _latitude, string _longitde, int? _hubId)
        {
            FDPID = _fdpId;
            Name = _name;
            NameAM = _nameAM;
            AdminUnitID = _adminUnitId;
            Latitude = _latitude;
            Longitude = _longitde;
            HubID = _hubId;
        }
    }
}