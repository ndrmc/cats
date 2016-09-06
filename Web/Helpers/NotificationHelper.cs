﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using Cats.Services.Security;
using Cats.Services.Common;
using Cats.Services.EarlyWarning;
using System.Text.RegularExpressions;
using System.Web.Routing;
namespace Cats.Helpers
{
    public static class  NotificationHelper
    {       
        public static int GetUnreadNotifications(this HtmlHelper helper)
        {
            try
            {
               var notificationService = (INotificationService)DependencyResolver.Current.GetService(typeof(INotificationService));
               
                var user = HttpContext.Current.User.Identity.Name;
                List<Cats.Models.Notification> totallUnread = null;
                var currentUser = UserAccountHelper.GetUser(user);
                var app = GetApplication(user);
               
                if (app == Models.Constant.Application.HUB)
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && n.Id ==currentUser.DefaultHub && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();   
                }
                else if(app== Models.Constant.Application.REGIONAL)
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && n.Id == currentUser.RegionID && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();   
                }
                else
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false  && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();   
                }

                return totallUnread.Count();
            }
            catch (Exception)
            {
                return 0;
            }
        }

        //public static int GetUnreadNotifications()
        //{
        //    try
        //    {
               
        //        var user = HttpContext.Current.User.Identity.Name;
        //        var app = GetApplication(user);

        //        var notificationService = (INotificationService)DependencyResolver.Current.GetService(typeof(INotificationService));
        //        var totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
        //        return totallUnread.Count();
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //    }
        //}

        public static HtmlString GetActiveNotifications(this HtmlHelper helper)
        {
            try
            {
               
              
                var user = HttpContext.Current.User.Identity.Name;
                var notificationService = (INotificationService)DependencyResolver.Current.GetService(typeof(INotificationService));

                List<Cats.Models.Notification> totallUnread = null;
                var currentUser = UserAccountHelper.GetUser(user);
                var app = GetApplication(user);

                if (app == Models.Constant.Application.HUB)
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && n.Id == currentUser.DefaultHub && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
                }
                else if (app == Models.Constant.Application.REGIONAL)
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && n.Id == currentUser.RegionID && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
                }
                else
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
                }



                var str = "<ul>";
               
               
                int max = 0;

                if (totallUnread.Count < 1)
                    return MvcHtmlString.Create("");
                max = totallUnread.Count > 5 ? 5 : totallUnread.Count;

                for (int i = 0; i < max;i ++ )
                {
                    str = str + "<li>";
                    // str = str + "<a href=" + totallUnread[i].Url + ">";
                    str = str + "<a href=" + GetRelativeURL(totallUnread[i].Url) + ">"; 
                    str = str + totallUnread[i].Text;
                    str = str + "</li>";
                    str = str + "</a>";
                }
                    
                str = str + "</ul>";
                if (totallUnread.Count > 5)
                {
                    string local = System.Web.HttpContext.Current.Request.Url.ToString();
                    if (local.Contains("/trunk"))
                        str = str + "<a href=/trunk/Home/GetUnreadNotificationDetail>" + "More...</a>";
                    else
                     str = str + "<a href=/Home/GetUnreadNotificationDetail>" + "More...</a>";
                    
                }
               
                return MvcHtmlString.Create(str);
            }
            catch (Exception)
            {
                return MvcHtmlString.Create("");
            }
        }

        public static string   GetRelativeURL(string AbsURL)
        {
           var extractedUrl=  AbsURL.Substring(AbsURL.IndexOf("//", StringComparison.Ordinal) + 1);
            extractedUrl= extractedUrl.Substring(extractedUrl.IndexOf("//", StringComparison.Ordinal) + 1);
            string local = System.Web.HttpContext.Current.Request.Url.ToString();
            if (local.Contains( "/trunk"))
                extractedUrl = "/trunk" + extractedUrl;
            return extractedUrl;
        }

        public static HtmlString GetActiveNotifications()
        {
            try
            {
               

                var user = HttpContext.Current.User.Identity.Name;
                var notificationService = (INotificationService)DependencyResolver.Current.GetService(typeof(INotificationService));
                List<Cats.Models.Notification> totallUnread = null;
                var currentUser = UserAccountHelper.GetUser(user);
                var app = GetApplication(user);

                if (app == Models.Constant.Application.HUB)
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && n.Id == currentUser.DefaultHub && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
                }
                else if (app == Models.Constant.Application.REGIONAL)
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && n.Id == currentUser.RegionID && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
                }
                else
                {
                    totallUnread = notificationService.GetAllNotification().Where(n => n.IsRead == false && app.Contains(n.Application)).OrderByDescending(n => n.NotificationId).ToList();
                }




                var str = "<ul>";
              
              
                int max = 0;

                if (totallUnread.Count < 1)
                    return MvcHtmlString.Create("");
                max = totallUnread.Count > 5 ? 5 : totallUnread.Count;

                for (int i = 0; i < max; i++)
                {
                    str = str + "<li>";
                    str = str + "<a href=" + GetRelativeURL(totallUnread[i].Url) + ">";
                    str = str + totallUnread[i].Text;
                    str = str + "</li>";
                    str = str + "</a>";
                }

                str = str + "</ul>";
                if (totallUnread.Count > 5)
                {
                    str = str + "<a href=/Home/GetUnreadNotificationDetail>" + "More...</a>";
                }

                return MvcHtmlString.Create(str);
            }
            catch (Exception)
            {
                return MvcHtmlString.Create("");
            }
        }


        public static void MakeNotificationRead(int recordId)
        {
            try
            {
                var notificationService = (INotificationService)DependencyResolver.Current.GetService(typeof(INotificationService));
                //var reliefRequisitionService =
                //    (IReliefRequisitionService)DependencyResolver.Current.GetService(typeof(IReliefRequisitionService));

                var notification = notificationService.FindBy(i => i.RecordId == recordId).Single();
                notification.IsRead = true;
                notificationService.EditNotification(notification); 
            }
            catch (Exception)
            {
                
               
            }
           
        }


        private static string GetApplication(string user)
        {

            var currentUser = UserAccountHelper.GetUser(user);
            var userID = currentUser.UserProfileID;
            if (currentUser.IsAdmin)
            {
                return Cats.Models.Constant.Application.EARLY_WARNING;
            }
            if (currentUser.DefaultHub != null)
            {
               
                    return Cats.Models.Constant.Application.HUB;
                   
            }
            if (currentUser.RegionalUser)
            {
                return Cats.Models.Constant.Application.REGIONAL;
            }
            switch (currentUser.CaseTeam)
            {
                case 1://EarlyWarning
                    return Cats.Models.Constant.Application.EARLY_WARNING;
                    break;
                case 2://PSNP
                    return Cats.Models.Constant.Application.PSNP;
                    break;
                case 3://Logistics
                    return Cats.Models.Constant.Application.LOGISTICS;
                    break;
                case 4://Procurement
                    return Cats.Models.Constant.Application.PROCUREMENT;
                    break;
                case 5://Finance
                    return Cats.Models.Constant.Application.FINANCE;
                    break;
                default:
                    return "";

            }

        }

    }
}