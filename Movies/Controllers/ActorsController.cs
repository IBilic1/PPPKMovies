using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Movies.Controllers
{
    public class ActorsController : Controller
    {
        private readonly ModelContainer db = new ModelContainer();

        ~ActorsController()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        // GET: Actors
        public ActionResult Index()
        {
            return View(db.People);
        }

        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            else
            {
                var actor = db.People.Find(id);
                if (actor==null)
                {
                    return new HttpNotFoundResult();
                }
                return View(actor);
            }
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        [HttpPost]
        public ActionResult Create([Bind(Include ="Firstname, Lastname")]Person person)
        {
            if (ModelState.IsValid)
            {
                Person actor = db.People.Add(person);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest); 
            }
            Person actor = db.People.Find(id);
            if (actor==null)
            {
                return new HttpNotFoundResult();
            }
            return View(actor);
        }

        // POST: Actors/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            Person personToUpdate = db.People.Find(id);
            if (TryUpdateModel(personToUpdate,"",new string[] { "Firstname","Lastname"}))
            {
                db.Entry(personToUpdate).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(personToUpdate);
        }

        // GET: Actors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Person actor = db.People.Find(id);
            if (actor == null)
            {
                return new HttpNotFoundResult();
            }
            return View(actor);
        }

        // POST: Actors/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            db.Movies.ToList().Where(movie => movie.People.ToList().Where(p => p.IDPerson == id) != null).ToList().ForEach(movie =>
            {
                db.People.Find(id).Movies.Remove(movie);
            });

            db.People.Remove(db.People.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
