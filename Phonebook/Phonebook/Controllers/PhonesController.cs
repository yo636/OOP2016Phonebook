using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Phonebook.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Phonebook.Controllers
{
    public class PhonesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Phones/
        public ActionResult Index(int id2 = 0)
        {
            Contacts contact = db.Contacts.Find(id2);
            var currentUserId = User.Identity.GetUserId();
            if (contact.ApplicationUser.Id != currentUserId)
            {
                return RedirectToAction("Index", "Contacts");
            }

            var phones = db.Phones.Where(c => c.ContactId == id2);
            //var phones = db.Phones.Include(p => p.Contact);
            /*IEnumerable<Phone> phones = from c in db.Phones
                                        where c.ContactId == id2
                                        select c;*/
            ViewBag.id2 = id2;
            return View(phones.ToList());
        }

        // GET: /Phones/Details/5
        public ActionResult Details(int? id, int id2 = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Find(id);

            var currentUserId = User.Identity.GetUserId();
            if(phone.Contact.ApplicationUser.Id != currentUserId)
            {
                return RedirectToAction("Index","Contacts");
            }

            if (phone == null)
            {
                return HttpNotFound();
            }
            ViewData["id2"] = id2;
            ViewBag.id2 = id2;
            return View(phone);
        }

        // GET: /Phones/Create
        public ActionResult Create(int id2 = 0)
        {
            ViewBag.id2 = id2;
            ViewBag.contactName = db.Contacts.Find(id2).Name;

            ViewBag.ContactId = new SelectList(db.Contacts, "Id", "Name");
            return View();
        }

        // POST: /Phones/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Number,ContactId")] Phone phone, int id2 = 0)
        {
            if (ModelState.IsValid)
            {
                phone.Contact = db.Contacts.Find(id2);
                phone.ContactId = id2;
                db.Phones.Add(phone);
                db.SaveChanges();
                return RedirectToAction("Index", new { id2 = id2});
            }

            ViewBag.id2 = id2;
            ViewBag.contactName = db.Contacts.Find(id2).Name;
            ViewBag.ContactId = new SelectList(db.Contacts, "Id", "Name", phone.ContactId);
            return View(phone);
        }

        // GET: /Phones/Edit/5
        public ActionResult Edit(int? id, int id2 = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Find(id);
            if (phone == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "Id", "Name", phone.ContactId);
            ViewBag.id2 = id2;
            ViewBag.contactName = phone.Contact.Name;
            //ViewBag.contactName = db.Contacts.Find(id2).Name;
            return View(phone);
        }

        // POST: /Phones/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Number,ContactId")] Phone phone, int id2 = 0)
        {
            if (ModelState.IsValid)
            {
                phone.ContactId = id2;
                phone.Contact = db.Contacts.Find(id2);
                db.Entry(phone).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id2 = id2 });
            }
            ViewBag.ContactId = new SelectList(db.Contacts, "Id", "Name", phone.ContactId);
            ViewBag.id2 = id2;
            ViewBag.contactName = phone.Contact.Name;
            //ViewBag.contactName = db.Contacts.Find(id2).Name;
            return View(phone);
        }

        // GET: /Phones/Delete/5
        public ActionResult Delete(int? id, int id2 = 0)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phone phone = db.Phones.Find(id);
            ViewBag.id2 = id2;
            if (phone == null)
            {
                return HttpNotFound();
            }
            return View(phone);
        }

        // POST: /Phones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int id2 = 0)
        {
            Phone phone = db.Phones.Find(id);
            db.Phones.Remove(phone);
            db.SaveChanges();
            return RedirectToAction("Index", new { id2 = id2 });
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
