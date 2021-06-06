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
    public class PaymentMethodsController : Controller
    {
        private LaundryDBEntities db = new LaundryDBEntities();

        // GET: PaymentMethods
        public ActionResult Index()
        {
            return View(db.PaymentMethods.ToList());
        }

        // GET: PaymentMethods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = db.PaymentMethods.Find(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PaymentMethods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Pay_Id,Pay_Type,Pay_Des")] PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                db.PaymentMethods.Add(paymentMethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(paymentMethod);
        }

        // GET: PaymentMethods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = db.PaymentMethods.Find(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Pay_Id,Pay_Type,Pay_Des")] PaymentMethod paymentMethod)
        {
            if (ModelState.IsValid)
            {
                db.Entry(paymentMethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(paymentMethod);
        }

        // GET: PaymentMethods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PaymentMethod paymentMethod = db.PaymentMethods.Find(id);
            if (paymentMethod == null)
            {
                return HttpNotFound();
            }
            return View(paymentMethod);
        }

        // POST: PaymentMethods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PaymentMethod paymentMethod = db.PaymentMethods.Find(id);
            db.PaymentMethods.Remove(paymentMethod);
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
    }
}
