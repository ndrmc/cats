﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cats.Models.Constant
{

    public enum ActionType
    {
        [Description("Submit to Finance")]
        Finance=1,

       [Description("Approve Payment")]
        Approve=2
    }

    public enum WORKFLOW
    {
        REGIONAL_REQUEST = 1,
        RELIEF_REQUISITION = 2,
        TRANSPORT_REQUISITION = 3,
        TRANSPORT_ORDER = 4,
        HRD = 5,
        Plan = 7,
        BidWinner = 8,
        BID = 9,
        LocalPUrchase = 10,

    }
    public enum Action // conventional states -- this will help identify where a specific document is on
    {
       Edited,
       Deleted,
       Printed,
       Viewed
    }
    public enum Programs
    {
        Releif = 1,
        PSNP = 2,
        IDPS = 3
    }
    public enum RegionalRequestStatus
    {
        Draft = 1,
        Approved = 2,
        Closed = 3,
        FederalApproved = 4,
        Reject = 5
    }

    public enum ReliefRequisitionStatus
    {
        Draft = 1,
        Approved = 2,
        HubAssigned = 3,
        ProjectCodeAssigned = 4,
        TransportRequisitionCreated = 5,
        TransportOrderCreated = 6,
        Rejected = 7,
        SiPcAllocationApproved = 8

    }
    public enum TransportRequisitionStatus
    {
        Draft = 1,
        Approved = 2,
        Closed = 3
    }

    public enum TransportOrderStatus
    {
        Draft = 1,
        Approved = 2,
        Signed = 3,
        Closed = 4,
        Failed = 5
    }
    public enum HRDStatus
    {
        Draft = 1,
        Approved = 2,
        Published = 3
    }
    public enum PlanStatus
    {
        Draft = 1,
        Approved = 2,
        AssessmentCreated = 3,
        HRDCreated = 4,
        PSNPCreated = 5,
        Closed = 6
    }
    public enum WoredaHubLinkVersionStatus
    {
        Draft = 1,
        Approved = 2,
        Closed = 3
    }

    public enum BIDWINNER
    {
        Awarded = 1,
        Signed = 2,
        Left = 3,
        Failed = 4
    }
    public enum BidWinnerStatus
    {
        Draft = 1,
        Approved = 2,
        Signed = 3,
        Disqualified = 4,


    }
    public enum DistributionStatus
    {
        Draft = 1,
        Closed = 2
    }
    public enum BidStatus
    {
        Open = 1,
        Approved = 2,
        Cancelled = 3,
        Closed = 4,
        Active = 5

    }
    public enum LocalPurchaseStatus
    {
        Draft = 1,
        Approved = 2,
        Recived = 3,
        Closed = 4
    }
    public enum TransferStatus
    {
        Draft=1,
        Approved=2

    }

    public enum ChequeStatus
    {
        ChequeIssued = 1,
        ChequeApproved = 2,
        ChequeCollected = 3
    }

    public enum StateTemplate
    {
        PaymentRequested = 2013,
        RequestVerified = 2015,
        SubmittedToFinance = 2016,
        VerifiedByFinance = 2017,
        ChequeIssued = 2018,
        ChequeApproved = 2021,
        ChequeCollected = 2019
    }

    public enum PSNPWorkFlow
    {
        Draft = 1,
        SubmittedForApproval = 2,
        UnderRevision = 3,
        Approved = 4,
        Completed = 5

    }

    public enum AgreementVersionStatus
    {
        Draft = 1,
        Approved = 2
    }

    public enum AdminUnitType
    {
        Federal = 1,
        Region = 2,
        Zone = 3,
        Woreda = 4
    }
}
