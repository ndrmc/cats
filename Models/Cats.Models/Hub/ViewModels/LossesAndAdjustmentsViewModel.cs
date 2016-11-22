﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cats.Models.Hubs.ViewModels.Common;
using System.ComponentModel.DataAnnotations;

namespace Cats.Models.Hubs.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class LossesAndAdjustmentsViewModel
    {
        //[Required(ErrorMessage = "Commodity is Required")]
        [Display(Name = "Commodity")]
        public List<Commodity> Commodities { get; set; }

        //[Required(ErrorMessage = "Project Code")]
        [Display(Name = "Project Code")]
        public List<ProjectCodeViewModel> ProjectCodes { get; set; }

        //[Required(ErrorMessage = "Program is Required")]
        [Display(Name = "Program")]
        public List<ProgramViewModel> Programes { get; set; }

        //[Required(ErrorMessage = "SI Number is Required")]
        [Display(Name = "SI Number")]
        public List<ShippingInstructionViewModel> ShippingInstructions { get; set; }

        //[Required(ErrorMessage = "Store is Required")]
        [Display(Name = "Store")]
        public List<StoreViewModel> Stores { get; set; }

        //[Required(ErrorMessage = "Reason is Required")]
        [Display(Name = "Reason")]
        public List<AdjustmentReason> Reasons { get; set; }

        //[Required(ErrorMessage = "Unit is required")]
        [Display(Name = "Unit")]
        public List<Unit> Units { get; set; }

        //[Required(ErrorMessage = "Chose Loss or Adjustment")]
        [Display(Name = "Loss")]
        public bool IsLoss { get; set; }


        [Display(Name = "Is Adjustment")]
        public bool IsAdjustment { get; set; }

        [Required(ErrorMessage = "Commodity should be selected")]
        public int CommodityId { get; set; }

        //[Required(ErrorMessage = "Date is required")]
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public DateTime SelectedDate { get; set; }

        [Required(ErrorMessage = "Project Code should be selected")]
        public int ProjectCodeId { get; set; }

        public int ProgramId { get; set; }


        [Required(ErrorMessage = "Memo Number")]
        [Display(Name = "Memo Number")]
        public string MemoNumber { get; set; }

        [Required(ErrorMessage = "Shipping Instruction should be selected")]
        public int ShippingInstructionId { get; set; }

        [Required(ErrorMessage = "Store should be selected")]
        public int StoreId { get; set; }

        [Required(ErrorMessage = "Store can't be empty")]
        [Display(Name = "Store Man")]
        [UIHint("AmharicTextBox")]
        public string StoreMan { get; set; }

        [Required(ErrorMessage = "The reason type should be specified")]
        public int ReasonId { get; set; }

        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        [UIHint("AmharicTextArea")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The measurement unit should be specified")]
        public int UnitId { get; set; }

        [Display(Name = "Quantity In Unit")]
        [Required(ErrorMessage = "The unit quantity should be specified")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid quantity number")]
        public decimal QuantityInUint { get; set; }

        [Display(Name = "Quntity in Mt")]
        [Required(ErrorMessage = "The quantity in MT should be specified")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid quantity number")]
        public decimal QuantityInMt { get; set; }

        [UIHint("AmharicTextBox")]
        public string ApprovedBy { get; set; }

        ///Methods 
        public LossesAndAdjustmentsViewModel()
        {
        }

        public LossesAndAdjustmentsViewModel(List<Commodity> commodity,
                                             List<StoreViewModel> stores,
                                             List<AdjustmentReason> adjustmentReasonMinus,
                                             List<AdjustmentReason> adjustmentReasonPlus,
                                             List<Unit> units,
                                             List<ProgramViewModel> programs,
                                             UserProfile user, 
                                             int type)
        {
            if (type == 1)
            {
                this.Commodities = commodity;
                this.SelectedDate = DateTime.Now;
                this.ProjectCodes = new List<ProjectCodeViewModel>();
                //this.MemoNumber = 
                this.ShippingInstructions = new List<ShippingInstructionViewModel>();
                this.Stores = stores;
                //this.StoreMan = ;
                this.Reasons = adjustmentReasonMinus;
                //this.Description =
                this.Units = units;
                //this.QuantityInMt = 
                //this.QuantityInUint
                //this.ApprovedBy
                this.Programes = programs;
            }
            else
            {
                this.Commodities = commodity;
                this.SelectedDate = DateTime.Now;
                this.ProjectCodes = new List<ProjectCodeViewModel>();
                //this.MemoNumber = 
                this.ShippingInstructions = new List<ShippingInstructionViewModel>();
                this.Stores = stores;
                //this.StoreMan = ;
                this.Reasons = adjustmentReasonPlus;
                //this.Description =
                this.Units = units;
                //this.QuantityInMt = 
                //this.QuantityInUint
                //this.ApprovedBy
                this.Programes = programs;
            }
        }
    }
}
