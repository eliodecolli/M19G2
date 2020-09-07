using M19G2.Common.Logging;
using M19G2.IBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using M19G2.ViewModels;
using M19G2.DAL.Entities;
using Microsoft.AspNet.Identity;

namespace M19G2.Controllers
{
    public class AccessRequestsController : BaseController
    {
        private readonly IAccessRequestService _accessRequestService;
        private readonly IRolesService _rolesService;

        public AccessRequestsController(IAccessRequestService accessRequestService, IRolesService rolesService)
        {
            _accessRequestService = accessRequestService;
            _rolesService = rolesService;
        }

        [HttpGet]
        public ActionResult CreateAccessRequest()
        {
            try
            {
                var viewModel = new AccessRequestFromViewModel()
                {
                    UserAccessRequests = new UsersAccessRequest(),
                    AspNetRoles = _rolesService.GetRolesForAccessRequest()
                };
                return View(viewModel);
            }
            catch (Exception e)
            {
                CustomLogger.LogError("Cannot get access request viewmodel properties", e);
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public ActionResult CreateAccessRequest(AccessRequestFromViewModel acccessRequestFromViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var viewModel = new AccessRequestFromViewModel()
                    {
                        UserAccessRequests = acccessRequestFromViewModel.UserAccessRequests,
                        AspNetRoles = _rolesService.GetRolesForAccessRequest()
                    };
                    return View(viewModel);
                }

                //kontrollo nese useri ekziston tek tabelat e user ose user requests
                var userInDb = UserManager.FindByEmail(acccessRequestFromViewModel.UserAccessRequests.Email);
                var userInAccessRequests = _accessRequestService.GetByEmail(acccessRequestFromViewModel.UserAccessRequests.Email);

                if (userInDb != null || userInAccessRequests != null)
                {
                    ModelState.AddModelError("" ,"Email already exists");
                    var viewModel = new AccessRequestFromViewModel()
                    {
                        UserAccessRequests = acccessRequestFromViewModel.UserAccessRequests,
                        AspNetRoles = _rolesService.GetRolesForAccessRequest()
                    };
                    return View(viewModel);
                }

                //krijo user request
                _accessRequestService.CreateAcessRequest(acccessRequestFromViewModel.UserAccessRequests);
                return Content("Your request is sent successfully");
            }
            catch(Exception e)
            {
                CustomLogger.LogError("Cannot create access request", e);
                return new HttpStatusCodeResult(500);
            }
        }
    }
}