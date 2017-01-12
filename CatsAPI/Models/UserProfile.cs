using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cats.Models;

namespace Cats.Rest.Models
{
    public class UserProfile
    {
        public UserProfile(int userProfileID, string userName, string password, string firstName, string lastName, string grandFatherName, bool activeInd, bool loggedInInd, DateTime? logginDate, DateTime? logOutDate, int failedAttempts, bool lockedInInd, string languageCode, string datePreference, string preferedWeightMeasurment, string keyboard, string mobileNumber, string email, string defaultTheme, int? partitionId, int? defaultHub, int? regionID, bool regionalUser, bool isAdmin, bool? tariffEntry)
        {
            UserProfileID = userProfileID;
            UserName = userName;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            GrandFatherName = grandFatherName;
            ActiveInd = activeInd;
            LoggedInInd = loggedInInd;
            LogginDate = logginDate;
            LogOutDate = logOutDate;
            FailedAttempts = failedAttempts;
            LockedInInd = lockedInInd;
            LanguageCode = languageCode;
            DatePreference = datePreference;
            PreferedWeightMeasurment = preferedWeightMeasurment;
            Keyboard = keyboard;
            MobileNumber = mobileNumber;
            Email = email;
            DefaultTheme = defaultTheme;
            PartitionId = partitionId;
            DefaultHub = defaultHub;
            RegionID = regionID;
            RegionalUser = regionalUser;
            IsAdmin = isAdmin;
            TariffEntry = tariffEntry;
        }

        public int UserProfileID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string GrandFatherName { get; set; }
        public bool ActiveInd { get; set; }
        public bool LoggedInInd { get; set; }
        public Nullable<System.DateTime> LogginDate { get; set; }
        public Nullable<System.DateTime> LogOutDate { get; set; }
        public int FailedAttempts { get; set; }
        public bool LockedInInd { get; set; }
        public string LanguageCode { get; set; }
        public string DatePreference { get; set; }
        public string PreferedWeightMeasurment { get; set; }
        public string Keyboard { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string DefaultTheme { get; set; }
        public int? PartitionId { get; set; }

       

        public int? DefaultHub { get; set; }
        public int? RegionID { get; set; }
        public bool RegionalUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool? TariffEntry { get; set; }
    }
}