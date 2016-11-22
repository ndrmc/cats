﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Models.Hubs;

namespace Cats.Models.Hubs
{
    public class CurrentUserModel
    {
     
        public string Owner { get; set; }
        public string Name { get; set; }
        public int DefaultHubId { get; set; }
        public List<Hub> PossibleHubs { get; set; }

        // Hide the constructor that works without having known the current user.
        public CurrentUserModel()
        {
        }

        public CurrentUserModel(UserProfile user)
        {
           // this.Owner = user.DefaultHub.HubOwner.Name;
            this.Name = user.DefaultHubObj.Name;
            this.DefaultHubId = user.DefaultHubObj.HubID;
            PossibleHubs = user.UserAllowedHubs;
        }
    }

    public class UserHubModel
    {
        public int HubID { get; set; }
        public string Name { get; set; }
        public bool Selected { get; set; }
    }
}