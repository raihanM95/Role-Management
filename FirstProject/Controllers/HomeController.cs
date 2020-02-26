using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstProject.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies.Get("admin") != null)
            {
                return RedirectToAction("Index", "Users");
            }
            else if(Request.Cookies.Get("client") != null)
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

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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
    }
}
