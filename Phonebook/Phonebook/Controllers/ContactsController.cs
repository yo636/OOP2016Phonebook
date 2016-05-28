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
    [Authorize]
    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: /Contacts/
        public ActionResult Index()
        {
            var id = User.Identity.GetUserId();
            var contactsByUserID = db.Contacts.Where(c => c.ApplicationUser.Id == id);
            //return View(db.Contacts.ToList());

            IEnumerable<Contacts> contactsByUserId = from c in db.Contacts
                                                     where c.ApplicationUser.Id == id
                                                     select c;

            return View(contactsByUserID.ToList());
        }

        // GET: /Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = db.Contacts.Find(id);
            string currentUserId = User.Identity.GetUserId();
            if (contacts.ApplicationUser.Id != currentUserId)
            {
                return RedirectToAction("Index");
            }
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // GET: /Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,Name,Family,Address")] Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                var id = User.Identity.GetUserId();
                contacts.ApplicationUser = db.Users.Find(id);
                db.Contacts.Add(contacts);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contacts);
        }

        // GET: /Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = db.Contacts.Find(id);
            string currentUserId = User.Identity.GetUserId();
            if (contacts.ApplicationUser.Id != currentUserId)
            {
                return RedirectToAction("Index");
            }
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // POST: /Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,Name,Family,Address")] Contacts contacts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contacts).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contacts);
        }

        // GET: /Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contacts contacts = db.Contacts.Find(id);
            string currentUserId = User.Identity.GetUserId();
            if (contacts.ApplicationUser.Id != currentUserId)
            {
                return RedirectToAction("Index");
            }
            if (contacts == null)
            {
                return HttpNotFound();
            }
            return View(contacts);
        }

        // POST: /Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contacts contacts = db.Contacts.Find(id);
            db.Contacts.Remove(contacts);
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
