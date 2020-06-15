using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Blogger.Help;
using Blogger.Models;

namespace Blogger.Areas.admin.Controllers
{
    public class CountriesController : Controller
    {
        private BlogTravelEntities1 db = new BlogTravelEntities1();

        // GET: admin/Countries
        public ActionResult Index(long? id = null)
        {
            getasia(id);
            return View();
        }
        public void getasia(long? selectedId = null)
        {
            ViewBag.asia = new SelectList(db.asias.Where(x => x.hide == true).OrderBy(x => x.order), "id", "name", selectedId);
        }
        public ActionResult getcountry(long? id)
        {
            if (id == null)
            {
                var v = db.Countries.OrderBy(x => x.order).ToList();
                return PartialView(v);
            }
            var m = db.Countries.Where(x => x.asiaId == id).OrderBy(x => x.order).ToList();
            return PartialView(m);
        }
        // GET: admin/Countries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // GET: admin/Countries/Create
        public ActionResult Create()
        {
            ViewBag.asiaId = new SelectList(db.asias, "id", "name");
            return View();
        }

        // POST: admin/Countries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "id,name,img,description,meta,hide,order,datebigin,asiaId,description1")] Country country, HttpPostedFileBase img)
        {
            try
            {
                var path = "";
                var filename = "";
                if (ModelState.IsValid)
                {
                    if (img != null)
                    {
                        filename = DateTime.Now.ToString("dd-MM-yy=hh-mm-ss-") + img.FileName;
                        path = Path.Combine(Server.MapPath("~/Content/images"), filename);
                        img.SaveAs(path);
                        country.img = filename;
                    }
                    else
                    {
                        country.img = "hagiang2.jpg";
                    }
                    country.datebigin = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                    country.meta = Functions.ConvertToUnSign(country.meta);
                    country.order = getMaxOrder(country.asiaId);
                    db.Countries.Add(country);
                    db.SaveChanges();
                    return RedirectToAction("Index", "country", new { id = country.asiaId });
                }
            }
            catch (DbEntityValidationException e)
            {
                throw e;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(country);
        }
        public int getMaxOrder(long? asiaId)
        {
            if (asiaId == null)
                return 1;
            return db.Countries.Where(x => x.asiaId == asiaId).Count();
        }
        // GET: admin/Countries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            ViewBag.asiaId = new SelectList(db.asias, "id", "name", country.asiaId);
            return View(country);
        }

        // POST: admin/Countries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,img,description,meta,hide,order,datebigin,asiaId,description1")] Country country)
        {
            if (ModelState.IsValid)
            {
                db.Entry(country).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.asiaId = new SelectList(db.asias, "id", "name", country.asiaId);
            return View(country);
        }

        // GET: admin/Countries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Country country = db.Countries.Find(id);
            if (country == null)
            {
                return HttpNotFound();
            }
            return View(country);
        }

        // POST: admin/Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Country country = db.Countries.Find(id);
            db.Countries.Remove(country);
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
