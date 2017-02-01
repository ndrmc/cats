 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Web;
 using Cats.Models;

 namespace Cats.Rest.Models
 {
     public class UserHub
 		{

		 public UserHub(int HubId , int UserProfileID , string UserName ,  string IsDefault  )
			{
				this.UserHubId = UserHubId;
				this.UserProfileID = UserProfileID;
				this.UserName = UserName;
				this.HubId = HubId;
				this.IsDefault = IsDefault;
			 }


				public int UserHubId {get;set;}
				public int UserProfileID {get;set;}
				public string UserName {get;set;}
				public int HubId {get;set;}
				public string IsDefault {get;set;}


		}
	}
