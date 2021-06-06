using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaundryApplication.Models;

namespace LaundryApplication.Controllers
{
    public class OnlineOrdersController : Controller
    {
        private LaundryDBEntities db = new LaundryDBEntities();

        // GET: OnlineOrders
        public ActionResult Index()
        {
            var onlineOrders = db.OnlineOrders.Include(o => o.Category).Include(o => o.Customer).Include(o => o.PaymentMethod);
            return View(onlineOrders.ToList());
        }

        // GET: OnlineOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineOrder onlineOrder = db.OnlineOrders.Find(id);
            if (onlineOrder == null)
            {
                return HttpNotFound();
            }
            return View(onlineOrder);
        }

        // GET: OnlineOrders/Create
        public ActionResult Create()
        {
            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name");
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name");
            ViewBag.PayIDFk = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type");
            return View();
        }

        // POST: OnlineOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Ord_Id,CusIDFk,CatIDFK,PayIDFk,Cat_Des,Pay_Des,Item_Quantity,Amount,Pay_Status")] OnlineOrder onlineOrder)
        {
            if (ModelState.IsValid)
            {
                db.OnlineOrders.Add(onlineOrder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name", onlineOrder.CatIDFK);
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name", onlineOrder.CusIDFk);
            ViewBag.PayIDFk = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type", onlineOrder.PayIDFk);
            return View(onlineOrder);
        }

        // GET: OnlineOrders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineOrder onlineOrder = db.OnlineOrders.Find(id);
            if (onlineOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name", onlineOrder.CatIDFK);
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name", onlineOrder.CusIDFk);
            ViewBag.PayIDFk = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type", onlineOrder.PayIDFk);
            return View(onlineOrder);
        }

        // POST: OnlineOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ord_Id,CusIDFk,CatIDFK,PayIDFk,Cat_Des,Pay_Des,Item_Quantity,Amount,Pay_Status")] OnlineOrder onlineOrder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(onlineOrder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name", onlineOrder.CatIDFK);
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name", onlineOrder.CusIDFk);
            ViewBag.PayIDFk = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type", onlineOrder.PayIDFk);
            return View(onlineOrder);
        }

        // GET: OnlineOrders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OnlineOrder onlineOrder = db.OnlineOrders.Find(id);
            if (onlineOrder == null)
            {
                return HttpNotFound();
            }
            return View(onlineOrder);
        }

        // POST: OnlineOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OnlineOrder onlineOrder = db.OnlineOrders.Find(id);
            db.OnlineOrders.Remove(onlineOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult BarcodeGenerate()
        {
            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name");
            ViewBag.CusIDFK = new SelectList(db.Customers, "Cus_Id", "Cus_Name");
            ViewBag.PayIDFK = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type");
            return View();
        }
        [HttpPost]
        public ActionResult BarcodeGenerate(InvoiceGenerate invoice)
        {
            Random r = new Random();
            var Barcode = "Az" + r.Next() + "x";

            if (ModelState.IsValid)
            {
                int NetAmount = BillCal(invoice);

                if (invoice.Cat_Des == "Per Piece")
                {
                    
                    if(invoice.Pay_Des=="Cash")
                    {
                        invoice.Pay_Status = "Pending";
                        invoice.Amount = NetAmount;
                        invoice.DeliveryStatus = "Pending";
                        invoice.Barcode = Barcode;
                        db.InvoiceGenerates.Add(invoice);
                        db.SaveChanges();
                    }
                    else 
                    {
                        invoice.Pay_Status = "Paid";
                        invoice.Amount = NetAmount;
                        invoice.DeliveryStatus = "Pending";
                        invoice.Barcode = Barcode;
                        db.InvoiceGenerates.Add(invoice);
                        db.SaveChanges();
                    }                   
                }
                else if (invoice.Cat_Des == "Per Kg")
                {
                    if (invoice.Pay_Des == "Cash")
                    {
                        invoice.Pay_Status = "Pending";
                        invoice.Amount = NetAmount;
                        invoice.DeliveryStatus = "Pending";
                        invoice.Barcode = Barcode;
                        db.InvoiceGenerates.Add(invoice);
                        db.SaveChanges();
                    }
                    else
                    {
                        invoice.Pay_Status = "Paid";
                        invoice.Amount = NetAmount;
                        invoice.DeliveryStatus = "Pending";
                        invoice.Barcode = Barcode;
                        db.InvoiceGenerates.Add(invoice);
                        db.SaveChanges();
                    }
                  
                }
 
                else
                {
                    if (invoice.Item_Quantity > 5)
                    {
                        TempData["PackageError"] = "<script>alert('Item Quantity are Fixed that is 5 Items')</script>";
                        return RedirectToAction("LaundryOrder", "Home");
                    }
                    else
                    {
                        if (invoice.Pay_Des == "Cash")
                        {
                            invoice.Pay_Status = "Pending";
                            invoice.Amount = NetAmount;
                            invoice.DeliveryStatus = "Pending";
                            invoice.Barcode = Barcode;
                            db.InvoiceGenerates.Add(invoice);
                            db.SaveChanges();
                        }
                        else
                        {
                            invoice.Pay_Status = "Paid";
                            invoice.Amount = NetAmount;
                            invoice.DeliveryStatus = "Pending";
                            invoice.Barcode = Barcode;
                            db.InvoiceGenerates.Add(invoice);
                            db.SaveChanges();
                        }                    
                    }

                }

                return RedirectToAction("Index", "InvoiceGenerates");
            }
            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name", invoice.CatIDFK);
            ViewBag.CusIDFK = new SelectList(db.Customers, "Cus_Id", "Cus_Name", invoice.CusIDFk);
            ViewBag.PayIDFK = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Name", invoice.PayIDFk);
            return View(invoice);
        }

        public int BillCal(InvoiceGenerate b)
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
        public ActionResult logout()
        {
            Session.Abandon();
            return RedirectToAction("ClientLogin", "Home");
        }

        public ActionResult PostInvoice(int? id)
        {
            int userId = Convert.ToInt32(Session["CustomerID"]);
            var UserLogin = Session["CustomerName"];
            ViewBag.CusLogin = UserLogin;
            try
            {
                InvoiceGenerate invoice = db.InvoiceGenerates.Where(model => model.Customer.Cus_Name.ToString() == UserLogin.ToString()).FirstOrDefault();
                //OnlineOrder order = db.OnlineOrders.Where(model => model.Customer.Cus_Name.ToString() == UserLogin.ToString()).FirstOrDefault();

                if (invoice.Inv_Id.ToString() == null)
                {
                }                
                return View(invoice);
            }
            catch
            {
                TempData["NotPosted"] = "<script>alert('Your Invoice is Not Generated Yet!! Please Wait for the Admin Response')</script>";
                return RedirectToAction("ClientLogin", "Home");
            }
        }

        public ActionResult Sms()
        {
            //ViewBag.CusIDFK = new SelectList(db.Customers, "Cus_Id", "Phone");
            return View();
        }
        [HttpPost]
        public ActionResult Sms(string number, string txt, InvoiceGenerate invoice)
        {
            string MyUsername = "923013384672"; //Your Username At Sendpk.com 
            string MyPassword = "Wm03152993405"; //Your Password At Sendpk.com 
            string toNumber = number; //Recepient cell phone number with country code 
            string Masking = "SMS Alert"; //Your Company Brand Name 
            string MessageText = txt;
            string jsonResponse = SendSMS(Masking, toNumber, MessageText, MyUsername, MyPassword);
            ViewBag.str = jsonResponse;

            /*ViewBag.CusIDFK = new SelectList(db.Customers, "Cus_Id", "Phone", invoice.CusIDFk)*/
            ;
            return View();
        }
        public static string SendSMS(string Masking, string toNumber, string MessageText, string MyUsername, string MyPassword)
        {
            String URI = "http://sendpk.com" +
            "/api/sms.php?" +
            "username=" + MyUsername +
            "&password=" + MyPassword +
            "&sender=" + Masking +
            "&mobile=" + toNumber +
            "&message=" + Uri.UnescapeDataString(MessageText); // Visual Studio 10-15 
            //"&message=" + WebUtility.UrlEncode(MessageText);// Visual Studio 12 
            try
            {
                WebRequest req = WebRequest.Create(URI);
                WebResponse resp = req.GetResponse();
                var sr = new System.IO.StreamReader(resp.GetResponseStream());
                return sr.ReadToEnd().Trim();
            }
            catch (WebException ex)
            {
                var httpWebResponse = ex.Response as HttpWebResponse;
                if (httpWebResponse != null)
                {
                    switch (httpWebResponse.StatusCode)
                    {
                        case HttpStatusCode.NotFound:
                            return "404:URL not found :" + URI;
                            break;
                        case HttpStatusCode.BadRequest:
                            return "400:Bad Request";
                            break;
                        default:
                            return httpWebResponse.StatusCode.ToString();
                    }
                }
            }
            return null;
        }
    }
}
