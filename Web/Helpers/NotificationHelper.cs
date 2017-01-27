using System;
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
    public static class NotificationHelper
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

        public static HtmlString GetActiveNotifications(this HtmlHelper helper, bool isOldTheme = true)
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



                var str = "<ul class='dropdown-menu-list scroller'>";


                int max = 0;

                if (totallUnread.Count < 1)
                    return MvcHtmlString.Create("");
                max = totallUnread.Count > 5 ? 5 : totallUnread.Count;

                for (int i = 0; i < max; i++)
                {
                    str = str + "<li>";
                    // str = str + "<a href=" + totallUnread[i].Url + ">";
                    str = str + "<a style='padding: 10px 5px' href=" + GetRelativeURL(totallUnread[i].Url) + ">";
                    str = str + totallUnread[i].Text;
                    str = str + "</li>";
                    str = str + "</a>";
                }

                str = str + "</ul>";
                if (totallUnread.Count > 5 && isOldTheme)
                {
                    var webAppDir = (HttpContext.Current.Request.ApplicationPath == "/") ? "" : HttpContext.Current.Request.ApplicationPath;
                    str += "<a href='" + webAppDir + "/Home/GetUnreadNotificationDetail'> View More </a>";
                }

                return MvcHtmlString.Create(str);
            }
            catch (Exception)
            {
                return MvcHtmlString.Create("");
            }
        }

        public static MvcHtmlString GetMoreNotificationLink(this HtmlHelper helper, int notificationCount)
        {
            string webAppDir = "";
            if (notificationCount > 5)
            {
                webAppDir = (HttpContext.Current.Request.ApplicationPath == "/") ? "" : HttpContext.Current.Request.ApplicationPath;
                webAppDir = "<a href='" + webAppDir + "/Home/GetUnreadNotificationDetail'> View More </a>";

            }
            return MvcHtmlString.Create(webAppDir); ;
        }
        public static string GetRelativeURL(string AbsURL)
        {
            string[] CaseTeams =
             {
                "LOgistics",
                "EARLYWARNING",
                "PSNP",
                "PROCUREMENT",
                "FINANCE",
                "HUB",
                "REGIONAL"

            };
            var extractedUrl = "";
            foreach (var caseTeam in CaseTeams)
            {
                if (AbsURL.IndexOf(caseTeam, 0, StringComparison.CurrentCultureIgnoreCase) != -1)
                {
                    extractedUrl = AbsURL.Substring(AbsURL.IndexOf(caseTeam, StringComparison.OrdinalIgnoreCase));
                    break;
                }
            }
            // var extractedUrl = AbsURL.Substring(AbsURL.Any(CaseTeams.Contains(), StringComparison.OrdinalIgnoreCase) + 1);
            return GetCurrentApplicationPath() + "/" + extractedUrl;
        }


        private static string GetCurrentApplicationPath()
        {
            return HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath;
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