using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using Cats.Services.Hub;

namespace Cats.Rest.Controllers
{
    public class ShippingInstructionController : ApiController

    {
        
        //
        // GET: /ShippingInstruction/

        private IShippingInstructionService _shippingInstructionService;

        public ShippingInstructionController(IShippingInstructionService shippingInstructionService)
        {
            _shippingInstructionService = shippingInstructionService;
        }
        /// <summary>
        /// Return all shipping Instructions
        /// </summary>
        /// <returns></returns>
        public List<Models.ShippingInstruction> Get()
        {
            var shippingInstructions = _shippingInstructionService.GetAllShippingInstruction();
            return shippingInstructions.Select(shippingInstruction => new Models.ShippingInstruction(shippingInstruction.ShippingInstructionID, shippingInstruction.Value)).ToList();
           
        }
        /// <summary>
        /// Gets a sinlge shipping Intruction # by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.ShippingInstruction Get(int id)
        {
            var shippingInstructions = _shippingInstructionService.FindById(id);
            if (shippingInstructions!=null)
            {
                var newSH = new Models.ShippingInstruction(shippingInstructions.ShippingInstructionID,
                                                           shippingInstructions.Value);
                return newSH;
            }
            return null;
        }
    }
}
