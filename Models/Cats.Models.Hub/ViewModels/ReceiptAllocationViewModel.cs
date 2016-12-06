﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cats.Models.Hubs;
using Cats.Models.Hubs.ViewModels;

namespace Cats.Models.Hubs
{
    /// <summary>
    /// 
    /// </summary>
    public class ReceiptAllocationViewModel
    {
        //todo:code smell 
         // private IUnitOfWork _Repository = new UnitOfWork();
        private UserProfile _UserProfile = null;

        public List<Commodity> Commodities { get; set; }
        public string CommodityName { get; set; }
        public List<Donor> Donors { get; set; }
        public List<Hub> Hubs { get; set; }
        public List<Hub> AllHubs { get; set; }
        public List<GiftCertificateDetail> GiftCertificateDetail { get; set; }
        public List<Program> Programs { get; set; }
        public List<CommoditySource> CommoditySources { get; set; }
        public List<CommodityType> CommodityTypes { get; set; }
       
        public Nullable<int> UnitID { get; set; }
        public bool IsClosed { get; set; }
        public Decimal ReceivedQuantityInMT { get; set; }
        public string ReceivedQuantityInMTFormatted { get; set; }
        //public string GRN { get; set; }
        public Decimal RemainingBalanceInMT { get; set; }
        public decimal ReceivedQuantityInUnit { get; set; }
        public decimal RemainingBalanceInUnit { get; set; }
        public List<Receive> Receives { get; set; }
        public List<Receive> ReceivesList { get; set; }
        public int BusinessProcessID { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiptAllocationViewModel"/> class.
        /// </summary>
        public ReceiptAllocationViewModel()
        {
            //Commodities = _Repository.Commodity.GetAll().OrderBy(o => o.Name).ToList();
            //Donors = _Repository.Donor.GetAll().OrderBy(o => o.Name).ToList();
            //Hubs = _Repository.Hub.GetAll().OrderBy(o => o.Name).ToList();
           // GRN = ReceivedQuantityInMT.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceiptAllocationViewModel"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userProfile">The user profile.</param>
        public ReceiptAllocationViewModel(List<Commodity> commodities, List<Donor> donors, List<Hub> allHubs, List<Program> programs, List<CommoditySource> commoditySources, List<CommodityType> commodityTypes, UserProfile user, List<Receive> receiveList)
        {
            //_Repository = unitOfWork;
            _UserProfile = user;
            InitalizeViewModel(commodities, donors, allHubs, programs, commoditySources, commodityTypes,  receiveList);
        }

        /// <summary>
        /// Initalizes the view model.
        /// </summary>
        public void InitalizeViewModel(List<Commodity> commodities, List<Donor> donors, List<Hub> allHubs, List<Program> programs, List<CommoditySource> commoditySources, List<CommodityType> commodityTypes, List<Receive> receiveList )
        {
            Commodities = commodities;//_Repository.Commodity.GetAll().DefaultIfEmpty().OrderBy(o => o.Name).ToList();
            Donors = donors;// _Repository.Donor.GetAll().DefaultIfEmpty().OrderBy(o => o.Name).ToList();
            AllHubs = allHubs;
            //if (_UserProfile != null)
            //{
            //    Hubs = new List<Hub>() { _UserProfile.DefaultHub };
            //    AllHubs = allHubs;
            //        _Repository.Hub.GetAllWithoutId(_UserProfile.DefaultHub.HubID).DefaultIfEmpty().OrderBy(o => o.Name)
            //            .ToList();
            //}
            //else
            //{
            //    Hubs = new List<Hub>();
            //    AllHubs =
            //        _Repository.Hub.GetAll().DefaultIfEmpty().OrderBy(o => o.Name).ToList();
            //}
            Hubs = allHubs;
            ReceivesList = receiveList; 
            Programs = programs;// _Repository.Program.GetAll().DefaultIfEmpty().OrderBy(o => o.Name).ToList();
            CommoditySources = commoditySources;// _Repository.CommoditySource.GetAll().DefaultIfEmpty().OrderBy(o => o.Name).ToList();
            CommodityTypes = commodityTypes;// _Repository.CommodityType.GetAll().DefaultIfEmpty().OrderBy(o => o.Name).ToList();
        }



        /// <summary>
        /// Generates the receipt allocation.
        /// </summary>
        /// <returns></returns>
        public ReceiptAllocation GenerateReceiptAllocation()
        {
            ReceiptAllocation receiptAllocation = new ReceiptAllocation()
            {
                
                ProjectNumber = this.ProjectNumber,
                SINumber = this.SINumber,
                QuantityInMT = this.QuantityInMT,
                CommodityID = this.CommodityID,
                HubID = this.HubID,
                 
                ETA = this.ETA,
                DonorID = this.DonorID == null ? (int?)null : this.DonorID.Value,
                GiftCertificateDetailID = this.GiftCertificateDetailID,
                PartitionId = this.PartitionId,
                ProgramID = this.ProgramID,
                CommoditySourceID = this.CommoditySourceID,
                IsCommited = this.IsCommited,
                SourceHubID = this.SourceHubID,
                PurchaseOrder = this.PurchaseOrder,
                SupplierName = this.SupplierName,
                OtherDocumentationRef = this.OtherDocumentationRef,
                QuantityInUnit = this.QuantityInUnit,
                Receives = this.Receives,
                Remark = this.Remark
            };
            if (this.ReceiptAllocationID.HasValue)
            {
                receiptAllocation.ReceiptAllocationID = this.ReceiptAllocationID.Value;
            }
            
            return receiptAllocation;
        }

        /// <summary>
        /// Generates the allocation view model.
        /// </summary>
        /// <param name="receiptAllocation">The receipt allocation.</param>
        /// <returns></returns>
        public static ReceiptAllocationViewModel GenerateAllocationViewModel(ReceiptAllocation receiptAllocation)
        {
            ReceiptAllocationViewModel receiptAllocationViewModel = new ReceiptAllocationViewModel();

            receiptAllocationViewModel.ProjectNumber = receiptAllocation.ProjectNumber;
            receiptAllocationViewModel.CommodityID = receiptAllocation.CommodityID;
            receiptAllocationViewModel.SINumber = receiptAllocation.SINumber;
            receiptAllocationViewModel.QuantityInMT = receiptAllocation.QuantityInMT;
            if (receiptAllocation.QuantityInUnit == null)
                receiptAllocationViewModel.QuantityInUnit = 0;
            else
                receiptAllocationViewModel.QuantityInUnit = receiptAllocation.QuantityInUnit.Value;

            receiptAllocationViewModel.HubID = receiptAllocation.HubID;
            receiptAllocationViewModel.ETA = receiptAllocation.ETA;
            receiptAllocationViewModel.DonorID = receiptAllocation.DonorID;
            receiptAllocationViewModel.GiftCertificateDetailID = receiptAllocation.GiftCertificateDetailID;
            receiptAllocationViewModel.ReceiptAllocationID = receiptAllocation.ReceiptAllocationID;
            receiptAllocationViewModel.IsCommited = receiptAllocation.IsCommited;
            receiptAllocationViewModel.SourceHubID = receiptAllocation.SourceHubID;
            receiptAllocationViewModel.PurchaseOrder = receiptAllocation.PurchaseOrder;
            receiptAllocationViewModel.SupplierName = receiptAllocation.SupplierName;
            receiptAllocationViewModel.OtherDocumentationRef = receiptAllocation.OtherDocumentationRef;
            receiptAllocationViewModel.Remark = receiptAllocation.Remark;
            receiptAllocationViewModel.Receives = receiptAllocation.Receives.ToList();
            receiptAllocationViewModel.CommodityTypeID = receiptAllocation.Commodity.CommodityTypeID;
            
            receiptAllocationViewModel.ProgramID = receiptAllocation.ProgramID;
            receiptAllocationViewModel.CommoditySourceID = receiptAllocation.CommoditySourceID;
            return receiptAllocationViewModel;

        }
        // [Required(ErrorMessage = "Partition is required")]
        public Int32? PartitionId { get; set; }

        //   [Required(ErrorMessage = "Receipt Allocation is required")]
        public Guid? ReceiptAllocationID { get; set; }

        //    [Required(ErrorMessage = "Transaction Group is required")]
        // [Required(ErrorMessage = "Is Commited is required")]
        public Boolean IsCommited { get; set; }

        [Required(ErrorMessage = "ETA is required")]
        [DataType(DataType.DateTime)]
        public DateTime ETA { get; set; }

        [Required(ErrorMessage = "Project Number is required")]
        [StringLength(50)]
        public String ProjectNumber { get; set; }

        //public Int32 GiftCertificateID { get; set; }

        [Required(ErrorMessage = "Commodity is required")]
        public Int32 CommodityID { get; set; }

        [StringLength(50)]
        [Required]
        //[Remote("SIMustBeInGift", "ReceiptAllocation", AdditionalFields = "CommoditySourceID", ErrorMessage = "The SInumber not Found in Gift Certificate")]
        [Remote("SINotUnique", "ReceiptAllocation", AdditionalFields = "CommoditySourceID", ErrorMessage = "The SInumber given is Not valid for this Commodity Source")]
        public String SINumber { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        [RegularExpression("[0-9.]*", ErrorMessage = "Only numbers allowed")]
        [Range(0, 999999999.99)]
        [UIHint("PreferedWeightMeasurment")]
        public Decimal QuantityInMT { get; set; }
        public string QuantityInMTFormatted { get; set; }

        [Required(ErrorMessage = "Quantity In Unit is required")]
        [Range(0, 999999999.99)]
        public Decimal QuantityInUnit { get; set; }
        public string QuantityInUnitFormatted { get; set; }

        [Required(ErrorMessage = "Hub is required")]
        public Int32 HubID { get; set; }

        [Required(ErrorMessage = "Donor is required")]
        public Int32? DonorID { get; set; }

        public Int32? GiftCertificateDetailID { get; set; }

        [Required(ErrorMessage = "Program is required")]
        public Int32 ProgramID { get; set; }

        
        [Required(ErrorMessage = "Commodity Source is required")]
        //[Remote("SINotUnique", "ReceiptAllocation", AdditionalFields = "SINumber", ErrorMessage = "The SInumber given is Not valid for this Commodity Source")]
        public Int32 CommoditySourceID { get; set; }

        [StringLength(50)]
        [Required]
        public String PurchaseOrder { get; set; }

        [StringLength(50)]
        [Required]
        public String SupplierName { get; set; }

        [Required]
        [Remote("SelfReference", "ReceiptAllocation", AdditionalFields = "HubID", ErrorMessage = "Hub can not be the same as Destination")]
        public Int32? SourceHubID { get; set; }

        [StringLength(50)]
        public String OtherDocumentationRef { get; set; }

        [StringLength(5000)]
        public String Remark { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, validationContext, validationResults, false);
            return validationResults;
        }

        [Required]
        public int CommodityTypeID { get; set; }

        private string grn ;
        public string CommodityTypeText { get; set; }
        public string CommodityText { get; set; }
        public string GRN{
            get
            {
                
                grn = ReceiptAllocationID.ToString();
                //var d = ReceiveDetailViewModelDto(x => x.ReceiptAllocationID == ReceiptAllocationID) 
                //var list = new List<Receive>().FindAll(x => x.ReceiptAllocationID == ReceiptAllocationID);

                
                
                 //var list  = ReceivesList.FindAll((x => x.ReceiptAllocationID == ReceiptAllocationID));
                return grn;
            }
            
            set
            {
                
                for (int i = 0; i < Receives.ToArray().Length; i++)
                {
                    // grn + Receives.ToArray()[i].GRN;

                }
                
            }
        }
        //public string CommodityTypeText
        //{
        //    get { return _Repository.CommodityType.FindById(CommodityTypeID).Name; }
        //}

        //public string CommodityText
        //{
        //    get { return _Repository.Commodity.FindById(CommodityID).Name; }
        //}
    }
}