﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Cats.Rest.Models;
using Cats.Services.EarlyWarning;
using Microsoft.Ajax.Utilities;

namespace Cats.Rest.Controllers
{
    public class RationController : ApiController
    {

        private readonly IRationService _rationService;

        public RationController(IRationService rationService)
        {
            _rationService = rationService;
        }
        /// <summary>
        /// Returns all list of Rations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        
        public IEnumerable<Ration> GetRations()
        {
            return (from r in _rationService.GetAllRation()
                select new Ration()
                {
                    CreatedBy = r.CreatedBy,
                    CreatedDate = r.CreatedDate,
                    IsDefaultRation = r.IsDefaultRation,
                    RationID = r.RationID,
                    RefrenceNumber = r.RefrenceNumber,
                    UpdatedBy = r.UpdatedBy,
                    UpdatedDate = r.UpdatedDate
                }).ToList();
        }

        // GET api/<controller>/5
        /// <summary>
        /// Return a single object of Ration given a ration id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Ration GetRation(int id)
        {
            var ration = _rationService.FindById(id);
            if (ration == null) return null;
            return new Ration()
            {
                CreatedBy = ration.CreatedBy,
                CreatedDate = ration.CreatedDate,
                IsDefaultRation = ration.IsDefaultRation,
                RationID = ration.RationID,
                RefrenceNumber = ration.RefrenceNumber,
                UpdatedBy = ration.UpdatedBy,
                UpdatedDate = ration.UpdatedDate
            };
        }

    }
}