using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.Procurement;

namespace Cats.Rest.Controllers
{
    public class TransporterController : ApiController
    {
        //
        // GET: /Transporter/

        private ITransporterService _transporterService;

        public TransporterController(ITransporterService transporterService)
        {
            _transporterService = transporterService;
        }
        /// <summary>
        /// Returns all Transporters
        /// </summary>
        /// <returns></returns>
        public List<Models.Transporter> Get()
      {
            var transporters = _transporterService.GetAllTransporter();
            return transporters.Select(transporter => new Models.Transporter(transporter.TransporterID, transporter.Name, transporter.Region, transporter.SubCity, transporter.Zone, transporter.Woreda, transporter.Kebele, transporter.HouseNo, transporter.TelephoneNo, transporter.MobileNo, transporter.Email, transporter.Ownership, transporter.VehicleCount, transporter.LiftCapacityFrom, transporter.LiftCapacityTo, transporter.LiftCapacityTotal, transporter.Capital, transporter.EmployeeCountMale, transporter.EmployeeCountMale, transporter.OwnerName, transporter.OwnerMobile, transporter.ManagerName, transporter.ManagerMobile, transporter.OwnedByDRMFSS)).ToList();
      }
        /// <summary>
        /// Returns a transporter object given a transporter Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Transporter Get(int id)
        {
            var transporter = _transporterService.FindById(id);
            if(transporter!=null)
            {
                 var newTransporter = new Models.Transporter(transporter.TransporterID, transporter.Name,
                                                            transporter.Region, transporter.SubCity,
                                                            transporter.Zone, transporter.Woreda, transporter.Kebele,
                                                            transporter.HouseNo, transporter.TelephoneNo,
                                                            transporter.MobileNo,
                                                            transporter.Email, transporter.Ownership,
                                                            transporter.VehicleCount, transporter.LiftCapacityFrom,
                                                            transporter.LiftCapacityTo,
                                                            transporter.LiftCapacityTotal, transporter.Capital,
                                                            transporter.EmployeeCountMale, transporter.EmployeeCountMale,
                                                            transporter.OwnerName,
                                                            transporter.OwnerMobile, transporter.ManagerName,
                                                             transporter.ManagerMobile, transporter.OwnedByDRMFSS);

                return newTransporter;

            }
            return null;
        }

    }
}
