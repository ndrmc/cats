﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Cats.Models;

namespace Cats.Services.Common
{
    public interface INotificationService
    {
        bool AddNotification(Notification notification);
        bool DeleteNotification(Notification notification);
        bool DeleteById(int id);
        bool EditNotification(Notification notification);
        Notification FindById(int id);
        List<Notification> GetAllNotification();
        List<Notification> FindBy(Expression<Func<Notification, bool>> predicate);
        bool AddNotificationForHubManagersFromTransportOrder(string destinationUrl, int transportOrderId, string transportOrderNo, List<int> hubId);
        bool AddNotificationForEarlyWaringFromRegions(string destinationURl, int requestId, int regionId);
        bool AddNotificationForProcurementFromLogistics(string destinationUrl, TransportRequisition transportRequisition);
        bool AddNotificationForLogistcisFromEarlyWaring(string destinationUrl, int requisitionID, int regionId, string requisitioNo);

        bool AddNotificationForProcurmentForGRNDiscripancy(string destinationURl, int transportOrderId,
                                                           string transportOrderNo);

        bool AddNotificationForRegionOnNeedAssessmentApproval(string destinationURL, Plan plan);
    }
}
