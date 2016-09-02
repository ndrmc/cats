using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats.Models;
using Cats.Data.UnitWork;

namespace Cats.Services.Common
{

   public class LedgerService : ILedgerService
    {

#region "SI Struct"

        public class DispatchedQuantity 
        {
            public System.Guid DispatchAllocationID { get; set; }
            public decimal AllocatedAmountInMT { get; set; }
            public decimal? DispatchedAmountInMT { get; set; }
            public decimal? DispatchedAmountInUnit { get; set; }
        }
       public class AvailableShippingCodes
       {
           private decimal _amount;
           private int? _siCodeId;
           private string _SIcode;
           private string _hubName;
           private int _hubId;

           public int HubId
           {
               get { return _hubId; }
               set { _hubId = value; }
           }

           public string HubName
           {
               get { return _hubName; }
               set { _hubName = value; }
           }
           public decimal amount
           {
               get { return _amount; }
               set { _amount = value; }
           }

           public int? siCodeId
           {
               get { return _siCodeId; }
               set { _siCodeId = value; }
           }
           public string SIcode
           {
               get { return _SIcode; }
               set { _SIcode = value; }
           }
       }

#endregion

#region "PC Struct"
       public class AvailableProjectCodes
       {
           private decimal _amount;
           private int? _pcCodeId;
           private string _PCcode;
           private string _hubName;
           private int _hubId;

           public int HubId
           {
               get { return _hubId; }
               set { _hubId = value; }
           }

           public string HubName
           {
               get { return _hubName; }
               set { _hubName = value; }
           }

           public decimal amount
           {
               get { return _amount; }
               set { _amount = value; }
           }

           public int? pcCodeId
           {
               get { return _pcCodeId; }
               set { _pcCodeId = value; }
           }
           public string PCcode 
           {
               get { return _PCcode; }
               set { _PCcode = value; }
           }

       }

#endregion


       private readonly IUnitOfWork _unitOfWork;

        public LedgerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region "SI Code"

        public List<AvailableShippingCodes> GetFreeSICodesByCommodityAmount(int hubId, int commodityId, System.Guid dispatchAllocationId)
        {
            String dispatchedQtyQuery = String.Format(@"SELECT SUM(dbo.VWDispatchAllocationPartial.QuantityInMT) AS DispatchedAmountInMT, SUM(dbo.VWDispatchAllocationPartial.QuantityInUnit) AS DispatchedAmountInUnit,
                        dbo.VWDispatchAllocationPartial.DispatchAllocationID AS DispatchAllocationID, dbo.VWDispatchAllocationPartial.AmountInUnit AS AllocatedAmountInMT
                    FROM     dbo.VWDispatchAllocationPartial LEFT OUTER JOIN
                        Procurement.Transporter ON dbo.VWDispatchAllocationPartial.TransporterID = Procurement.Transporter.TransporterID LEFT OUTER JOIN
                        dbo.Commodity ON dbo.VWDispatchAllocationPartial.CommodityID = dbo.Commodity.CommodityID LEFT OUTER JOIN
                        dbo.FDP ON dbo.FDP.FDPID = dbo.VWDispatchAllocationPartial.FDPID LEFT OUTER JOIN
                        dbo.VWAdminUnitHierarchy ON dbo.VWAdminUnitHierarchy.WoredaID = dbo.FDP.AdminUnitID
                    WHERE  ((dbo.VWDispatchAllocationPartial.LedgerID IN
                        (SELECT LedgerID
                        FROM      dbo.Ledger
                        WHERE   (Name LIKE 'Commited To FDP'))) OR (dbo.VWDispatchAllocationPartial.LedgerID IS NULL)) AND dbo.VWDispatchAllocationPartial.DispatchAllocationID = '{0}'
                    GROUP BY dbo.VWDispatchAllocationPartial.ShippingInstructionID, dbo.VWDispatchAllocationPartial.ProjectCodeID, dbo.VWDispatchAllocationPartial.HubID, dbo.VWDispatchAllocationPartial.IsClosed, 
                        dbo.Commodity.CommodityTypeID, dbo.VWDispatchAllocationPartial.AmountInUnit, dbo.VWDispatchAllocationPartial.DispatchAllocationID, dbo.VWDispatchAllocationPartial.RequisitionNo, 
                        dbo.VWDispatchAllocationPartial.BidRefNo, dbo.VWDispatchAllocationPartial.Round, dbo.VWDispatchAllocationPartial.CommodityID, dbo.VWDispatchAllocationPartial.FDPID, dbo.FDP.Name, 
                        dbo.VWAdminUnitHierarchy.WoredaID, dbo.VWAdminUnitHierarchy.WoredaName, dbo.VWAdminUnitHierarchy.ZoneID, dbo.VWAdminUnitHierarchy.ZoneName, dbo.VWAdminUnitHierarchy.RegionID, 
                        dbo.VWAdminUnitHierarchy.RegionName, dbo.Commodity.Name, dbo.VWDispatchAllocationPartial.TransporterID, Procurement.Transporter.Name, dbo.VWDispatchAllocationPartial.StoreID, 
                        dbo.VWDispatchAllocationPartial.Year, dbo.VWDispatchAllocationPartial.DonorID, dbo.VWDispatchAllocationPartial.Month, dbo.VWDispatchAllocationPartial.ProgramID, 
                        dbo.VWDispatchAllocationPartial.ContractStartDate, dbo.VWDispatchAllocationPartial.RequisitionId, dbo.VWDispatchAllocationPartial.ContractEndDate, dbo.VWDispatchAllocationPartial.Beneficiery, 
                        dbo.VWDispatchAllocationPartial.Unit, dbo.VWDispatchAllocationPartial.TransportOrderID, dbo.VWDispatchAllocationPartial.RequisitionId
                    HAVING (dbo.VWDispatchAllocationPartial.ShippingInstructionID IS NOT NULL) OR (dbo.VWDispatchAllocationPartial.ProjectCodeID IS NOT NULL)
                    ", dispatchAllocationId);
            var dispatchedQtyRow = _unitOfWork.Database.SqlQuery<DispatchedQuantity>(dispatchedQtyQuery).ToList().FirstOrDefault();
            decimal remainingQty = dispatchedQtyRow.AllocatedAmountInMT;
            if (dispatchedQtyRow.DispatchedAmountInMT != null && dispatchedQtyRow.DispatchedAmountInMT > 0)
            {
                remainingQty = dispatchedQtyRow.AllocatedAmountInMT - (decimal)dispatchedQtyRow.DispatchedAmountInMT;
            }

            String query = String.Format(@"SELECT SOH.QuantityInMT - ABS(ISNULL(Commited.QuantityInMT, 0)) as amount, SOH.ShippingInstructionID siCodeId, ShippingInstruction.Value SIcode, SOH.HubID as HubId, Hub.Name HubName 
                                                        from (SELECT SUM(QuantityInMT) QuantityInMT , ShippingInstructionID, HubID from [Transaction] 
					                                        WHERE LedgerID = {0} and CommodityID = {2} and HubID = {3}
					                                        GROUP BY HubID, ShippingInstructionID) AS SOH
	                                            LEFT JOIN (SELECT SUM(QuantityInMT) QuantityInMT, ShippingInstructionID, HubID from [Transaction]
					                                        WHERE LedgerID = {1} and CommodityID = {2} and HubID = {3}
					                                        GROUP By HubID, ShippingInstructionID) AS Commited	
		                                            ON SOH.ShippingInstructionID = Commited.ShippingInstructionID and SOH.HubId = Commited.HubId
	                                            JOIN ShippingInstruction 
		                                            ON SOH.ShippingInstructionID = ShippingInstruction.ShippingInstructionID 
                                                JOIN Hub
                                                    ON Hub.HubID = SOH.HubID
                                                WHERE 
                                                 SOH.QuantityInMT - ISNULL(Commited.QuantityInMT, 0) > {4}    
                                                ", Ledger.Constants.GOODS_ON_HAND, Ledger.Constants.COMMITED_TO_FDP, commodityId, hubId, remainingQty);

            var availableShippingCodes = _unitOfWork.Database.SqlQuery<AvailableShippingCodes>(query);

            return availableShippingCodes.ToList();
        }



        /// <summary>
        /// Gets the balance of an SI number commodity .
        /// </summary>
        /// <param name="hubId">The hub id.</param>
        /// <param name="commodityId">The commodity id.</param>

        /// <returns>available amount,shipping Instruction Id, and Shipping Instruction Code</returns>
        public List<AvailableShippingCodes> GetFreeSICodesByCommodity(int hubId, int commodityId)
        {
            var hubs=_unitOfWork.HubAllocationRepository.GetAll().Select(m=>m.HubID).Distinct();
            var listHubs=string.Join(", ", from item in hubs select item);
            String HubFilter = (hubId > 0) ? string.Format(" And HubID IN ({0},{1},{2})", 1,2,3) : "";

            String query = String.Format(@"SELECT SOH.QuantityInMT - ABS(ISNULL(Commited.QuantityInMT, 0)) as amount, SOH.ShippingInstructionID siCodeId, ShippingInstruction.Value SIcode, SOH.HubID as HubId, Hub.Name HubName 
                                                        from (SELECT SUM(QuantityInMT) QuantityInMT , ShippingInstructionID, HubID from [Transaction] 
					                                        WHERE LedgerID = {0} and CommodityID = {2} and HubID IN({3})
					                                        GROUP BY HubID, ShippingInstructionID) AS SOH
	                                            LEFT JOIN (SELECT SUM(QuantityInMT) QuantityInMT, ShippingInstructionID, HubID from [Transaction]
					                                        WHERE LedgerID = {1} and CommodityID = {2} and HubID IN ({3})
					                                        GROUP By HubID, ShippingInstructionID) AS Commited	
		                                            ON SOH.ShippingInstructionID = Commited.ShippingInstructionID and SOH.HubId = Commited.HubId
	                                            JOIN ShippingInstruction 
		                                            ON SOH.ShippingInstructionID = ShippingInstruction.ShippingInstructionID 
                                                JOIN Hub
                                                    ON Hub.HubID = SOH.HubID
                                                WHERE 
                                                 SOH.QuantityInMT - ISNULL(Commited.QuantityInMT, 0) > 0    
                                                ", Ledger.Constants.GOODS_ON_HAND, Ledger.Constants.COMMITED_TO_FDP, commodityId, listHubs);

            var availableShippingCodes = _unitOfWork.Database.SqlQuery<AvailableShippingCodes>(query);
           
            return availableShippingCodes.ToList();
        }

        
        #endregion


      


        public List<AvailableProjectCodes> GetFreePCCodesByCommodity(int hubId, int commodityId)
        {
            String HubFilter = (hubId > 0) ? string.Format(" And HubID IN ({0},{1},{2})", 1,2,3) : "";
            var hubs = _unitOfWork.HubAllocationRepository.GetAll().Select(m => m.HubID).Distinct();
            var listHubs = string.Join(", ", from item in hubs select item);
            String query = String.Format(@"SELECT SOH.QuantityInMT - ABS(ISNULL(Commited.QuantityInMT, 0)) as amount, SOH.ProjectCodeID pcCodeId, ProjectCode.Value PCcode, SOH.HubID as HubId, Hub.Name HubName 
                                                        from (SELECT SUM(QuantityInMT) QuantityInMT , ProjectCodeID, HubID from [Transaction] 
					                                        WHERE LedgerID = {0} and CommodityID = {2} AND HubID IN ({3}) and ShippingInstructionID = NULL
					                                        GROUP BY HubID, ProjectCodeID) AS SOH
	                                            LEFT JOIN (SELECT SUM(QuantityInMT) QuantityInMT, ProjectCodeID, HubID from [Transaction]
					                                        WHERE LedgerID = {1} and CommodityID = {2} and HubID IN ({3}) and ShippingInstructionID = NULL
					                                        GROUP By HubID, ProjectCodeID) AS Commited	
		                                            ON SOH.ProjectCodeID = Commited.ProjectCodeID and SOH.HubId = Commited.HubId
	                                            JOIN ProjectCode 
		                                            ON SOH.ProjectCodeID = ProjectCode.ProjectCodeID 
                                                JOIN Hub
                                                    ON Hub.HubID = SOH.HubID
                                                WHERE 
                                                 SOH.QuantityInMT - ISNULL(Commited.QuantityInMT, 0) > 0    
                                                ", Ledger.Constants.GOODS_ON_HAND, Ledger.Constants.COMMITED_TO_FDP, commodityId, listHubs);

            var availableShippingCodes = _unitOfWork.Database.SqlQuery<AvailableProjectCodes>(query);

            return availableShippingCodes.ToList();
        }


     

    }
}
