using FashionShop.Models;
using FashionShop.Models.Common;
using FashionShop.Models.EF;
using FashionShop.Models.ViewModel;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Controllers
{
    public class AccountController : Controller
    {
        private CustomerModel customerModel = new CustomerModel();

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var result = customerModel.Login(viewModel.Username, viewModel.Password);
                if (result == 1)
                {
                    var user = customerModel.GetByUsername(viewModel.Username);
                    //set value session USER_SESSION
                    var userLogin = new UserLogin();
                    userLogin.LastName = user.LastName;
                    userLogin.UserID = user.ID;
                    userLogin.Username = user.Username;
                    userLogin.Email = user.Email;

                    Session.Add(Constant.CUSTOMER_SESSION, userLogin);
                    return RedirectToAction("Index", "Home");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("Username", "Tài khoản đã bị khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("Password", "Sai mật khẩu");
                }
                else
                {
                    ModelState.AddModelError("Username", "Tài khoản không tồn tại");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            Session.Remove(Constant.CUSTOMER_SESSION);
            return View("Login");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool checkUsername = customerModel.CheckUsername(model.Username);
                if (checkUsername)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập đã tồn tại");
                }
                else
                {
                    Customer user = new Customer();
                    user.Username = model.Username.Trim();
                    user.Password = model.Password;
                    user.Phone = model.Phone.Trim();
                    user.FirstName = model.FirstName.Trim();
                    user.LastName = model.LastName.Trim();
                    user.Address = model.Address.Trim();
                    user.Email = model.Email;
                    var userID = customerModel.Insert(user);
                    if (userID > 0)
                    {
                        TempData["Message"] = "Đăng ký thành công";
                        TempData["Status"] = "success";
                        return RedirectToAction("Login");
                    }
                }
            }
            return View();
        }

        public void SignIn(string ReturnUrl = "/", string Type = "")
        {
            if (!Request.IsAuthenticated)
            {
                if (Type == "google")
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "Account/GoogleLoginCallback" }, "Google");
                }
                if (Type == "microsoft")
                {
                    HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "Account/GoogleLoginCallback" }, "Microsoft");
                }
            }
        }

        [AllowAnonymous]
        public ActionResult GoogleLoginCallback()
        {
            var claimsPrincipal = HttpContext.User.Identity as ClaimsIdentity;
            var loginInfo = GoogleLoginViewModel.GetLoginInfo(claimsPrincipal);
            if (loginInfo == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                Customer user = new Customer();
                if (!customerModel.CheckUsername(loginInfo.EmailAddress))
                {
                    user.Email = loginInfo.EmailAddress;
                    user.Username = loginInfo.EmailAddress;
                    user.FirstName = loginInfo.SurName;
                    user.LastName = loginInfo.GiveName;

                    user.ID = customerModel.InsertAccountGoogle(user);
                }
                else
                {
                    user = customerModel.GetByUsername(loginInfo.EmailAddress);
                }
                var userSession = new UserLogin();
                userSession.FirstName = loginInfo.SurName;
                userSession.LastName = loginInfo.GiveName;
                userSession.Username = user.Username;
                userSession.UserID = user.ID;
                userSession.Type = "Google";

                Session.Add(Constant.CUSTOMER_SESSION, userSession);
                return Redirect("~/");
            }
            //var ident = new ClaimsIdentity(new[]
            //{
            //    new Claim(ClaimTypes.NameIdentifier, loginInfo.EmailAddress),
            //    //new Claim()
            //    new Claim(ClaimTypes.Name, loginInfo.Name),
            //    new Claim(ClaimTypes.Email, loginInfo.EmailAddress),
            //    new Claim(ClaimTypes.Role, "User"),
            //}, CookieAuthenticationDefaults.AuthenticationType);
            //HttpContext.GetOwinContext().Authentication.SignIn(new AuthenticationProperties { IsPersistent = false }, ident);
            //return Redirect("~/");
        }

        public ActionResult Forget()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Forget(ForgetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = customerModel.GetByEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Email", "Email chưa đăng ký");
                }
                else if (user.EmailConfirmed == false)
                {
                    ModelState.AddModelError("Email", "Email chưa xác thực");
                }
                else
                {
                    if (user.Type == "Shop")
                    {
                        Random rd = new Random();
                        string code = rd.Next(000000, 1000000).ToString();
                        //add UserCode to session
                        UserCode userCode = new UserCode();
                        userCode.UserID = user.ID;
                        userCode.Code = code;
                        userCode.Email = user.Email;
                        userCode.Status = false;
                        Session.Add(Constant.CodeSession, userCode);

                        string body = "<p>Mã xác thực của bạn là:" + userCode.Code + "</p>";
                        MailHelper.SendMail("Xác thực đăng nhập", body, userCode.Email);
                        return RedirectToAction("ConfirmCode");
                    }
                }
            }
            return View();
        }

        public ActionResult ConfirmCode()
        {
            var model = (UserCode)Session["CodeSession"];
            if (model == null)
            {
                return View("Forget");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmCode(UserCode userCode)
        {
            if (ModelState.IsValid)
            {
                var UserCodeSess = (UserCode)Session["CodeSession"];
                if (userCode.Code == UserCodeSess.Code)
                {
                    UserCodeSess.Status = true;
                    Session["CodeSession"] = UserCodeSess;
                    return RedirectToAction("NewPassword");
                }
                else
                {
                    ModelState.AddModelError("Code", "Mã xác thực chưa đúng");
                }
            }
            return View();
        }

        public ActionResult NewPassword()
        {
            var userCodeSess = (UserCode)Session["CodeSession"];
            if (userCodeSess == null || userCodeSess.Status == false)
            {
                return View("Forget");
            }
            var user = customerModel.GetByEmail(userCodeSess.Email);
            NewPasswordViewModel model = new NewPasswordViewModel();
            model.ID = user.ID;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewPassword(NewPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();
                customer.ID = model.ID;
                customer.Password = model.Password;
                bool result = customerModel.UpdatePassword(customer);
                if (result)
                {
                    TempData["Message"] = "Lấy lại mật khẩu thành công";
                    TempData["Status"] = "success";
                    return RedirectToAction("Login");
                }
            }
            return View(model);
        }
    }
}