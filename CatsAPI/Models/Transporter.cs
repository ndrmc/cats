using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Transporter
    {
        public int TransporterID { get; set; }
        public string Name { get; set; }
        public int Region { get; set; }
        public string SubCity { get; set; }
        public int Zone { get; set; }
        public int Woreda { get; set; }
        public string Kebele { get; set; }
        public string HouseNo { get; set; }
        public string TelephoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Ownership { get; set; }
        public int VehicleCount { get; set; }
        public decimal LiftCapacityFrom { get; set; }
        public decimal LiftCapacityTo { get; set; }
        public decimal LiftCapacityTotal { get; set; }
        public decimal Capital { get; set; }

        public int EmployeeCountMale { get; set; }
        public int EmployeeCountFemale { get; set; }

        public string OwnerName { get; set; }
        public string OwnerMobile { get; set; }

        public string ManagerName { get; set; }
        public string ManagerMobile { get; set; }
        public int? PartitionId { get; set; }
        public bool OwnedByDRMFSS { get; set; }

        public Transporter(int transporterID, string name, int region, string subCity, int zone, 
            int woreda, string kebele, string houseNo, string telephoneNo, string mobileNo, 
            string email, string ownership, int vehicleCount, 
            decimal liftCapacityFrom, decimal liftCapacityTo, 
            decimal liftCapacityTotal, decimal capital, int employeeCountMale,
            int employeeCountFemale, string ownerName, string ownerMobile, 
            string managerName, string managerMobile, bool ownedByDrmfss)
        {
            TransporterID = transporterID;
            Name = name;
            Region = region;
            SubCity = subCity;
            Zone = zone;
            Woreda = woreda;
            Kebele = kebele;
            HouseNo = houseNo;
            TelephoneNo = telephoneNo;
            MobileNo = mobileNo;
            Email = email;
            Ownership = ownership;
            VehicleCount = vehicleCount;
            LiftCapacityFrom = liftCapacityFrom;
            LiftCapacityTo = liftCapacityTo;
            LiftCapacityTotal = liftCapacityTotal;
            Capital = capital;
            EmployeeCountMale = employeeCountMale;
            EmployeeCountFemale = employeeCountFemale;
            OwnerName = ownerName;
            OwnerMobile = ownerMobile;
            ManagerName = managerName;
            ManagerMobile = managerMobile;
            OwnedByDRMFSS = ownedByDrmfss;
        }
    }
}