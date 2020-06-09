using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blogger.Models;
namespace Blogger.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        BlogTravelEntities5 _db = new BlogTravelEntities5();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getasia()
        {
            var v = from t in _db.asias
                    where t.hide == true
                    orderby t.order ascending
                    select t;
            return PartialView(v.ToList());
        }

    }
}