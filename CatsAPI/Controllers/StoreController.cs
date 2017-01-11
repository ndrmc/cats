using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.Hub;

namespace Cats.Rest.Controllers
{
    public class StoreController : ApiController
    {
        //
        // GET: /Store/

        private readonly IStoreService _storeService;

        public StoreController(IStoreService storeService)
        {
            _storeService = storeService;
        }


        /// <summary>
        /// Gets all Stores
        /// </summary>
        /// <returns></returns>
        public List<Models.Store> Get()
        {
            var stores = _storeService.GetAllStore();
            if (stores != null)
                return stores.Select(store => new Models.Store(store.StoreID, store.Number, store.Name, store.HubID, store.IsTemporary, store.IsActive, store.StackCount, store.StoreManName)).ToList();
            return null;
        }
        /// <summary>
        /// Get a single store by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.Store Get(int id)
        {
            var store = _storeService.FindById(id);
            if (store!=null)
            {
                var newStore = new Models.Store(store.StoreID, store.Number, store.Name, store.HubID, store.IsTemporary,
                                                store.IsActive, store.StackCount, store.StoreManName);
                return newStore;
            }
            return null;
        }
    }
}
