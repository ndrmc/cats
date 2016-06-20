﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Areas.Logistics.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.Security;
using Cats.Models.ViewModels;

namespace Cats.ViewModelBinder
{
    public class TransportRequisitionViewModelBinder
    {
        public static List<TransportRequisitionViewModel> BindListTransportRequisitonViewModel(List<TransportRequisition> transportRequisitions, List<WorkflowStatus> statuses, string datePrefrence, List<UserInfo> users)
        {
            return transportRequisitions.Select(t => BindTransportRequisitionViewModel(t, statuses, datePrefrence,users)).ToList();
        }
        public static TransportRequisitionViewModel BindTransportRequisitionViewModel(TransportRequisition transportRequisition,List<WorkflowStatus> statuses,string datePrefrence,List<UserInfo> users  )
        {
            TransportRequisitionViewModel transportRequisitionViewModel = null;
            if (transportRequisition != null)
            {
                transportRequisitionViewModel = new TransportRequisitionViewModel();
                transportRequisitionViewModel.BusinessProcessID = transportRequisition.BusinessProcessID;
                transportRequisitionViewModel.CertifiedBy =users.First(t=>t.UserProfileID==transportRequisition.CertifiedBy).FullName;
                transportRequisitionViewModel.CertifiedDate = transportRequisition.CertifiedDate;
                transportRequisitionViewModel.DateCertified = transportRequisition.CertifiedDate.ToCTSPreferedDateFormat(datePrefrence);
                //EthiopianDate.GregorianToEthiopian(transportRequisition.CertifiedDate);
                transportRequisitionViewModel.Remark = transportRequisition.Remark;
                transportRequisitionViewModel.RequestedBy = users.First(t => t.UserProfileID == transportRequisition.RequestedBy).FullName;
                transportRequisitionViewModel.RequestedDate = transportRequisition.RequestedDate;
                transportRequisitionViewModel.DateRequested = transportRequisition.RequestedDate.ToCTSPreferedDateFormat(datePrefrence);
                //EthiopianDate.GregorianToEthiopian( transportRequisition.RequestedDate);
                transportRequisitionViewModel.Status = transportRequisition.BusinessProcess.CurrentState.BaseStateTemplate.Name;
                transportRequisitionViewModel.StatusID = transportRequisition.BusinessProcess.CurrentState.BaseStateTemplate.StateTemplateID;
                transportRequisitionViewModel.TransportRequisitionID = transportRequisition.TransportRequisitionID;
                transportRequisitionViewModel.TransportRequisitionNo = transportRequisition.TransportRequisitionNo;
                transportRequisitionViewModel.Program = transportRequisition.Program.Name;
                transportRequisitionViewModel.Month =
                    RequestHelper.MonthName(
                        transportRequisition.TransportRequisitionDetails.FirstOrDefault().ReliefRequisition.Month);
                transportRequisitionViewModel.Round =transportRequisition.TransportRequisitionDetails.FirstOrDefault().ReliefRequisition.Round;
                transportRequisitionViewModel.Date = DateTime.Now.ToCTSPreferedDateFormat(datePrefrence);
            }
            return transportRequisitionViewModel;
        }

        public static TransportRequisitionDetailViewModel BindTransportRequisitionDetailViewModel(RequisitionToDispatch requisitionToDispatch)
        {
            TransportRequisitionDetailViewModel transportRequisitionDetailViewModel = null;
            if (requisitionToDispatch != null)
            {
                transportRequisitionDetailViewModel = new TransportRequisitionDetailViewModel();
                transportRequisitionDetailViewModel.CommodityID = requisitionToDispatch.CommodityID;
                transportRequisitionDetailViewModel.CommodityName = requisitionToDispatch.CommodityName;
                transportRequisitionDetailViewModel.HubID = requisitionToDispatch.HubID;
                //transportRequisitionDetailViewModel.OrignWarehouse = requisitionToDispatch.Store != ""
                //                                                         ? requisitionToDispatch.OrignWarehouse + "(" +
                //                                                           requisitionToDispatch.Store + ")"
                //                                                         : requisitionToDispatch.OrignWarehouse;
                transportRequisitionDetailViewModel.OrignWarehouse = requisitionToDispatch.OrignWarehouse;
                transportRequisitionDetailViewModel.QuanityInQtl = requisitionToDispatch.QuanityInQtl.ToPreferedWeightUnit();
                transportRequisitionDetailViewModel.Region = requisitionToDispatch.RegionName;
                transportRequisitionDetailViewModel.Zone = requisitionToDispatch.Zone;
                transportRequisitionDetailViewModel.RequisitionNo = requisitionToDispatch.RequisitionNo;
                transportRequisitionDetailViewModel.RequisitionID = requisitionToDispatch.RequisitionID;
                transportRequisitionDetailViewModel.ProgramID = requisitionToDispatch.ProgramID;
                transportRequisitionDetailViewModel.Program = requisitionToDispatch.Program;
                //transportRequisitionDetailViewModel.Store = requisitionToDispatch.Store;
            }
            return transportRequisitionDetailViewModel;
        }

        public static List<TransportRequisitionDetailViewModel> BindListTransportRequisitionDetailViewModel(List<RequisitionToDispatch> transportRequisitionDetails)
        {
            return transportRequisitionDetails.Select(BindTransportRequisitionDetailViewModel).ToList();
        } 
    }
}