using LaundryApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LaundryApplication.Controllers
{
    public class HomeController : Controller
    {
        private LaundryDBEntities db = new LaundryDBEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ClientLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ClientLogin(Customer cus)
        {
            Password p = new Password();
            OnlineOrder order = new OnlineOrder();
            var passDecoder = p.Encrypt(cus.Password);
            var CusLogin = db.Customers.Where(model => model.Cus_Name == cus.Cus_Name && model.Password == passDecoder).FirstOrDefault();
  
            if (CusLogin != null)
            {
                FormsAuthentication.SetAuthCookie(cus.Cus_Name, false);
                Session["CustomerID"] = cus.Cus_Id.ToString();
                Session["CustomerName"] = cus.Cus_Name.ToString();
                var UserLogin = Session["CustomerName"];
                var CustomerOrder = db.OnlineOrders.Where(model => model.Customer.Cus_Name == cus.Cus_Name).FirstOrDefault();
                //if (order.Customer.Cus_Name.ToString() == UserLogin.ToString())
                if (CustomerOrder != null)
                {
                    TempData["CusLogin"] = "<script>alert('Login Successfuly')</script>";
                    return RedirectToAction("PostInvoice", "OnlineOrders");
                }
                else
                {
                    TempData["Post"] = "<script>alert('First Create an order')</script>";
                    return RedirectToAction("LaundryOrder", "Home");
                }
            }
            else
            {
                TempData["CusFail"] = "<script>alert('You are not an Authorize User, Please first register your account')</script>";
                return RedirectToAction("Registration", "Home");

            }     

        }

        public ActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Registration(Customer cus)
        {
            if (ModelState.IsValid)
            {
                Password p = new Password();
                cus.Password = p.Encrypt(cus.Password);
                db.Customers.Add(cus);
                int a = db.SaveChanges();
                if (a > 0)
                {
                    TempData["Reg"] = "<script>alert('Registered Successfully')</script>";
                }
                return RedirectToAction("ClientLogin", "Home");
            }
            return View();
        }

        public ActionResult LaundryOrder()
        {
            ViewBag.CatIDFk = new SelectList(db.Categories, "Cat_Id", "Cat_Name");
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name");
            ViewBag.PayIDFK = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type");
            return View();
        }
        [HttpPost]
        public ActionResult LaundryOrder([Bind(Include = "Ord_Id,CusIDFK,CatIDFK,PayIDFk,Pay_Des,Cat_Des,Item_Quantity,Amount")] OnlineOrder onlineOrder)
        {
            
            if (ModelState.IsValid)
            {
                int NetAmount = BillCal(onlineOrder);

                if (onlineOrder.Cat_Des == "Per Piece")
                {
                    if (onlineOrder.Pay_Des == "Cash")
                    {
                        onlineOrder.Pay_Status = "Pending";
                        onlineOrder.Amount = NetAmount;
                        db.OnlineOrders.Add(onlineOrder);
                        db.SaveChanges();
                    }
                    else
                    {
                        onlineOrder.Pay_Status = "Paid";
                        onlineOrder.Amount = NetAmount;
                        db.OnlineOrders.Add(onlineOrder);
                        db.SaveChanges();
                    }                
                }
                
                else if (onlineOrder.Cat_Des == "Per Kg")
                {
                    if (onlineOrder.Pay_Des == "Cash")
                    {
                        onlineOrder.Pay_Status = "Pending";
                        onlineOrder.Amount = NetAmount;
                        db.OnlineOrders.Add(onlineOrder);
                        db.SaveChanges();
                    }
                    else
                    {
                        onlineOrder.Pay_Status = "Paid";
                        onlineOrder.Amount = NetAmount;
                        db.OnlineOrders.Add(onlineOrder);
                        db.SaveChanges();
                    }
       
                }
                else
                {

                    if (onlineOrder.Item_Quantity > 5)
                    {
                        TempData["PackageError"] = "<script>alert('Item Quantity are Fixed that is 5 Items')</script>";
                        return RedirectToAction("LaundryOrder", "Home");
                    }
                    else
                    {
                        if (onlineOrder.Pay_Des == "Cash")
                        {
                            onlineOrder.Pay_Status = "Pending";
                            onlineOrder.Amount = NetAmount;
                            db.OnlineOrders.Add(onlineOrder);
                            db.SaveChanges();
                        }
                        else
                        {
                            onlineOrder.Pay_Status = "Paid";
                            onlineOrder.Amount = NetAmount;
                            db.OnlineOrders.Add(onlineOrder);
                            db.SaveChanges();
                        }
                    }

                }

                return RedirectToAction("ClientLogin", "Home");

            }
            ViewBag.CatIDFk = new SelectList(db.Categories, "Cat_Id", "Cat_Name", onlineOrder.CatIDFK);
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name", onlineOrder.CusIDFk);
            ViewBag.PayIDFK = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type", onlineOrder.PayIDFk);
            //ViewBag.HasPassport = hasPassport;
            return View(onlineOrder);

            // return View();
        }

        public int BillCal(OnlineOrder b)
        {
            int NetAmount = 0;
            if (b.Cat_Des == "Per Piece")
            {
                NetAmount = Convert.ToInt32(b.Item_Quantity) * 50;
                TempData["Cal"] = NetAmount;
            }
            else if (b.Cat_Des == "Per Kg")
            {
                NetAmount = Convert.ToInt32(b.Item_Quantity) * 100;
                TempData["Cal"] = NetAmount;
            }
            else
            {
                NetAmount = 500;
                TempData["Cal"] = NetAmount;
            }
            return NetAmount;
        }
    }
}