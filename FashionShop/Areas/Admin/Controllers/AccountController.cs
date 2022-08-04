using FashionShop.Models;
using FashionShop.Models.Common;
using FashionShop.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FashionShop.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AdminModel adminModel = new AdminModel();
                var result = adminModel.Login(viewModel.Username,viewModel.Password);
                if (result == 1)
                {
                    var admin = adminModel.GetByUsername(viewModel.Username);
                    //set ADMIN_SESSION
                    var user = new UserLogin();
                    user.FirstName = admin.FirstName;
                    user.LastName = admin.LastName;
                    user.UserID = admin.ID;
                    user.Username = admin.Username;

                    Session.Add(Constant.ADMIN_SESSION, user);
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
                AdminModel adminModel = new AdminModel();
                var user = adminModel.GetByEmail(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Email chưa đăng ký");
                }
                else if (user.EmailConfirmed == false)
                {
                    ModelState.AddModelError("", "Email chưa xác thực");
                }
                else
                {
                    //ModelState.Clear();
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
                    ModelState.AddModelError("", "Mã xác thực chưa đúng");
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
            AdminModel adminModel = new AdminModel();
            var user = adminModel.GetByEmail(userCodeSess.Email);
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
                Models.EF.Admin user = new Models.EF.Admin();
                AdminModel adminModel = new AdminModel();
                user.ID = model.ID;
                user.Password = model.Password;
                bool result = adminModel.UpdatePassword(user);
                if (result)
                {
                    TempData["Message"] = "Lấy lại mật khẩu thành công";
                    TempData["Status"] = "success";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Remove(Constant.ADMIN_SESSION);
            return View("Login");
        }
    }
}