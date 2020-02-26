using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FirstProject.DatabaseContext;
using FirstProject.Models;
using FirstProject.ViewModels;
using PagedList;

namespace FirstProject.Controllers
{
    public class TokensController : Controller
    {
        private DBContext db = new DBContext();
        private TokenViewModel _tokenViewModel = new TokenViewModel();
        private Token _token = new Token();

        // GET: Tokens
        public ActionResult Index(int? page)
        {
            if (Request.Cookies.Get("client") != null)
            {
                var pageNumber = page ?? 1;
                var pageSize = 10;

                var cli = Request.Cookies.Get("client").Value;
                Users client = db.Users.Where(c => c.UserName == cli).FirstOrDefault();
                var product = db.ClientProducts.Include(t => t.Product).Where(r => r.UsersId == client.ID).ToList();
                
                List<Token> tokenList = new List<Token>();
                
                foreach (var item in product)
                {
                    if(tokenList.Count > 0){
                        if(tokenList.Where(i=> i.ProductId == item.ProductId).ToList().Count == 0) {
                            tokenList.AddRange(db.Tokens.Where(r => r.ProductId == item.ProductId));
                        }
                    }
                    else
                    {
                        tokenList.AddRange(db.Tokens.Where(r => r.ProductId == item.ProductId));
                    }
                }

                TokenViewModel tokenViewModel = new TokenViewModel {
                    Tokens = tokenList.OrderBy(d => d.ComplainDate).ToPagedList(pageNumber, pageSize)
                };
                return View(tokenViewModel);
            }
            return RedirectToAction("Login", "Users");
        }

        // GET: Tokens/New
        public ActionResult New(int? page)
        {
            if (Request.Cookies.Get("admin") != null)
            {
                var pageNumber = page ?? 1;
                var pageSize = 10;

                var tokens = db.Tokens.Include(t => t.Product);
                _tokenViewModel.Tokens = tokens.OrderBy(d => d.ComplainDate).ToPagedList(pageNumber, pageSize);
                return View(_tokenViewModel);
            }
            return RedirectToAction("Login", "Users");
        }

        // GET: Tokens/Solved
        public ActionResult UnSolved(int? page)
        {
            if (Request.Cookies.Get("admin") != null)
            {
                var pageNumber = page ?? 1;
                var pageSize = 10;

                var tokens = db.Tokens.Include(t => t.Product);
                _tokenViewModel.Tokens = tokens.OrderBy(d => d.ComplainDate).ToPagedList(pageNumber, pageSize);
                return View(_tokenViewModel);
            }
            return RedirectToAction("Login", "Users");
        }

        // GET: Tokens/Solved
        public ActionResult Solved(int? page)
        {
            if (Request.Cookies.Get("admin") != null)
            {
                var pageNumber = page ?? 1;
                var pageSize = 10;

                var tokens = db.Tokens.Include(t => t.Product);
                _tokenViewModel.Tokens = tokens.OrderBy(d => d.SolvedDate).ToPagedList(pageNumber, pageSize);
                return View(_tokenViewModel);
            }
            return RedirectToAction("Login", "Users");
        }

        // GET: Tokens/Create
        public ActionResult Create()
        {
            if (Request.Cookies.Get("client") != null)
            {
                var cli = Request.Cookies.Get("client").Value;

                Users client = db.Users.Where(c => c.UserName == cli).FirstOrDefault();
                var products = db.ClientProducts.Include(p => p.Product).Where(c => c.UsersId == client.ID).ToList();

                List<Product> productList = new List<Product>();
                
                foreach (var product in products)
                {
                    productList.Add(db.Products.Where(i => i.ID == product.ProductId).FirstOrDefault());
                }

                //token.ProductSelectListItems = productList.Distinct().Select(c => new SelectListItem()
                //{
                //    Value = c.ID.ToString(),
                //    Text = c.Name
                //});

                ViewBag.ProductId = new SelectList(productList.Distinct(), "ID", "Name");

                return View();
            }
            return RedirectToAction("Login", "Users");
        }

        // POST: Tokens/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ComplainDescription,SolvedDescription,Type,Status,ProductId")] TokenViewModel tokenViewModel)
        {
            if (ModelState.IsValid)
            {
                _token.ComplainDescription = tokenViewModel.ComplainDescription;
                _token.ProductId = tokenViewModel.ProductId;
                _token.ComplainDate = DateTime.Now;
                db.Tokens.Add(_token);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var cli = Request.Cookies.Get("client").Value;

            Users client = db.Users.Where(c => c.UserName == cli).FirstOrDefault();
            var products = db.ClientProducts.Include(p => p.Product).Where(c => c.UsersId == client.ID).ToList();

            List<Product> productList = new List<Product>();

            foreach (var product in products)
            {
                productList.Add(db.Products.Where(i => i.ID == product.ProductId).FirstOrDefault());
            }

            //token.ProductSelectListItems = productList.Distinct().Select(c => new SelectListItem()
            //{
            //    Value = c.ID.ToString(),
            //    Text = c.Name
            //});

            ViewBag.ProductId = new SelectList(productList.Distinct(), "ID", "Name");

            return View();
        }

        // POST: Tokens/Delete/5
        public ActionResult Delete(int id)
        {
            Token token = db.Tokens.Find(id);
            db.Tokens.Remove(token);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult SupportToken(int? page)
        {
            if (Request.Cookies.Get("support") != null)
            {
                var pageNumber = page ?? 1;
                var pageSize = 10;

                var supp = Request.Cookies.Get("support").Value;
                Users client = db.Users.Where(c => c.UserName == supp).FirstOrDefault();
                var products = db.SupportProducts.Include(p => p.Product).Where(i => i.UsersId == client.ID).ToList();

                List<Token> tokenList = new List<Token>();

                foreach (var product in products)
                {
                    if (tokenList.Count > 0)
                    {
                        if (tokenList.Where(i => i.ProductId == product.ProductId).ToList().Count == 0)
                        {
                            tokenList.AddRange(db.Tokens.Where(i => i.ProductId == product.ProductId));
                        }
                    }
                    else
                    {
                        tokenList.AddRange(db.Tokens.Where(i => i.ProductId == product.ProductId));
                    }
                }

                TokenViewModel tokenViewModel = new TokenViewModel
                {
                    Tokens = tokenList.OrderBy(d => d.SolvedDate).ToPagedList(pageNumber, pageSize)
                };
                return View(tokenViewModel);
            }
            return RedirectToAction("Login", "Users");
        }

        [HttpPost]
        public ActionResult Solve(Token token)
        {
            if (token.ID != 0)
            {
                try
                {
                    Token aToken = this.db.Tokens.FirstOrDefault(t => t.ID == token.ID);
                    aToken.SolvedDate = DateTime.Now;
                    aToken.Status = true;
                    aToken.SolvedDescription = token.SolvedDescription;
                    
                    this.db.SaveChanges();
                    return RedirectToAction("SupportToken", "Tokens");
                }
                catch (Exception x)
                {
                    return Json(x);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public JsonResult Seen(int id)
        {
            Token aToken = this.db.Tokens.FirstOrDefault(t => t.ID == id);
            if(aToken.Type == false)
            {
                aToken.Type = true;

                this.db.SaveChanges();
            }
            return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
        }
    }
}
