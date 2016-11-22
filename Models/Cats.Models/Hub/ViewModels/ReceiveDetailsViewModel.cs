using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cats.Models.Hubs.ViewModels.Common;

namespace Cats.Models.Hubs.ViewModels
{
    
    public class ReceiveDetailsViewModel
    {
        public ReceiveDetailsViewModel()
        {
            SentQuantityInUnit = 0;
            SentQuantityInMt = 0;
            ReceivedQuantityInUnit = 0;
            ReceivedQuantityInMt = 0;
            
        }


        public Guid? ReceiveDetailsId { get; set; }
        public string ReceiveDetailsIdString { get; set; }

        [Display(Name = "SI Number")]
        public int? SiNumber { get; set; }

        [Display(Name = "Commodity")]
     
        public int CommodityId { get; set; }
         [Display(Name = "Commodity Name")]
        public string CommodityName { get; set; }

        [Display(Name = "Sub Commodity")]
        public int CommodityChildID  { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "required")]
        [Display(Name = "Unit")]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "Sent quantity required")]
        [Range(1, 9999999.9)]
        [Display(Name = "Sent Quantity (Unit)")]
        public decimal SentQuantityInUnit { get; set; }

        [Required(ErrorMessage = "Recieved quantity is required")]
        [Range(1, 9999999.9)]
        [Display(Name = "Received Quantity (Unit)")]
        public decimal ReceivedQuantityInUnit { get; set; }

        [Required(ErrorMessage = "required")]
        [Range(0.1, 999999.99)]
        [Display(Name = "Received Quantity (MT)")]
        public decimal ReceivedQuantityInMt { get; set; }

        [Required(ErrorMessage = "required")]
        [Range(0.1, 999999.99)]
        [Display(Name = "Sent Quantity (MT)")]
        public decimal SentQuantityInMt { get; set; }

        public static List<ReceiveDetailsViewModel> GenerateReceiveDetailModels(ICollection<ReceiveDetail> entityCollection)
        {
            var details = new List<ReceiveDetailsViewModel>();
            int count = 0;
            foreach (var receiveDetail in entityCollection)
            {
                //count++;
                //var receiveDetailx = GenerateReceiveDetailModel(receiveDetail);
                //receiveDetailx.ReceiveDetailCounter = count;
                //details.Add(receiveDetailx);
            }
            return details;
        }


        public static ReceiveDetailViewModel GenerateReceiveDetailModel(ReceiveDetail ReceiveDetailModel)
        {
            var model = new ReceiveDetailViewModel();
            model.ReceiveDetailID = ReceiveDetailModel.ReceiveDetailID;
            model.UnitID = ReceiveDetailModel.UnitID;
            model.Description = ReceiveDetailModel.Description;
            model.ReceivedQuantityInMT = ReceiveDetailModel.QuantityInMT;
            model.ReceivedQuantityInUnit = ReceiveDetailModel.QuantityInUnit;
            model.CommodityGradeID = ReceiveDetailModel.CommodityGradeID;
            model.CommodityID = ReceiveDetailModel.CommodityID;
            model.CommodityChildID = ReceiveDetailModel.CommodityChildID ?? 0;
            model.SentQuantityInMT = ReceiveDetailModel.SentQuantityInMT;
            model.SentQuantityInUnit = ReceiveDetailModel.SentQuantityInUnit;
            model.ReceiveID = ReceiveDetailModel.ReceiveID;
            return model;
        }


    }

    public class ShippingInstructionModel
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
}
