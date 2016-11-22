using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cats.Models.Hubs
{

    public  class Account
    {    public class Constants
    {
        /// <summary>
        /// 
        /// </summary>
        public const string DONOR = "Donor";
        public const string FDP = "FDP";
        public const string HUBOWNER = "HubOwner";
        public const string HUB = "Hub";
    }
        public Account()
        {
          this.Transactions = new List<Transaction>();
        }
       [Key]
        public int AccountID { get; set; }
        public string EntityType { get; set; }
        public int EntityID { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
