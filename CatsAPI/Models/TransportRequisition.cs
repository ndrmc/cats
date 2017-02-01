 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Web;
 using Cats.Models;

 namespace Cats.Rest.Models
 {
     public class TransportRequisition
 		{

		 public TransportRequisition(int TransportRequisitionId , string TransportRequisitionNo , int RegionId , string RegionName , int ProgramId , string ProgramName , int RequestedBy , DateTime RequestedDate , int CertifiedBy , DateTime CertifiedDate , string Remark , int Status , string StatusName   )
			{
				this.TransportRequisitionId = TransportRequisitionId;
				this.TransportRequisitionNo = TransportRequisitionNo;
				this.RegionId = RegionId;
				this.RegionName = RegionName;
				this.ProgramId = ProgramId;
				this.ProgramName = ProgramName;
				this.RequestedBy = RequestedBy;
				this.RequestedDate = RequestedDate;
				this.CertifiedBy = CertifiedBy;
				this.CertifiedDate = CertifiedDate;
				this.Remark = Remark;
				this.Status = Status;
				this.StatusName = StatusName;
			 }


				public int TransportRequisitionId {get;set;}
				public string TransportRequisitionNo {get;set;}
				public int RegionId {get;set;}
				public string  RegionName{get;set;}
				public int  ProgramId{get;set;}
				public string  ProgramName{get;set;}
				public int  RequestedBy{get;set;}
				public DateTime RequestedDate {get;set;}
				public int CertifiedBy {get;set;}
				public DateTime CertifiedDate {get;set;}
				public string Remark {get;set;}
				public int Status {get;set;}
				public string StatusName {get;set;}
				public ICollection<TransportRequisitionDetail> TransportRequisitionDetails{get;set;}


		}
	}
