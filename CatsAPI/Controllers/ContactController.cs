using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Services.Administration;
namespace Cats.Rest.Controllers
{
    public class ContactController : ApiController
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        // GET api/<controller>
        public IEnumerable<Models.Contact> GetAllContacts()
        {
            return (from c in _contactService.GetAllContact()
                    select new Models.Contact()
                    {
                        ContactID = c.ContactID,
                        FDPID = c.FDPID,
                        FdpName = c.FDP.Name,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        PhoneNo = c.PhoneNo
                    }).ToList();
        }

        // GetContact api/<controller>/5
        [HttpGet]
        public Models.Contact GetContact(int id)
        {
            var c = _contactService.GetAllContact().Where(cn => cn.ContactID == id).FirstOrDefault();
            if (c != null)
            {
                return new Models.Contact()
                {
                    ContactID = c.ContactID,
                    FDPID = c.FDPID,
                    FdpName = c.FDP.Name,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    PhoneNo = c.PhoneNo
                };
            }
            return null;
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}