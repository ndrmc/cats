using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class ShippingInstruction
    {
        public int ShippingInstructionID { get; set; }
        public string Value { get; set; }

        public ShippingInstruction(int shippingInstcutionId, string value)
        {
            ShippingInstructionID = shippingInstcutionId;
            Value = value;

        }
    }
}