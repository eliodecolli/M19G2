using M19G2.IBLL;
using System.Web.Mvc;

namespace M19G2.Controllers
{
    public class DummyController : BaseController
    {
        private readonly IUserService _userService;

        public DummyController(IUserService userService)
        {
            _userService = userService;
        }

    }
}