 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Web;
 using Cats.Models;

 namespace Cats.Rest.Models
 {
     public class TransactionType
 		{

		 public TransactionType(int TransactionTypeID , string Name , string Descripton  )
			{
				this.TransactionTypeID = TransactionTypeID;
				this.Name = Name;
				this.Descripton = Descripton;
			 }


				public int TransactionTypeID{get;set;}
				public string  Name{get;set;}
				public string  Descripton{get;set;}


		}
	}
