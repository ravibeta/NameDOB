using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NameDOB.Models;
using System.Text.RegularExpressions;

namespace NameDOB.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        NameDOBEntities _db;
        IComparer<string> comparer;
        public HomeController()
        {
            _db = new NameDOBEntities();
            comparer = new SpecialComparer();
        }
        public ActionResult Index()
        {
            ViewData["Message"] = "Welcome to ASP.NET MVC!";
            var sorted = _db.Individuals.ToList().OrderBy(x => x.DateOfBirth, comparer);
            ViewData.Model = sorted.Reverse();
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View(new Individual());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Add(FormCollection form)
        {
            var individualToAdd = new Individual();

            // Deserialize (Include white list!)
            TryUpdateModel(individualToAdd, new string[] { "Name", "DateOfBirth" }, form.ToValueProvider());

            // Validate
            if (String.IsNullOrEmpty(individualToAdd.Name))
                ModelState.AddModelError("Name", "Name is required!");
            if (String.IsNullOrEmpty(individualToAdd.DateOfBirth))
                ModelState.AddModelError("DateOfBirth", "DateOfBirth is required!");

            var error = individualToAdd.ValidateDateOfBirth();
            if (!String.IsNullOrEmpty(error))
                ModelState.AddModelError("DateOfBirth", error);


            // If valid, save Individual to Database
            if (ModelState.IsValid)
            {
                _db.AddToIndividuals(individualToAdd);
                _db.SaveChanges();
                return RedirectToAction("Add");
            }

            // Otherwise, reshow form
            return View(individualToAdd);
        }


    }
}
