using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaundryApplication.Models;
using Newtonsoft.Json;

namespace LaundryApplication.Controllers
{
    public class InvoiceGeneratesController : Controller
    {
        private LaundryDBEntities db = new LaundryDBEntities();

        // GET: InvoiceGenerates
        public ActionResult Index()
        {
            var invoiceGenerates = db.InvoiceGenerates.Include(i => i.Category).Include(i => i.Customer).Include(i => i.PaymentMethod);
            return View(invoiceGenerates.ToList());
        }

        // GET: InvoiceGenerates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceGenerate invoiceGenerate = db.InvoiceGenerates.Find(id);
            if (invoiceGenerate == null)
            {
                return HttpNotFound();
            }
            return View(invoiceGenerate);
        }

        // GET: InvoiceGenerates/Create
        public ActionResult Create()
        {
            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name");
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name");
            ViewBag.PayIDFk = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type");
            return View();
        }

        // POST: InvoiceGenerates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Inv_Id,CusIDFk,CatIDFK,PayIDFk,Cat_Des,Item_Quantity,DeliveryStatus,Invoice_Month,Invoice_Date,Barcode,Amount,Pay_Status")] InvoiceGenerate invoiceGenerate)
        {
            if (ModelState.IsValid)
            {
                db.InvoiceGenerates.Add(invoiceGenerate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name", invoiceGenerate.CatIDFK);
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name", invoiceGenerate.CusIDFk);
            ViewBag.PayIDFk = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type", invoiceGenerate.PayIDFk);
            return View(invoiceGenerate);
        }

        // GET: InvoiceGenerates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceGenerate invoiceGenerate = db.InvoiceGenerates.Find(id);
            if (invoiceGenerate == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name", invoiceGenerate.CatIDFK);
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name", invoiceGenerate.CusIDFk);
            ViewBag.PayIDFk = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type", invoiceGenerate.PayIDFk);
            return View(invoiceGenerate);
        }

        // POST: InvoiceGenerates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Inv_Id,CusIDFk,CatIDFK,PayIDFk,Cat_Des,Item_Quantity,DeliveryStatus,Invoice_Month,Invoice_Date,Barcode,Amount,Pay_Status")] InvoiceGenerate invoiceGenerate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoiceGenerate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatIDFK = new SelectList(db.Categories, "Cat_Id", "Cat_Name", invoiceGenerate.CatIDFK);
            ViewBag.CusIDFk = new SelectList(db.Customers, "Cus_Id", "Cus_Name", invoiceGenerate.CusIDFk);
            ViewBag.PayIDFk = new SelectList(db.PaymentMethods, "Pay_Id", "Pay_Type", invoiceGenerate.PayIDFk);
            return View(invoiceGenerate);
        }

        // GET: InvoiceGenerates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InvoiceGenerate invoiceGenerate = db.InvoiceGenerates.Find(id);
            if (invoiceGenerate == null)
            {
                return HttpNotFound();
            }
            return View(invoiceGenerate);
        }

        // POST: InvoiceGenerates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InvoiceGenerate invoiceGenerate = db.InvoiceGenerates.Find(id);
            db.InvoiceGenerates.Remove(invoiceGenerate);
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

        public ActionResult DashBoard()
        {
            List<DataPoint> dataPoint = new List<DataPoint>();
            var q = db.InvoiceGenerates.ToList();
            foreach (var item in q)
            {
                dataPoint.Add(new DataPoint(item.Invoice_Date, Convert.ToInt32(item.Amount)));
            }

            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoint);
            return View();
        }

        public ActionResult GarmentWiseCollection()
        {
            List<DataPoint> dataPoint = new List<DataPoint>();
            var q = db.InvoiceGenerates.ToList();
            //InvoiceGenerate inv = new InvoiceGenerate();
            //int Net;
            int NetTotalPP = 0;
            int NetTotalWei = 0;
            int NetTotalPk = 0;
            foreach (var item in q)
            {               
                if (item.Cat_Des == "Per Piece")
                {
                    //Net= Convert.ToInt32(db.InvoiceGenerates.Find(item.Amount));
                    NetTotalPP += Convert.ToInt32(item.Amount);
                }
                else if (item.Cat_Des == "Per Kg")
                {
                    //Net = Convert.ToInt32(db.InvoiceGenerates.Find(item.Amount));
                    NetTotalWei += Convert.ToInt32(item.Amount);
                }
                else
                {
                    //Net = Convert.ToInt32(db.InvoiceGenerates.Find(item.Amount));
                    NetTotalPk +=Convert.ToInt32( item.Amount);
                }                
            }
            dataPoint.Add(new DataPoint("Per Piece", NetTotalPP));
            dataPoint.Add(new DataPoint("Per Kg", NetTotalWei));
            dataPoint.Add(new DataPoint("Package", NetTotalPk));

            ViewBag.Garment = JsonConvert.SerializeObject(dataPoint);
            return View();
        }

        public ActionResult CustomerList(string SearchBy, string Search)
        {
            if (SearchBy == "Name")
            {
                return View(db.Customers.Where(x => x.Cus_Name.StartsWith(Search)).ToList());
            }
            return View(db.Customers.ToList());

        }

        public ActionResult TotalBusiness()
        {
            //InvoiceGenerate invoice = new InvoiceGenerate();
            List<DataPoint> dataPoint = new List<DataPoint>();
            var q = db.InvoiceGenerates.ToList();           
            var Amount = 0;
            foreach (var item in q)
            {
                Amount += Convert.ToInt32(item.Amount);

            }
            dataPoint.Add(new DataPoint("2016", 1000));
            dataPoint.Add(new DataPoint("2017", 2200));
            dataPoint.Add(new DataPoint("2018", 2800));
            dataPoint.Add(new DataPoint("2019", 3200));
            dataPoint.Add(new DataPoint("2020", Amount));

            ViewBag.TotalBussiness = JsonConvert.SerializeObject(dataPoint);
            return View();
        }
    }
}
