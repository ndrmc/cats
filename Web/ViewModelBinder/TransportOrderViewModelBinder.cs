using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Areas.Procurement.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.ViewModels;

namespace Cats.ViewModelBinder
{
    public class TransportOrderViewModelBinder
    {
        public static List<TransportOrderViewModel> BindListTransportOrderViewModel(List<TransportOrder> transportOrders, string datePref, List<WorkflowStatus> statuses)
        {
            return transportOrders.Select(transportOrder => BindTransportOrderViewModel(transportOrder, datePref, statuses)).ToList();
        }
        
        public static TransportOrderViewModel BindTransportOrderViewModel(TransportOrder transportOrder, string datePref,List<WorkflowStatus> statuses )
        {
            TransportOrderViewModel transportOrderViewModel = null;
            if (transportOrder != null)
            {
                transportOrderViewModel = new TransportOrderViewModel();
                transportOrderViewModel.BidDocumentNo = transportOrder.BidDocumentNo;
                transportOrderViewModel.OrderDate = transportOrder.OrderDate;
                transportOrderViewModel.OrderDateET =
                   transportOrder.OrderDate.ToCTSPreferedDateFormat(datePref);
                transportOrderViewModel.ContractNumber = transportOrder.ContractNumber;
                transportOrderViewModel.PerformanceBondReceiptNo = transportOrder.PerformanceBondReceiptNo;
                transportOrderViewModel.OrderExpiryDate = transportOrder.OrderExpiryDate;
                transportOrderViewModel.OrderExpiryDateET = transportOrder.OrderExpiryDate.ToCTSPreferedDateFormat(datePref);
                transportOrderViewModel.Transporter = transportOrder.Transporter.Name;
                transportOrderViewModel.RequestedDispatchDate = transportOrder.RequestedDispatchDate;
                transportOrderViewModel.RequestedDispatchDateET = transportOrder.RequestedDispatchDate.ToCTSPreferedDateFormat(datePref);
                transportOrderViewModel.TransporterID = transportOrder.TransporterID;
                transportOrderViewModel.TransportOrderNo = transportOrder.TransportOrderNo;
                transportOrderViewModel.TransportOrderID = transportOrder.TransportOrderID;
                transportOrderViewModel.StatusID = transportOrder.StatusID;
                transportOrderViewModel.StartDate = transportOrder.StartDate.ToCTSPreferedDateFormat(datePref);
                transportOrderViewModel.EndDate = transportOrder.EndDate.ToCTSPreferedDateFormat(datePref);
                transportOrderViewModel.Status = transportOrder.BusinessProcess.CurrentState.BaseStateTemplate.Name;//transportOrder.StatusID.HasValue?statuses.Find(t => t.StatusID == transportOrder.StatusID.Value).Description:string.Empty;
                transportOrderViewModel.InitialStateFlowTemplates = BindFlowTemplateViewModel(transportOrder.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates).ToList();
                //requisition.IsDraft = reliefRequisition.BusinessProcess.CurrentState.BaseStateTemplate.Name == "Draft";
                transportOrderViewModel.BusinessProcessID = transportOrder.BusinessProcessID;
                transportOrderViewModel.RejectStateID =
                  transportOrder.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.Where(
                      t => t.Name == "Reject").Select(t => t.FinalStateID).FirstOrDefault();
                transportOrderViewModel.SignStateID =
                     transportOrder.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.Where(
                         t => t.Name == "Sign").Select(t => t.FinalStateID).FirstOrDefault();
                transportOrderViewModel.ApproveStateID =
                     transportOrder.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates.Where(
                         t => t.Name == "Approve").Select(t => t.FinalStateID).FirstOrDefault();

            }
            return transportOrderViewModel;
        }
        public static IEnumerable<FlowTemplateViewModel> BindFlowTemplateViewModel(IEnumerable<FlowTemplate> flowTemplates)
        {
            var flowTemplateViewModels = new List<FlowTemplateViewModel>();

            foreach (var flowTemplate in flowTemplates)
            {
                var flowTemplateViewModel = new FlowTemplateViewModel();
                flowTemplateViewModel.Name = flowTemplate.Name;
                flowTemplateViewModel.FinalState = flowTemplate.FinalState.Name;
                flowTemplateViewModel.FinalStateID = flowTemplate.FinalStateID;
                flowTemplateViewModel.FlowTemplateID = flowTemplate.FlowTemplateID;
                flowTemplateViewModel.InitialState = flowTemplate.InitialState.Name;
                flowTemplateViewModel.InitialStateID = flowTemplate.InitialStateID;
                flowTemplateViewModel.ParentProcessTemplate = flowTemplate.ParentProcessTemplate.Name;
                flowTemplateViewModel.ParentProcessTemplateID = flowTemplate.ParentProcessTemplateID;
                flowTemplateViewModels.Add(flowTemplateViewModel);
            }
            return flowTemplateViewModels;
        }
    }
}