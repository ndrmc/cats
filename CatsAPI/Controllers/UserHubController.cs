using Cats.Services.EarlyWarning;
using Cats.Services.Hub;
using System.Linq;
using System.Web.Http;

namespace Cats.Rest.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class UserHubController : ApiController
    {
        private readonly IUserHubService _iUserHubService;
        /// <summary>
        ///
        /// </summary>
        /// <param name="iUserHubService"></param>
        public UserHubController(IUserHubService iUserHubService)
        {
            _iUserHubService = iUserHubService;
        }
        /// <summary>
        /// Returns list of UserHub objects
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetUserHubs()
        {
            var results = _iUserHubService.GetAllUserHub().Select(item => new
						   {
							UserHubId=item.UserHubID,
							item.UserProfileID,
							UserName=item.UserProfile.FullName,
							HubId=item.HubID,
							item.IsDefault
						  }).ToList();

            return results;
        }
        /// <summary>
        /// Given an id returns a UserHub object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public dynamic GetUserHub(int id)
        {
            var obj = _iUserHubService.FindById(id);
            var element = new
						 {
                UserHubId = obj.UserHubID,
                obj.UserProfileID,
                UserName = obj.UserProfile.FullName,
                HubId = obj.HubID,
                obj.IsDefault
            };

            return element;
        }
    }
}


