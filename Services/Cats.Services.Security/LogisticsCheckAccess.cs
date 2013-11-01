﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// ------------------------------------------------------------------------------
//  <auto-generated>
//     This code was generated by NetSqlAzMan CodeDom.
//     NetSqlAzMan - Andrea Ferendeles - http://netsqlazman.codeplex.com
//     NetSqlAzMan Version: 3.6.0.15
//     CLR Version: v4.0.30319
//     <NetSqlAzMan-info>
//        Store: CATS
//        Application: Logistics
//        Last update: 10/30/2013 10:30:30 AM
//     </NetSqlAzMan-info>
//  </auto-generated>
// ------------------------------------------------------------------------------
// 
// 
// TODO: Add NetSqlAzMan.dll Assembly reference.
// 
// 
namespace Cats.Services.Security
{
    using System;
    using System.Security.Principal;
    using System.Collections.Generic;
    using System.Text;
    using NetSqlAzMan;
    using NetSqlAzMan.Interfaces;

    /// <summary>
    /// NetSqlAzMan Check Access Helper Class for NetSqlAzMan 'Logistics' Application 
    /// </summary>
    public partial class LogisticsCheckAccess : ILogisticsCheckAccess
    {
        #region Constants
        /// <summary>
        /// Store Name
        /// </summary>
        protected internal const string STORE_NAME = "CATS";
        /// <summary>
        /// Application Name
        /// </summary>
        protected internal const string APPLICATION_NAME = "Logistics";
        #endregion
        #region Fields
        /// <summary>
        /// NetSqlAzMan Storage reference.
        /// </summary>
        protected NetSqlAzMan.Interfaces.IAzManStorage mStorage;
        /// <summary>
        /// User Windows Principal Identity.
        /// </summary>
        protected System.Security.Principal.WindowsIdentity windowsIdentity;
        #endregion
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LogisticsCheckAccess"/> class [Windows Users ONLY].
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <param name="windowsIdentity">The Windows Principal Identity.</param>
        public LogisticsCheckAccess(string storageConnectionString, System.Security.Principal.WindowsIdentity windowsIdentity)
        {
            this.mStorage = new NetSqlAzMan.SqlAzManStorage(storageConnectionString);
            this.windowsIdentity = windowsIdentity;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:LogisticsCheckAccess"/> class [DB Users ONLY].
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        public LogisticsCheckAccess(string storageConnectionString)
        {
            this.mStorage = new NetSqlAzMan.SqlAzManStorage(storageConnectionString);
        }
        #endregion
        #region Properties
        /// <summary>
        /// Gets the NetSqlAzMan Storage.
        /// </summary>
        public virtual NetSqlAzMan.Interfaces.IAzManStorage Storage
        {
            get
            {
                return this.mStorage;
            }
        }
        #endregion
        #region Methods
        /// <summary>
        /// Opens the connection
        /// </summary>
        public virtual void OpenConnection()
        {
            this.mStorage.OpenConnection();
        }
        /// <summary>
        /// Closes the connection
        /// </summary>
        public virtual void CloseConnection()
        {
            this.mStorage.CloseConnection();
        }
        /// <summary>
        /// Retrieve Item name from a Operation Enum.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns>The Operation Name.</returns>
        public virtual string ItemName(Operation operation)
        {
            if ((operation == Operation.Assign_Hub))
            {
                return "Assign Hub";
            }
            if ((operation == Operation.Delete_transporter))
            {
                return "Delete transporter";
            }
            if ((operation == Operation.Edit__transport_order))
            {
                return "Edit  transport order";
            }
            if ((operation == Operation.Edit_transport_supplier))
            {
                return "Edit transport supplier";
            }
            if ((operation == Operation.Export_bid_winner_list))
            {
                return "Export bid winner list";
            }
            if ((operation == Operation.Export_hub_allocation))
            {
                return "Export hub allocation";
            }
            if ((operation == Operation.Export_transport_order))
            {
                return "Export transport order";
            }
            if ((operation == Operation.Export_transporters_list))
            {
                return "Export transporters list";
            }
            if ((operation == Operation.New_transport_order))
            {
                return "New transport order";
            }
            if ((operation == Operation.New_transport_supplier))
            {
                return "New transport supplier";
            }
            if ((operation == Operation.Price_quatation_data_entry))
            {
                return "Price quatation data entry";
            }
            if ((operation == Operation.Print_bid_winners_list))
            {
                return "Print bid winners list";
            }
            if ((operation == Operation.Print_hub_allocation))
            {
                return "Print hub allocation";
            }
            if ((operation == Operation.Print_RFQ))
            {
                return "Print RFQ";
            }
            if ((operation == Operation.Print_transport_order))
            {
                return "Print transport order";
            }
            if ((operation == Operation.Print_transporters_list))
            {
                return "Print transporters list";
            }
            if ((operation == Operation.View_active_contracts))
            {
                return "View active contracts";
            }
            if ((operation == Operation.View_allocated_hubs))
            {
                return "View allocated hubs";
            }
            if ((operation == Operation.View_bid_winner_transporters))
            {
                return "View bid winner transporters";
            }
            if ((operation == Operation.View_contract_history))
            {
                return "View contract history";
            }
            if ((operation == Operation.View_draft_hub_allocation))
            {
                return "View draft hub allocation";
            }
            if ((operation == Operation.View_transporter_list))
            {
                return "View transporter list";
            }
            throw new System.ArgumentException("Unknown Operation name", "operation");
        }
        /// <summary>
        /// Checks the access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="itemName">The Item Name.</param>
        /// <param name="operationsOnly">if set to <c>true</c> checks the access for operations only.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        protected virtual NetSqlAzMan.Interfaces.AuthorizationType CheckAccess(string itemName, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.mStorage.CheckAccess(LogisticsCheckAccess.STORE_NAME, LogisticsCheckAccess.APPLICATION_NAME, itemName, this.windowsIdentity, DateTime.Now, operationsOnly, contextParameters);
        }
        /// <summary>
        /// Checks the access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="itemName">The Item Name.</param>
        /// <param name="dbUserName">The DB User Name.</param>
        /// <param name="operationsOnly">if set to <c>true</c> checks the access for operations only.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        protected virtual NetSqlAzMan.Interfaces.AuthorizationType CheckAccess(string itemName, string dbUserName, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.mStorage.CheckAccess(LogisticsCheckAccess.STORE_NAME, LogisticsCheckAccess.APPLICATION_NAME, itemName, this.mStorage.GetDBUser(dbUserName), DateTime.Now, operationsOnly, contextParameters);
        }
        /// <summary>
        /// Checks the access [FOR custom SID ONLY].
        /// </summary>
        /// <param name="itemName">The Item Name.</param>
        /// <param name="customSID">The custom SID.</param>
        /// <param name="operationsOnly">if set to <c>true</c> checks the access for operations only.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        protected virtual NetSqlAzMan.Interfaces.AuthorizationType CheckAccess(string itemName, NetSqlAzMan.Interfaces.IAzManSid customSID, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.mStorage.CheckAccess(LogisticsCheckAccess.STORE_NAME, LogisticsCheckAccess.APPLICATION_NAME, itemName, this.mStorage.GetDBUser(customSID), DateTime.Now, operationsOnly, contextParameters);
        }
        /// <summary>
        /// Checks the access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="itemName">The Item Name.</param>
        /// <param name="operationsOnly">if set to <c>true</c> checks the access for operations only.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        protected virtual NetSqlAzMan.Interfaces.AuthorizationType CheckAccess(string itemName, bool operationsOnly, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.mStorage.CheckAccess(LogisticsCheckAccess.STORE_NAME, LogisticsCheckAccess.APPLICATION_NAME, itemName, this.windowsIdentity, DateTime.Now, operationsOnly, out attributes, contextParameters);
        }
        /// <summary>
        /// Checks the access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="itemName">The Item Name.</param>
        /// <param name="dbUserName">The DB User Name.</param>
        /// <param name="operationsOnly">if set to <c>true</c> checks the access for operations only.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        protected virtual NetSqlAzMan.Interfaces.AuthorizationType CheckAccess(string itemName, string dbUserName, bool operationsOnly, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.mStorage.CheckAccess(LogisticsCheckAccess.STORE_NAME, LogisticsCheckAccess.APPLICATION_NAME, itemName, this.mStorage.GetDBUser(dbUserName), DateTime.Now, operationsOnly, out attributes, contextParameters);
        }
        /// <summary>
        /// Checks the access [FOR Custom SID ONLY].
        /// </summary>
        /// <param name="itemName">The Item Name.</param>
        /// <param name="customSID">The custom SID.</param>
        /// <param name="operationsOnly">if set to <c>true</c> checks the access for operations only.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        protected virtual NetSqlAzMan.Interfaces.AuthorizationType CheckAccess(string itemName, NetSqlAzMan.Interfaces.IAzManSid customSID, bool operationsOnly, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.mStorage.CheckAccess(LogisticsCheckAccess.STORE_NAME, LogisticsCheckAccess.APPLICATION_NAME, itemName, this.mStorage.GetDBUser(customSID), DateTime.Now, operationsOnly, out attributes, contextParameters);
        }
        /// <summary>
        /// Gets the Authorization Type [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        public virtual NetSqlAzMan.Interfaces.AuthorizationType GetAuthorizationType(Operation operation, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.CheckAccess(this.ItemName(operation), true, contextParameters);
        }
        /// <summary>
        /// Gets the Authorization Type [FOR DB Users ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="dbUserName">The DB UserName.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        public virtual NetSqlAzMan.Interfaces.AuthorizationType GetAuthorizationType(Operation operation, string dbUserName, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.CheckAccess(this.ItemName(operation), dbUserName, true, contextParameters);
        }
        /// <summary>
        /// Gets the Authorization Type [FOR Custom SID ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="customSID">The Custom SID.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        public virtual NetSqlAzMan.Interfaces.AuthorizationType GetAuthorizationType(Operation operation, NetSqlAzMan.Interfaces.IAzManSid customSID, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.CheckAccess(this.ItemName(operation), customSID, true, contextParameters);
        }
        /// <summary>
        /// Gets the Authorization Type [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        public virtual NetSqlAzMan.Interfaces.AuthorizationType GetAuthorizationType(Operation operation, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.CheckAccess(this.ItemName(operation), true, out attributes, contextParameters);
        }
        /// <summary>
        /// Gets the Authorization Type [FOR DB Users ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="dbUserName">The DB UserName.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        public virtual NetSqlAzMan.Interfaces.AuthorizationType GetAuthorizationType(Operation operation, string dbUserName, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.CheckAccess(this.ItemName(operation), dbUserName, true, out attributes, contextParameters);
        }
        /// <summary>
        /// Gets the Authorization Type [FOR custom SID ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="customSID">The Custom SID.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral].</returns>
        public virtual NetSqlAzMan.Interfaces.AuthorizationType GetAuthorizationType(Operation operation, NetSqlAzMan.Interfaces.IAzManSid customSID, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.CheckAccess(this.ItemName(operation), customSID, true, out attributes, contextParameters);
        }
        /// <summary>
        /// Checks the access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>True for Access Granted, False for Access Denied.</returns>
        public virtual bool CheckAccess(Operation operation, params KeyValuePair<string, object>[] contextParameters)
        {
            NetSqlAzMan.Interfaces.AuthorizationType result = this.GetAuthorizationType(operation, contextParameters);
            return ((result == AuthorizationType.AllowWithDelegation)
                        || (result == AuthorizationType.Allow));
        }
        /// <summary>
        /// Checks the access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="dbUserName">The DB UserName.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>True for Access Granted, False for Access Denied.</returns>
        public virtual bool CheckAccess(Operation operation, string dbUserName, params KeyValuePair<string, object>[] contextParameters)
        {
            NetSqlAzMan.Interfaces.AuthorizationType result = this.GetAuthorizationType(operation, dbUserName, contextParameters);
            return ((result == AuthorizationType.AllowWithDelegation)
                        || (result == AuthorizationType.Allow));
        }
        /// <summary>
        /// Checks the access [FOR custom SID ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="customSID">The custom SID.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>True for Access Granted, False for Access Denied.</returns>
        public virtual bool CheckAccess(Operation operation, NetSqlAzMan.Interfaces.IAzManSid customSID, params KeyValuePair<string, object>[] contextParameters)
        {
            NetSqlAzMan.Interfaces.AuthorizationType result = this.GetAuthorizationType(operation, customSID, contextParameters);
            return ((result == AuthorizationType.AllowWithDelegation)
                        || (result == AuthorizationType.Allow));
        }
        /// <summary>
        /// Checks the access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>True for Access Granted, False for Access Denied.</returns>
        public virtual bool CheckAccess(Operation operation, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            NetSqlAzMan.Interfaces.AuthorizationType result = this.GetAuthorizationType(operation, out attributes, contextParameters);
            return ((result == AuthorizationType.AllowWithDelegation)
                        || (result == AuthorizationType.Allow));
        }
        /// <summary>
        /// Checks the access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="dbUserName">The DB UserName.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>True for Access Granted, False for Access Denied.</returns>
        public virtual bool CheckAccess(Operation operation, string dbUserName, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            NetSqlAzMan.Interfaces.AuthorizationType result = this.GetAuthorizationType(operation, dbUserName, out attributes, contextParameters);
            return ((result == AuthorizationType.AllowWithDelegation)
                        || (result == AuthorizationType.Allow));
        }
        /// <summary>
        /// Checks the access [FOR custom SID ONLY].
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <param name="customSID">The custom SID.</param>
        /// <param name="attributes">Retrieved attributes.</param>
        /// <param name="contextParameters">Context Parameters for Biz Rules.</param>
        /// <returns>True for Access Granted, False for Access Denied.</returns>
        public virtual bool CheckAccess(Operation operation, NetSqlAzMan.Interfaces.IAzManSid customSID, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            NetSqlAzMan.Interfaces.AuthorizationType result = this.GetAuthorizationType(operation, customSID, out attributes, contextParameters);
            return ((result == AuthorizationType.AllowWithDelegation)
                        || (result == AuthorizationType.Allow));
        }
        #endregion
        #region Enums
        /// <summary>
        /// Operations Enumeration
        /// </summary>
        public enum Operation
        {
            /// <summary>
            /// Operation Assign Hub
            /// </summary>
            Assign_Hub,
            /// <summary>
            /// Operation Delete transporter
            /// </summary>
            Delete_transporter,
            /// <summary>
            /// Operation Edit  transport order
            /// </summary>
            Edit__transport_order,
            /// <summary>
            /// Operation Edit transport supplier
            /// </summary>
            Edit_transport_supplier,
            /// <summary>
            /// Operation Export bid winner list
            /// </summary>
            Export_bid_winner_list,
            /// <summary>
            /// Operation Export hub allocation
            /// </summary>
            Export_hub_allocation,
            /// <summary>
            /// Operation Export transport order
            /// </summary>
            Export_transport_order,
            /// <summary>
            /// Operation Export transporters list
            /// </summary>
            Export_transporters_list,
            /// <summary>
            /// Operation New transport order
            /// </summary>
            New_transport_order,
            /// <summary>
            /// Operation New transport supplier
            /// </summary>
            New_transport_supplier,
            /// <summary>
            /// Operation Price quatation data entry
            /// </summary>
            Price_quatation_data_entry,
            /// <summary>
            /// Operation Print bid winners list
            /// </summary>
            Print_bid_winners_list,
            /// <summary>
            /// Operation Print hub allocation
            /// </summary>
            Print_hub_allocation,
            /// <summary>
            /// Operation Print RFQ
            /// </summary>
            Print_RFQ,
            /// <summary>
            /// Operation Print transport order
            /// </summary>
            Print_transport_order,
            /// <summary>
            /// Operation Print transporters list
            /// </summary>
            Print_transporters_list,
            /// <summary>
            /// Operation View active contracts
            /// </summary>
            View_active_contracts,
            /// <summary>
            /// Operation View allocated hubs
            /// </summary>
            View_allocated_hubs,
            /// <summary>
            /// Operation View bid winner transporters
            /// </summary>
            View_bid_winner_transporters,
            /// <summary>
            /// Operation View contract history
            /// </summary>
            View_contract_history,
            /// <summary>
            /// Operation View draft hub allocation
            /// </summary>
            View_draft_hub_allocation,
            /// <summary>
            /// Operation View transporter list
            /// </summary>
            View_transporter_list,
        }
        #endregion
    }
}

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// ------------------------------------------------------------------------------
//  <auto-generated>
//     This code was generated by NetSqlAzMan CodeDom.
//     NetSqlAzMan - Andrea Ferendeles - http://netsqlazman.codeplex.com
//     NetSqlAzMan Version: 3.6.0.15
//     CLR Version: v4.0.30319
//     <NetSqlAzMan-info>
//        Store: CATS
//        Application: Logistics
//        Last update: 10/30/2013 10:30:30 AM
//     </NetSqlAzMan-info>
//  </auto-generated>
// ------------------------------------------------------------------------------
// 
namespace Logistics.Security
{
    using System;

    /// <summary>
    /// NetSqlAzMan OPERATION Helper Class for NetSqlAzMan 'Logistics' Application 
    /// </summary>
    public partial class OPERATION
    {
        /// <summary>
        /// OPERATION 'Assign Hub'
        /// </summary>
        public const string ASSIGN_HUB = "Assign Hub";
        /// <summary>
        /// OPERATION 'Delete transporter'
        /// </summary>
        public const string DELETE_TRANSPORTER = "Delete transporter";
        /// <summary>
        /// OPERATION 'Edit  transport order'
        /// </summary>
        public const string EDIT__TRANSPORT_ORDER = "Edit  transport order";
        /// <summary>
        /// OPERATION 'Edit transport supplier'
        /// </summary>
        public const string EDIT_TRANSPORT_SUPPLIER = "Edit transport supplier";
        /// <summary>
        /// OPERATION 'Export bid winner list'
        /// </summary>
        public const string EXPORT_BID_WINNER_LIST = "Export bid winner list";
        /// <summary>
        /// OPERATION 'Export hub allocation'
        /// </summary>
        public const string EXPORT_HUB_ALLOCATION = "Export hub allocation";
        /// <summary>
        /// OPERATION 'Export transport order'
        /// </summary>
        public const string EXPORT_TRANSPORT_ORDER = "Export transport order";
        /// <summary>
        /// OPERATION 'Export transporters list'
        /// </summary>
        public const string EXPORT_TRANSPORTERS_LIST = "Export transporters list";
        /// <summary>
        /// OPERATION 'New transport order'
        /// </summary>
        public const string NEW_TRANSPORT_ORDER = "New transport order";
        /// <summary>
        /// OPERATION 'New transport supplier'
        /// </summary>
        public const string NEW_TRANSPORT_SUPPLIER = "New transport supplier";
        /// <summary>
        /// OPERATION 'Price quatation data entry'
        /// </summary>
        public const string PRICE_QUATATION_DATA_ENTRY = "Price quatation data entry";
        /// <summary>
        /// OPERATION 'Print bid winners list'
        /// </summary>
        public const string PRINT_BID_WINNERS_LIST = "Print bid winners list";
        /// <summary>
        /// OPERATION 'Print hub allocation'
        /// </summary>
        public const string PRINT_HUB_ALLOCATION = "Print hub allocation";
        /// <summary>
        /// OPERATION 'Print RFQ'
        /// </summary>
        public const string PRINT_RFQ = "Print RFQ";
        /// <summary>
        /// OPERATION 'Print transport order'
        /// </summary>
        public const string PRINT_TRANSPORT_ORDER = "Print transport order";
        /// <summary>
        /// OPERATION 'Print transporters list'
        /// </summary>
        public const string PRINT_TRANSPORTERS_LIST = "Print transporters list";
        /// <summary>
        /// OPERATION 'View active contracts'
        /// </summary>
        public const string VIEW_ACTIVE_CONTRACTS = "View active contracts";
        /// <summary>
        /// OPERATION 'View allocated hubs'
        /// </summary>
        public const string VIEW_ALLOCATED_HUBS = "View allocated hubs";
        /// <summary>
        /// OPERATION 'View bid winner transporters'
        /// </summary>
        public const string VIEW_BID_WINNER_TRANSPORTERS = "View bid winner transporters";
        /// <summary>
        /// OPERATION 'View contract history'
        /// </summary>
        public const string VIEW_CONTRACT_HISTORY = "View contract history";
        /// <summary>
        /// OPERATION 'View draft hub allocation'
        /// </summary>
        public const string VIEW_DRAFT_HUB_ALLOCATION = "View draft hub allocation";
        /// <summary>
        /// OPERATION 'View transporter list'
        /// </summary>
        public const string VIEW_TRANSPORTER_LIST = "View transporter list";
    }
}