using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cats.Rest.Models
{
    public class Store
    {
        public int StoreID { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int HubID { get; set; }
        public bool IsTemporary { get; set; }
        public bool IsActive { get; set; }
        public int StackCount { get; set; }
        public string StoreManName { get; set; }



        public Store(int storeId, int number, string name, int hubId, bool isTemporary, bool isActive, int stackCount,
                     string storeManName)
        {
            StoreID = storeId;
            Number = number;
            Name = name;
            HubID = hubId;
            IsTemporary = isTemporary;
            IsActive = isActive;
            StackCount = stackCount;
            StoreManName = storeManName;
        }
    }
}