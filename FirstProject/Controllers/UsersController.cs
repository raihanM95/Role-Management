using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using FirstProject.DatabaseContext;
using FirstProject.Models;
using FirstProject.ViewModels;
using PagedList;
using Encrypt.Pass;

namespace FirstProject.Controllers
{
    public class UsersController : Controller
    {
        private DBContext db = new DBContext();
        private UserViewModel _userViewModel = new UserViewModel();
        private Users _users = new Users();

        // GET: Users
        public ActionResult Index(int? page)
        {
            if (Request.Cookies.Get("admin") != null)
            {
                var pageNumber = page ?? 1;
                var pageSize = 10;

                _userViewModel.Users = db.Users.OrderBy(i => i.ID).ToPagedList(pageNumber, pageSize);

                return View(_userViewModel);
            }
            return RedirectToAction("Login", "Users");
        }

        public ActionResult Details(int? id)
        {
            if (Request.Cookies.Get("admin") != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var users = db.Users.Find(id);
                if (users == null)
                {
                    return HttpNotFound();
                }

                _userViewModel.Name = users.Name;
                _userViewModel.UserType = users.UserType;
                _userViewModel.UserName = users.UserName;
                return View(_userViewModel);
            }
            return RedirectToAction("Login", "Users");
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Users");
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,UserName,Password,UserType")] RegistrationViewModel registrationViewModel)
        {
            ModelState["ConfirmPassword"].Errors.Clear();

            _users.Name = registrationViewModel.Name;
            _users.UserName = registrationViewModel.UserName;
            _users.Password = Crypto.Hash(registrationViewModel.Password);

            _users.UserType = registrationViewModel.UserType;
            if (ModelState.IsValid)
            {
                // Username is already Exist
                var isExist = IsUserNameExist(registrationViewModel.UserName);
                if (isExist)
                {
                    ModelState.AddModelError("UserNameExist", "User name already exist");
                    return View(registrationViewModel);
                }

                db.Users.Add(_users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        [NonAction]
        public bool IsUserNameExist(string username)
        {
            var e = this.db.Users.FirstOrDefault(u => u.UserName == username);
            return e != null;
        }

        // POST: Users/Delete/5
        public ActionResult Delete(int id)
        {
            ClientProduct client = db.ClientProducts.Where(i => i.UsersId == id).FirstOrDefault();
            SupportProduct support = db.SupportProducts.Where(i => i.UsersId == id).FirstOrDefault();
            if(client != null || support != null)
            {
                TempData["Message"] = "Can not delete this user!";
            }
            else
            {
                Users users = db.Users.Find(id);
                if(users.UserType == "System")
                {
                    TempData["Message"] = "Can not delete system user!";
                }
                else
                {
                    db.Users.Remove(users);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Users/Login
        public ActionResult Login()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Index", "Users");
            }
            else if (Request.Cookies.Get("client") != null)
            {
                return RedirectToAction("Index", "Tokens");
            }
            else if (Request.Cookies.Get("support") != null)
            {
                return RedirectToAction("SupportToken", "Tokens");
            }
            else
            {
                return View();
            }
        }

        // POST: Users/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                Users user = db.Users
                .Where(m => m.UserName == loginViewModel.UserName).SingleOrDefault();
                if (user != null)
                {
                    if (string.Compare(Crypto.Hash(loginViewModel.Password), user.Password) == 0)
                    {
                        int timeout = loginViewModel.RememberMe ? 1440 : 720; // 1440 min = 1 day && 720 min= 12 hour
                        
                        if (user.UserType == "Admin" || user.UserType == "System")
                        {
                            HttpCookie cookie = new HttpCookie("admin", user.UserName);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            Response.Cookies.Add(cookie);

                            return RedirectToAction("Index", "Users");
                        }
                        else if (user.UserType == "Client")
                        {
                            HttpCookie cookie = new HttpCookie("client", user.UserName);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            Response.Cookies.Add(cookie);

                            return RedirectToAction("Index", "Tokens");
                        }
                        else if (user.UserType == "Support")
                        {
                            HttpCookie cookie = new HttpCookie("support", user.UserName);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            Response.Cookies.Add(cookie);

                            return RedirectToAction("SupportToken", "Tokens");
                        }
                    }
                }
            }
            return RedirectToAction("Login", "Users");
        }

        public ActionResult Logout()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                FormsAuthentication.SignOut();
                Response.Cookies["admin"].Expires = DateTime.Now.AddDays(-1);
            }
            else if (Request.Cookies.Get("client") != null)
            {
                FormsAuthentication.SignOut();
                Response.Cookies["client"].Expires = DateTime.Now.AddDays(-1);
            }
            else if (Request.Cookies.Get("support") != null)
            {
                FormsAuthentication.SignOut();
                Response.Cookies["support"].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
