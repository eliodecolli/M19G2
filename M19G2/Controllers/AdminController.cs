using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Castle.Core.Internal;
using M19G2.Common;
using M19G2.Common.Logging;
using M19G2.DAL.Entities;
using M19G2.IBLL;
using M19G2.Models;
using M19G2.ViewModels;
using Microsoft.AspNet.Identity;

namespace M19G2.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRolesService _rolesService;
        private readonly ILogService _logService;
        private readonly IAccessRequestService _accessRequestService;
        private readonly IOrderQueueService _queueService;
        private readonly IDeliveryAutomation deliveryAutomation;
        private readonly SmtpClient client;

        public AdminController(
            IUserService userService, 
            IRolesService rolesService, 
            ILogService logService,
            IAccessRequestService accessRequestService,
            IOrderQueueService queueService,
            IDeliveryAutomation deliveryAutomation
        )
        {
            _userService = userService;
            _rolesService = rolesService;
            _logService = logService;
            _accessRequestService = accessRequestService;
            _queueService = queueService;
            this.deliveryAutomation = deliveryAutomation;
            client = new SmtpClient();
        }

        // GET: Admin
        public ActionResult Index(bool showDisabled = false)
        {
            try
            {
                List<UserDto> users;
                if (!showDisabled)
                {
                    ViewBag.showDisabled = "false";
                    users = _userService.GetAllActiveUsers();
                    return View("Index", users);
                }

                ViewBag.showDisabled = "true";
                users = _userService.GetAllInactiveUsers();
                return View("Index", users);
            }
            catch (Exception e)
            {
                CustomLogger.LogError("Error getting inactive users", e);
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult Logs()
        {
            try
            {
                var logs = _logService.GetAllLogs();
                return View(logs);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var userInDb = UserManager.FindById(id);
                if (userInDb == null)
                    return HttpNotFound();

                var viewModel = new UserFormViewModel()
                {
                    Email = userInDb.Email,
                    FirstName = userInDb.FirstName,
                    LastName = userInDb.LastName,
                    Gender = userInDb.Gender,
                    Birthday = userInDb.Birthday,
                    PhoneNumber = userInDb.PhoneNumber,
                    AspNetRoleName = _rolesService.GetRoleOfUser(id).Name,
                    AspNetRoles = _rolesService.GetAllRoles(),
                };

                return View("UserForm", viewModel);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500);
            }
        }

        public ActionResult Create()
        {
            try
            {
                var viewModel = new UserFormViewModel()
                {
                    AspNetRoles = _rolesService.GetAllRoles(),
                };
                return View("UserForm", viewModel);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500);
            }
        }


        [HttpGet]
        public ActionResult ReviewAccessRequests()
        {
            try
            {
                var accessRequests = _accessRequestService.GetAllRequests();
                return View(accessRequests);
            }
            catch (Exception e)
            {
                CustomLogger.LogError("Cannot get access requests", e);
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> ApproveAccessRequest(int accessRequestId)
        {
            try
            {
                //make request status approved
                var accessRequest = _accessRequestService.GetById(accessRequestId);

                if(accessRequest == null)
                {
                    return Content("Access Request does not exist");
                }

                _accessRequestService.AcceptRequest(accessRequestId);

                //create user
                var applicationUser = new ApplicationUser()
                {
                    FirstName = accessRequest.FirstName,
                    LastName = accessRequest.LastName,
                    Email = accessRequest.Email,
                    Gender = accessRequest.Gender,
                    Birthday = accessRequest.Birthday,
                    DateCreated = DateTime.UtcNow,
                    PhoneNumber = accessRequest.PhoneNumber,
                    UserName = accessRequest.Email,
                };

                string password = PasswordHelper.GenerateRandomPassword();
                var result = await UserManager.CreateAsync(applicationUser, password);

                //AddRole
                UserManager.AddToRole(applicationUser.Id, accessRequest.AspNetRole.Name);
                var callbackUrl = Url.Action("Login", "Account");
                await UserManager.SendEmailAsync(applicationUser.Id, "Your user request was approved",
                    "Login by going to this link <a href=\"http://localhost:2276/account/login \">here </a> with " +
                    password + " as a password");

                if(accessRequest.AspNetRole.Name == "Kitched Staff")
                {
                    _queueService.AddWorkforce(new KitchenStaff() { ID = applicationUser.Id, Name = applicationUser.UserName, Capacity = 3, Workload = new Queue<int>() });  /// uh?
                }

                if(accessRequest.AspNetRole.Name == "Delivery Service Staff")
                {
                    deliveryAutomation.AddWorkforce(applicationUser.Id);
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                CustomLogger.LogError("Cannot approve Access Request", e);
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public ActionResult DenyAccessRequest(int accessRequestId)
        {
            try
            {
                //make request status approved
                var accessRequest = _accessRequestService.GetById(accessRequestId);

                if (accessRequest == null)
                {
                    return Content("Access Request does not exist");
                }

                _accessRequestService.DenyRequest(accessRequestId);
                return RedirectToAction("ReviewAccessRequests");
            }
            catch (Exception e)
            {
                CustomLogger.LogError("Cannot deny access request", e);
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public ActionResult DisableUser(int id)
        {
            try
            {
                var user = UserManager.FindById(id);
                user.LockoutEndDateUtc = DateTime.MaxValue;
                UserManager.Update(user);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult EnableUser(int id)
        {
            try
            {
                var user = UserManager.FindById(id);
                user.LockoutEndDateUtc = null;
                UserManager.Update(user);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public ActionResult AnonymizeAccount(int userId)
        {
            try
            {
                var userInDb = UserManager.FindById(userId);
                if (userInDb == null)
                {
                    return HttpNotFound();
                }

                _userService.AnonimyzeUser(userId);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                CustomLogger.LogError("Aksnonymyzing account", e);
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public ActionResult ChangePassword(int id)
        {
            try
            {
                var user = UserManager.FindById(id);
                string newPassword = PasswordHelper.GenerateRandomPassword();
                UserManager.RemovePassword(id);
                UserManager.AddPassword(id, newPassword);
                UserManager.Update(user);

                UserManager.SendEmail(id, "Your password was changed",
                    "Login by going to this link <a href=\"http://localhost:2276/account/login \">here </a> with " +
                    newPassword + " as a password");
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Save(UserFormViewModel user)
        {
            try
            {
                //kontrollo nese ka validation errors
                if (!ModelState.IsValid)
                {
                    var viewModel = new UserFormViewModel()
                    {
                        AspNetRoles = _rolesService.GetAllRoles(),

                    };
                    return View("UserForm", viewModel);
                }
                //krijo user te ri nese nuk ekziston
                if (user.Id == 0)
                {
                    var applicationUser = new ApplicationUser();

                    //kontrollo nese useri ka nje pendin access request dhe merri ato te dhena
                    var accessRequest = _accessRequestService.GetPendingRequestByEmail(user.Email);
                    if(accessRequest != null)
                    {
                        applicationUser.FirstName = accessRequest.FirstName;
                        applicationUser.LastName = accessRequest.LastName;
                        applicationUser.Email = accessRequest.Email;
                        applicationUser.Gender = accessRequest.Gender;
                        applicationUser.Birthday = accessRequest.Birthday;
                        applicationUser.DateCreated = DateTime.UtcNow;
                        applicationUser.PhoneNumber = accessRequest.PhoneNumber;
                        applicationUser.UserName = accessRequest.Email;

                        _accessRequestService.AcceptRequest(accessRequest.Id);

                    }
                    else
                    {
                        applicationUser.FirstName = user.FirstName;
                        applicationUser.LastName = user.LastName;
                        applicationUser.Email = user.Email;
                        applicationUser.Gender = user.Gender;
                        applicationUser.Birthday = user.Birthday;
                        applicationUser.DateCreated = DateTime.UtcNow;
                        applicationUser.PhoneNumber = user.PhoneNumber;
                        applicationUser.UserName = user.Email;
                    }

                    string password = PasswordHelper.GenerateRandomPassword();
                    //Create user
                    var result = await UserManager.CreateAsync(applicationUser, password);

                    //kthe userform, por me erroret
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }

                        user.AspNetRoles = _rolesService.GetAllRoles();
                        return View("UserForm", user);
                    }

                    //AddRole
                    var roleResult = UserManager.AddToRole(applicationUser.Id, user.AspNetRoleName);
                    var callbackUrl = Url.Action("Login", "Account");
                    await UserManager.SendEmailAsync(applicationUser.Id, "Your account was created",
                        "Login by going to this link <a href=\"http://localhost:2276/account/login \">here </a> with " +
                        password + " as a password");
                }
                //update userit
                else
                {
                    var userInDb = UserManager.FindById(user.Id);
                    if (userInDb == null)
                    {
                        return HttpNotFound();
                    }

                    string oldUserEmail = userInDb.Email;

                    //start updating user
                    userInDb.FirstName = user.FirstName;
                    userInDb.LastName = user.LastName;
                    userInDb.Email = user.Email;
                    userInDb.Gender = user.Gender;
                    userInDb.Birthday = user.Birthday;
                    userInDb.PhoneNumber = user.PhoneNumber;

                    //update roles
                    var userRoles = UserManager.GetRoles(user.Id).ToArray();
                    UserManager.RemoveFromRoles(user.Id, userRoles);
                    UserManager.AddToRole(user.Id, user.AspNetRoleName);

                    UserManager.Update(userInDb);

                    //kontrollo nese ka ndryshuar emaili pasi eshte bere success change i userit
                    if (oldUserEmail != user.Email.ToLower())
                    {
                        //dergo email tek emaili i vjeter
                        IdentityMessage oldEmailMessage = new IdentityMessage()
                        {
                            Subject = "Your email was changed",
                            Body = "Login by going to this link <a href=\"http://localhost:2276/account/login \">here </a> with " +
                                   user.Email + " as the email of your account",
                            Destination = oldUserEmail,
                        };
                        var msg = new MailMessage("tasoshytiii@gmail.com", oldEmailMessage.Destination, oldEmailMessage.Subject, oldEmailMessage.Body);
                        msg.IsBodyHtml = true;
                        await client.SendMailAsync(msg);

                        //dergo email tek emaili i ri
                        IdentityMessage newEmailMessage = new IdentityMessage()
                        {
                            Subject = "Your email was changed",
                            Body = "Login by going to this link <a href=\"http://localhost:2276/account/login \">here </a> with ",
                            Destination = user.Email,
                        };
                        var newMsg = new MailMessage("tasoshytiii@gmail.com", newEmailMessage.Destination, newEmailMessage.Subject, newEmailMessage.Body);
                        newMsg.IsBodyHtml = true;
                        await client.SendMailAsync(newMsg);
                    }

                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(500);
            }


        }
    }
}