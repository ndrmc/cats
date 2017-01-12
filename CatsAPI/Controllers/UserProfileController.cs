using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

using Cats.Services.Administration;

namespace Cats.Rest.Controllers
{
    public class UserProfileController : ApiController

    {
        //
        // GET: /UserProfile/

        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }
        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns></returns>
        public List<Models.UserProfile> Get()
        {
            var userProfles = _userProfileService.GetAllUserProfile();
            return userProfles.Select(userProfile => new Models.UserProfile(userProfile.UserProfileID, userProfile.UserName, userProfile.Password, userProfile.FirstName, userProfile.LastName, userProfile.GrandFatherName, userProfile.ActiveInd, userProfile.LoggedInInd, userProfile.LogginDate, userProfile.LogOutDate, userProfile.FailedAttempts, userProfile.LockedInInd, userProfile.LanguageCode, userProfile.DatePreference, userProfile.PreferedWeightMeasurment, userProfile.Keyboard, userProfile.MobileNumber, userProfile.Email, userProfile.DefaultTheme, userProfile.PartitionId, userProfile.DefaultHub, userProfile.RegionID, userProfile.RegionalUser, userProfile.IsAdmin, userProfile.TariffEntry)).ToList();
        }
        /// <summary>
        /// Returns a single user by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Models.UserProfile Get(int id)
        {

            var userProfile = _userProfileService.FindById(id);
            if (userProfile != null)
            {
                var newUserProfile = new Models.UserProfile(userProfile.UserProfileID, userProfile.UserName,
                                                            userProfile.Password, userProfile.FirstName,
                                                            userProfile.LastName, userProfile.GrandFatherName,
                                                            userProfile.ActiveInd, userProfile.LoggedInInd,
                                                            userProfile.LogginDate, userProfile.LogOutDate,
                                                            userProfile.FailedAttempts, userProfile.LockedInInd,
                                                            userProfile.LanguageCode, userProfile.DatePreference,
                                                            userProfile.PreferedWeightMeasurment, userProfile.Keyboard,
                                                            userProfile.MobileNumber, userProfile.Email,
                                                            userProfile.DefaultTheme, userProfile.PartitionId,
                                                            userProfile.DefaultHub, userProfile.RegionID,
                                                            userProfile.RegionalUser, userProfile.IsAdmin,
                                                            userProfile.TariffEntry);

                return newUserProfile;
            }
            return null;
        }
    }
}
