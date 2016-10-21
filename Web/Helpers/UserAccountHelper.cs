using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Cats.Services.Security;
using Cats.Models.Security;
using System.Web.Security;
using Cats.Security;
using Cats.Services.EarlyWarning;
using NetSqlAzMan.Cache;
using NetSqlAzMan.Interfaces;

namespace Cats.Helpers
{
    public static class UserAccountHelper
    {
        public static string GetUserName(this HtmlHelper helper)
        {
            return GetUserName();
        }

        public static string GetUserName()
        {
            var userName = string.Empty;
            try
            {
                var user = (UserInfo)HttpContext.Current.Session["USER_INFO"];
                userName= user.FullName;
            }
            catch (Exception)
            {
                userName="Guest User";
            }
            return userName;
        }

        public static string UserLanguagePreference(this HtmlHelper helper)
        {
            var userLanguagePreference = string.Empty;
            try
            {
                var user = (UserIdentity)HttpContext.Current.User.Identity;
                userLanguagePreference= GetUser(user.Name).LanguageCode;
            }
            catch (Exception)
            {
                userLanguagePreference ="Guest User";
            }
            return userLanguagePreference;
        }
        public static UserInfo GetUser(string userName)
        {
            return GetUserInfo(userName);
        }

        private static void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public static UserInfo GetCurrentUser()
        {
            return GetUserInfo(HttpContext.Current.User.Identity.Name);
        }

        public static string GetCurrentUserRegion()
        {
            var currentUser = GetUserInfo(HttpContext.Current.User.Identity.Name);
            var regionName = "";
            if (currentUser.RegionalUser && currentUser.RegionID!=null)
            {
                var _adminUnitService = (IAdminUnitService)DependencyResolver.Current.GetService(typeof(IAdminUnitService));
                regionName = _adminUnitService.FindById((int) currentUser.RegionID).Name;
            }
            return regionName;
        }

        private static UserInfo GetUserInfo(string userName)
        {
            // Try returning session stored values if available
            if(null != HttpContext.Current.Session && null!=HttpContext.Current.Session["USER_INFO"])
            {
                return (UserInfo)HttpContext.Current.Session["USER_INFO"]; 
            }

            // Fetch a fresh copy of user information from the database
            var service = (IUserAccountService)DependencyResolver.Current.GetService(typeof(IUserAccountService));
            var user = service.GetUserInfo(userName);

            // Save user information to session state for latter usage
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session["USER_INFO"] = user;
                HttpContext.Current.Session["USER_PROFILE"] = service.GetUserDetail(userName);
            }

            return user;

            //// Initialize the user object with the incoming user name to avoid 'Use of uninitialized variable exception'
            //UserInfo user = new UserInfo { UserName = userName };

            //try
            //{
            //    // Check to see if we already have the user profile loaded in the session.
            //    if ( HttpContext.Current.Session.Keys.Count>0)
            //    {
            //        if (HttpContext.Current.Session["USER_INFO"]!=null)
            //        {
            //            user = (UserInfo)HttpContext.Current.Session["USER_INFO"];    
            //        }
            //        else
            //        {
            //            // Fetch a copy from the database if we don't have a session variable already loaded in memory
            //            var service = (IUserAccountService)DependencyResolver.Current.GetService(typeof(IUserAccountService));
            //            user = service.GetUserInfo(userName);
            //        }

            //        //to update the "USER_INFO"session as far as the user is engaged 
            //        //HttpContext.Current.Session["USER_INFO"] = user;
            //    }
            //    else
            //    {
            //        // Fetch a copy from the database if we don't have a session variable already loaded in memory
            //        if(HttpContext.Current.User.Identity.IsAuthenticated)
            //        {
            //            var service = (IUserAccountService)DependencyResolver.Current.GetService(typeof(IUserAccountService));
            //            user = service.GetUserInfo(userName);
            //            HttpContext.Current.Session["USER_INFO"] = user;
            //            HttpContext.Current.Session["USER_PROFILE"] = service.GetUserDetail(userName);
            //        }
            //    }
            //}

            //catch (Exception ex)
            //{
            //    //TODO: Log error here
            //    Logger.Log(ex);
            //    SignOut();
            //    return null;
            //}

            //return user;
        }

        public static string UserCalendarPreference(this HtmlHelper helper)
        {
            return UserCalendarPreference();
        }

        public static string UserCalendarPreference()
        {
            var preference = "EN";
            var user = GetUser(HttpContext.Current.User.Identity.Name);
            try
            {
                preference = user.DatePreference;
            }
            catch (Exception ex)
            {
                // TODO: Log exception hrere
            }
            return preference.ToUpper();
        }
        public static string UserUnitPreference(this HtmlHelper helper)
        {
            return UserUnitPreference();
        }

        public static string UserUnitPreference()
        {
            var preference = "MT";
            var user = GetUser(HttpContext.Current.User.Identity.Name);
            try
            {
                preference = user.PreferedWeightMeasurment;
            }
            catch (Exception ex)
            {
                // TODO: Log exception hrere
            }

            return preference.ToUpper();
        }

        public static UserPermissionCache GetUserPermissionCache(CatsGlobals.Applications application)
        {
            UserPermissionCache permissionsCache = null;
           
            switch (application)
            {
                case CatsGlobals.Applications.EarlyWarning:
                    permissionsCache = (UserPermissionCache)HttpContext.Current.Session[CatsGlobals.EARLY_WARNING_PERMISSIONS];
                    break;
                case CatsGlobals.Applications.PSNP:
                    permissionsCache = (UserPermissionCache)HttpContext.Current.Session[CatsGlobals.PSNP_PERMISSIONS];
                    break;
                case CatsGlobals.Applications.Logistics:
                    permissionsCache = (UserPermissionCache)HttpContext.Current.Session[CatsGlobals.LOGISTICS_PERMISSIONS];
                    break;
                case CatsGlobals.Applications.Procurement:
                    permissionsCache = (UserPermissionCache)HttpContext.Current.Session[CatsGlobals.PROCUREMENT_PERMISSIONS];
                    break;
                case CatsGlobals.Applications.Finance:
                    permissionsCache = (UserPermissionCache)HttpContext.Current.Session[CatsGlobals.FINANCE_PERMISSIONS];
                    break;
                case CatsGlobals.Applications.Hub:
                    permissionsCache = (UserPermissionCache)HttpContext.Current.Session[CatsGlobals.HUB_PERMISSIONS];
                    break;
                case CatsGlobals.Applications.Administration:
                    permissionsCache = (UserPermissionCache)HttpContext.Current.Session[CatsGlobals.ADMINISTRATION_PERMISSIONS];
                    break;
                case CatsGlobals.Applications.Region:
                    permissionsCache = (UserPermissionCache)HttpContext.Current.Session[CatsGlobals.REGION_PERMISSIONS];
                    break;

            }

            return permissionsCache;
        }

        public static bool EarlyWarningPsnpOperationCheck(EarlyWarningConstants.Operation operationEW, PsnpConstants.Operation
            operationPSNP)
        {
            //return MvcHtmlString.Create(@"<a data-buttontype=" + dataButtontype + "  class=" + ccsClass + " href=" + url + ">" + text + "</a>");

            var ewConstants = new EarlyWarningConstants();
            var ewCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.EarlyWarning);

            var psnpConstants = new PsnpConstants();
            var psnpCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.PSNP);
            
            if (ewCache.CheckAccess(ewConstants.ItemName(operationEW), DateTime.Now) == AuthorizationType.Allow
                || psnpCache.CheckAccess(psnpConstants.ItemName(operationPSNP), DateTime.Now) == AuthorizationType.Allow)
            {
                return true;
            }

            return false;
        }

        public static bool EarlyWarningPsnpOperationCheck(EarlyWarningConstants.Operation operationEW)
        {
            //return MvcHtmlString.Create(@"<a data-buttontype=" + dataButtontype + "  class=" + ccsClass + " href=" + url + ">" + text + "</a>");

            var ewConstants = new EarlyWarningConstants();
            var ewCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.EarlyWarning);

            // If cache is null then force the user to sign-in again
            if (null == ewCache )
            {
                Signout();
                return false;
            }

            if (ewCache.CheckAccess(ewConstants.ItemName(operationEW), DateTime.Now) == AuthorizationType.Allow)
            {
                return true;
            }
            return false;
        }

        public static bool PsnpOperationCheck(PsnpConstants.Operation operationPsnp)
        {
            //return MvcHtmlString.Create(@"<a data-buttontype=" + dataButtontype + "  class=" + ccsClass + " href=" + url + ">" + text + "</a>");

            var psnpConstants = new PsnpConstants();
            var psnpCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.PSNP);

            // If cache is null then force the user to sign-in again
            if (null == psnpCache)
            {
                Signout();
                return false;
            }
            try
            {
                if (psnpCache.CheckAccess(psnpConstants.ItemName(operationPsnp), DateTime.Now) ==
                    AuthorizationType.Allow)
                {
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }
            return false;
        }

        public static bool EarlyWarningRegionalOperationCheck(EarlyWarningConstants.Operation operationEW, RegionalConstants.Operation
            operationRegional)
        {
            //return MvcHtmlString.Create(@"<a data-buttontype=" + dataButtontype + "  class=" + ccsClass + " href=" + url + ">" + text + "</a>");

            var ewConstants = new EarlyWarningConstants();
            var ewCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.EarlyWarning);

            var regionalConstants = new RegionalConstants();
            var regionalCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.Region);
            
            if (ewCache.CheckAccess(ewConstants.ItemName(operationEW), DateTime.Now) == AuthorizationType.Allow
                || regionalCache.CheckAccess(regionalConstants.ItemName(operationRegional), DateTime.Now) == AuthorizationType.Allow)
            {
                return true;
            }

            return false;
        }

        public static bool RegionalOperationCheck(RegionalConstants.Operation operationRegional)
        {
            //return MvcHtmlString.Create(@"<a data-buttontype=" + dataButtontype + "  class=" + ccsClass + " href=" + url + ">" + text + "</a>");

            var regionalConstants = new RegionalConstants();
            var regionalCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.Region);

            // If cache is null then force the user to sign-in again
            if (null == regionalCache)
            {
                Signout();
                return false;
            }

            if (regionalCache.CheckAccess(regionalConstants.ItemName(operationRegional), DateTime.Now) == AuthorizationType.Allow)
            {
                return true;
            }
            return false;
        }


        public static bool HubOperationCheck(HubConstants.Operation operationHub)
        {
            
            var hubConstants = new HubConstants();
            var hubCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.Hub);

            
            if (null == hubCache)
            {
                Signout();
                return false;
            }

            if (hubCache.CheckAccess(hubConstants.ItemName(operationHub), DateTime.Now) == AuthorizationType.Allow)
            {
                return true;
            }
            return false;
        }
      
        public static bool ProcurementOperationCheck(ProcurementConstants.Operation operationProcurement)
        {
            //return MvcHtmlString.Create(@"<a data-buttontype=" + dataButtontype + "  class=" + ccsClass + " href=" + url + ">" + text + "</a>");

            var procurementConstants = new ProcurementConstants();
            var procurementCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.Procurement);

            // If cache is null then force the user to sign-in again
            if (null == procurementCache)
            {
                Signout();
                return false;
            }

            if (procurementCache.CheckAccess(procurementConstants.ItemName(operationProcurement), DateTime.Now) == AuthorizationType.Allow)
            {
                return true;
            }
            return false;
        }

        public static bool LogisticsOperationCheck(LogisticsConstants.Operation operationLogistics)
        {
            //return MvcHtmlString.Create(@"<a data-buttontype=" + dataButtontype + "  class=" + ccsClass + " href=" + url + ">" + text + "</a>");

            var logisticsConstants = new LogisticsConstants();
            var logisticsCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.Logistics);

            // If cache is null then force the user to sign-in again
            if (null == logisticsCache)
            {
                Signout();
                return false;
            }

            if (logisticsCache.CheckAccess(logisticsConstants.ItemName(operationLogistics), DateTime.Now) == AuthorizationType.Allow)
            {
                return true;
            }
            return false;
        }

        public static bool FinanceOperationCheck(FinanceConstants.Operation operationFinance)
        {
            //return MvcHtmlString.Create(@"<a data-buttontype=" + dataButtontype + "  class=" + ccsClass + " href=" + url + ">" + text + "</a>");

            var financeConstants = new FinanceConstants();
            var financeCache = UserAccountHelper.GetUserPermissionCache(CatsGlobals.Applications.Finance);

            // If cache is null then force the user to sign-in again
            if (null == financeCache)
            {
                Signout();
                return false;
            }

            if (financeCache.CheckAccess(financeConstants.ItemName(operationFinance), DateTime.Now) == AuthorizationType.Allow)
            {
                return true;
            }
            return false;
        }

        private static MvcHtmlString Signout()
        {
            FormsAuthentication.SignOut();
            return MvcHtmlString.Create(string.Empty);
        }

    }
}