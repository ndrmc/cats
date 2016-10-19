﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Cats.Areas.EarlyWarning.Models;
using Cats.Helpers;
using Cats.Models;
using Cats.Models.Constant;
using Cats.Models.ViewModels;

namespace Cats.ViewModelBinder
{
    public class RequestViewModelBinder
    {
        public static IEnumerable<RegionalRequestViewModel> BindRegionalRequestListViewModel(

          IEnumerable<RegionalRequest> requests, List<WorkflowStatus> statuses, string userPreference)

        {
            var requestsViewModel = new List<RegionalRequestViewModel>();
            foreach (var regionalRequest in requests)
            {
                requestsViewModel.Add(BindRegionalRequestViewModel(regionalRequest, userPreference));
            }

            return requestsViewModel;
        }

        public static RegionalRequestViewModel BindRegionalRequestViewModel(RegionalRequest regionalRequest, string userPrefrence)
        {
            var regionalRequestViewModel = new RegionalRequestViewModel();

            if (regionalRequest == null)
            {
                return regionalRequestViewModel;
            }
            regionalRequestViewModel.ProgramId = regionalRequest.ProgramId;
            regionalRequestViewModel.Program = regionalRequest.Program.Name;
            regionalRequestViewModel.Region = regionalRequest.AdminUnit.Name;
            regionalRequestViewModel.ReferenceNumber = regionalRequest.ReferenceNumber;
            regionalRequestViewModel.RegionID = regionalRequest.RegionID;
            regionalRequestViewModel.RegionalRequestID = regionalRequest.RegionalRequestID;
            regionalRequestViewModel.Remark = regionalRequest.Remark;
            regionalRequestViewModel.RequestDate = regionalRequest.RequistionDate.ToCTSPreferedDateFormat(userPrefrence);
            regionalRequestViewModel.Round = regionalRequest.Round;

            //RequestDateEt = EthiopianDate.GregorianToEthiopian(regionalRequest.RequistionDate);
            regionalRequestViewModel.MonthName = RequestHelper.GetMonthList().Find(t => t.Id == regionalRequest.Month).Name;
            regionalRequest.Month = regionalRequest.Month;
            //regionalRequestViewModel.Status = statuses.Find(t => t.WorkflowID == (int)WORKFLOW.REGIONAL_REQUEST && t.StatusID == regionalRequest.Status).Description;
            // regionalRequestViewModel. RequistionDate = regionalRequest.RequistionDate;
            regionalRequestViewModel.StatusID = regionalRequest.Status;
            if (regionalRequest.Ration != null) regionalRequestViewModel.Ration = regionalRequest.Ration.RefrenceNumber;
            regionalRequestViewModel.RationID = regionalRequest.RationID;
            regionalRequestViewModel.Year = regionalRequest.Year;
            //regionalRequestViewModel.IsApprovable = Request 
            regionalRequestViewModel.PlanId = regionalRequest.PlanID;

            if (regionalRequest.UserProfile != null)
                regionalRequestViewModel.RequestedBy = regionalRequest.UserProfile.FirstName + " " +
                                                       regionalRequest.UserProfile.LastName;
            if (regionalRequest.UserProfile1 != null && regionalRequest.Status == (int)Cats.Models.Constant.RegionalRequestStatus.Approved)
                regionalRequestViewModel.ApprovedBy = regionalRequest.UserProfile1.FirstName + " " +
                                                      regionalRequest.UserProfile1.LastName;
            regionalRequestViewModel.BusinessProcessID = regionalRequest.BusinessProcessID;
            if (regionalRequest.BusinessProcess != null)
            {
                regionalRequestViewModel.Status = regionalRequest.BusinessProcess.CurrentState.BaseStateTemplate.Name;
                regionalRequestViewModel.CurrentStateNo =
                    BindFlowTemplateViewModel(
                        regionalRequest.BusinessProcess.CurrentState.BaseStateTemplate.InitialStateFlowTemplates)
                        .ToList();
                regionalRequestViewModel.StateId =
                    BindStateTemplateViewModel(regionalRequest.BusinessProcess.CurrentState.BaseStateTemplate.Name);
            }
            return regionalRequestViewModel;
        }

        public static int BindStateTemplateViewModel(string stateName)
        {
            if (stateName == "Draft")
                return 1;
            if (stateName == "Approved")
                return 2;
            if (stateName == "Closed")
                return 3;
            if (stateName == "FederalApproved")
                return 4;
            if (stateName == "Rejected")
                return 5;

            return 0;
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
        public static RegionalRequest BindRegionalRequest(RegionalRequestViewModel regionalRequestViewModel, RegionalRequest request = null)
        {
            if (request == null) request = new RegionalRequest();

            request.ProgramId = regionalRequestViewModel.ProgramId;
            request.ReferenceNumber = regionalRequestViewModel.ReferenceNumber;
            request.RegionID = regionalRequestViewModel.RegionID;
            request.RegionalRequestID = regionalRequestViewModel.RegionalRequestID;
            request.Remark = regionalRequestViewModel.Remark;
            request.Month = regionalRequestViewModel.Month;
            request.RequistionDate = regionalRequestViewModel.RequistionDate;
            request.Round = regionalRequestViewModel.Round;
            request.Status = regionalRequestViewModel.StatusID;
            request.Year = regionalRequestViewModel.Year;
            //  request.DonorID=
            return request;
        }

        public static RegionalRequest BindRegionalRequest(RegionalRequest origin, RegionalRequest request = null)
        {
            if (request == null) request = new RegionalRequest();

            request.ProgramId = origin.ProgramId;
            request.ReferenceNumber = origin.ReferenceNumber;
            request.RegionID = origin.RegionID;
            request.RegionalRequestID = origin.RegionalRequestID;
            request.Remark = origin.Remark;
            request.Month = origin.Month;
            request.RequistionDate = origin.RequistionDate;
            request.Status = origin.Status;
            request.Year = origin.Year;
            request.RationID = origin.RationID;
            request.Round = origin.Round;
            return request;
        }




        public static DataTable TransposeDataNew(List<PLANWithRegionalRequestViewModel> woredaRequestDetail, int programID, string preferedweight)
        {
            var dt = new DataTable("Transpose");

            var colZone = new DataColumn("Zone", typeof(string));
            colZone.ExtendedProperties["ID"] = -1;
            dt.Columns.Add(colZone);

            var colWoreda = new DataColumn("Woreda", typeof(string));
            colWoreda.ExtendedProperties["ID"] = -1;
            dt.Columns.Add(colWoreda);

            var colNoBeneficiary = new DataColumn("Beneficiaries", typeof(int));
            colNoBeneficiary.ExtendedProperties["ID"] = -1;
            dt.Columns.Add(colNoBeneficiary);

            var colNoPlannedBeneficiary = new DataColumn("PlannedBeneficiaries", typeof(int));
            var colDifference = new DataColumn("Difference", typeof(int));

            if (programID != 3)
            {
                colNoPlannedBeneficiary.ExtendedProperties["ID"] = -1;
                dt.Columns.Add(colNoPlannedBeneficiary);

                colDifference.ExtendedProperties["ID"] = -1;
                dt.Columns.Add(colDifference);
            }

            //var requestdetail = requestDetails.FirstOrDefault();

            if (woredaRequestDetail == null || woredaRequestDetail.Count == 0) return dt;
            var requestdetail = woredaRequestDetail.FirstOrDefault().RegionalRequestDetails.FirstOrDefault();

            if (requestdetail != null)
            {
                foreach (var ds in requestdetail.RequestDetailCommodities)
                {
                    var col = new DataColumn(ds.Commodity.Name.Trim() + " in " + preferedweight.ToUpper().Trim(), typeof(decimal));
                    col.ExtendedProperties.Add("ID", ds.CommodityID);
                    dt.Columns.Add(col);
                }
            }

            var currentZone = "";
            var subtotalBeneficiary = 0;
            var subtotalPlannedBeneficiary = 0;
            //woredaRequestDetail = woredaRequestDetail.Sort(t => t.zone);
            foreach (var singleWoreda in woredaRequestDetail)
            {
                if (currentZone == "")
                {
                    currentZone = singleWoreda.zone;
                    subtotalBeneficiary = singleWoreda.RequestedBeneficiaryNo;
                    subtotalPlannedBeneficiary = singleWoreda.PlannedBeneficaryNo;
                }
                else if (currentZone == singleWoreda.zone)
                {
                    subtotalBeneficiary += singleWoreda.RequestedBeneficiaryNo;
                    subtotalPlannedBeneficiary += singleWoreda.PlannedBeneficaryNo;
                }
                else if (currentZone != singleWoreda.zone)
                {
                    var drSubtotal = dt.NewRow();

                    drSubtotal[colZone] = currentZone;
                    drSubtotal[colWoreda] = "Subtotal";
                    drSubtotal[colNoBeneficiary] = subtotalBeneficiary;
                    drSubtotal[colNoPlannedBeneficiary] = subtotalPlannedBeneficiary;
                    drSubtotal[colDifference] = subtotalPlannedBeneficiary - subtotalBeneficiary;
                    dt.Rows.Add(drSubtotal);

                    currentZone = singleWoreda.zone;
                    subtotalBeneficiary = singleWoreda.RequestedBeneficiaryNo;
                    subtotalPlannedBeneficiary = singleWoreda.PlannedBeneficaryNo;
                }

                var dr = dt.NewRow();

                dr[colZone] = singleWoreda.zone;
                dr[colWoreda] = singleWoreda.Woreda;

                dr[colNoBeneficiary] = singleWoreda.RequestedBeneficiaryNo;
                if (programID != 3)
                {
                    dr[colNoPlannedBeneficiary] = singleWoreda.PlannedBeneficaryNo;
                    dr[colDifference] = singleWoreda.Difference;
                }
                var details = singleWoreda.RegionalRequestDetails;
                //var groupedByWoreda = (
                //                          from regionalRequestDetail in requestDetails
                //                          group regionalRequestDetail by regionalRequestDetail.Fdp.AdminUnit
                //                          into g
                //                          select g
                //                      );

                //decimal accumelate = 0;


                foreach (var requestDetailCommodity in requestdetail.RequestDetailCommodities)
                {
                    decimal amount = 0;

                    foreach (var requestDetail in details)
                    {
                        foreach (var commodity in requestDetail.RequestDetailCommodities)
                        {
                            if (commodity.CommodityID == requestDetailCommodity.CommodityID)
                            {
                                amount = amount + commodity.Amount;
                            }

                        }

                    }

                    var rDetatils = singleWoreda.RegionalRequestDetails;
                    //_requestDetailCommodityService.FindBy(requestDetailCommodity.CommodityID);

                    DataColumn col = null;
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (requestDetailCommodity.CommodityID.ToString() ==
                            column.ExtendedProperties["ID"].ToString())
                        {
                            col = column;
                            break;
                        }
                    }

                    if (col != null)
                    {
                        //_requestDetailCommodityService.FindBy(y=>y.CommodityID)
                        //dr[col.ColumnName] = requestDetailCommodity.Amount.ToPreferedWeightUnit();
                        dr[col.ColumnName] = amount.ToPreferedWeightUnit();
                    }
                }

                dt.Rows.Add(dr);
                if (woredaRequestDetail.Last().Equals(singleWoreda))
                {
                    var drSubtotal = dt.NewRow();

                    drSubtotal[colZone] = currentZone;
                    drSubtotal[colWoreda] = "Subtotal";
                    drSubtotal[colNoBeneficiary] = subtotalBeneficiary;
                    drSubtotal[colNoPlannedBeneficiary] = subtotalPlannedBeneficiary;
                    drSubtotal[colDifference] = subtotalPlannedBeneficiary - subtotalBeneficiary;
                    dt.Rows.Add(drSubtotal);
                }
            }

            return dt;
        }

        public static DataTable TransposeData(IEnumerable<RegionalRequestDetail> requestDetails)
        {
            var dt = new DataTable("Transpose");
            //var colRequstDetailID = new DataColumn("RequstDetailID", typeof(int));
            //colRequstDetailID.ExtendedProperties["ID"] = -1;
            //dt.Columns.Add(colRequstDetailID);

            var colZone = new DataColumn("Zone", typeof(string));
            colZone.ExtendedProperties["ID"] = -1;
            dt.Columns.Add(colZone);

            var colWoreda = new DataColumn("Woreda", typeof(string));
            colWoreda.ExtendedProperties["ID"] = -1;
            dt.Columns.Add(colWoreda);

            var colFDP = new DataColumn("FDP", typeof(string));
            colFDP.ExtendedProperties["ID"] = -1;
            dt.Columns.Add(colFDP);

            var colNoBeneficiary = new DataColumn("NoBeneficiary", typeof(int));
            colNoBeneficiary.ExtendedProperties["ID"] = -1;
            dt.Columns.Add(colNoBeneficiary);

            var requestdetail = requestDetails.FirstOrDefault();

            //requestdetail.

            if (requestdetail != null)
            {
                foreach (var ds in requestdetail.RequestDetailCommodities)
                {
                    var col = new DataColumn(ds.Commodity.Name.Trim(), typeof(decimal));
                    col.ExtendedProperties.Add("ID", ds.CommodityID);
                    dt.Columns.Add(col);
                }

                //int rowID = 0; 
                //bool addRow = false;
                //var rowGroups = (from item in mydata select item.MyClassID).Distinct().ToList();
                var groupedByWoreda = (
                                        from regionalRequestDetail in requestDetails
                                        group regionalRequestDetail by regionalRequestDetail.Fdp.AdminUnit
                                            into g
                                        select g
                                      );

                foreach (var requestDetail in requestDetails)
                {
                    var dr = dt.NewRow();
                    //dr[colRequstDetailID] = requestDetail.RegionalRequestDetailID;
                    dr[colNoBeneficiary] = requestDetail.Beneficiaries;
                    dr[colZone] = requestDetail.Fdp.AdminUnit.AdminUnit2.Name;
                    dr[colWoreda] = requestDetail.Fdp.AdminUnit.Name;
                    dr[colFDP] = requestDetail.Fdp.Name;

                    foreach (var requestDetailCommodity in requestDetail.RequestDetailCommodities)
                    {
                        DataColumn col = null;
                        foreach (DataColumn column in dt.Columns)
                        {
                            if (requestDetailCommodity.CommodityID.ToString() ==
                                column.ExtendedProperties["ID"].ToString())
                            {
                                col = column;
                                break;
                            }
                        }

                        if (col != null)
                        {

                            dr[col.ColumnName] = requestDetailCommodity.Amount.ToPreferedWeightUnit();
                        }
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }
}