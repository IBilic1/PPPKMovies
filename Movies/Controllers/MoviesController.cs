using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Movies.Models;
using System;
using System.Collections.Generic;
using System.Web;

namespace Movies.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ModelContainer db=new ModelContainer();

        ~MoviesController(){
            if (db!=null)
            {
                db.Dispose();
            }
        }

        // GET: Movies
        public ActionResult Index()
        {
            return View(db.Movies);
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies
                .Include(m=>m.UploadedPosters)
                .SingleOrDefault(m=>m.IDMovie==id);
            if (movie==null)
            {
                return new HttpNotFoundResult();
            }
            return View(movie);
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            ViewBag.genres = Enum.GetValues(typeof(Genre));
            return View();
        }

        // POST: Movies/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Title, Genre, Duration")]Movie movie,IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                movie.UploadedPosters = new List<UploadedPosters>();
                foreach (var file in files)
                {
                    if (file!=null && file.ContentLength>0)
                    {
                        var poster = new UploadedPosters
                        {
                            Name = System.IO.Path.GetFileName(file.FileName),
                            ContentType = file.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            poster.Content = reader.ReadBytes(file.ContentLength);
                        }
                        movie.UploadedPosters.Add(poster);
                    }
                }

                db.Movies.Add(movie);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            ViewBag.genres = Enum.GetValues(typeof(Genre));
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies
                .Include(m => m.UploadedPosters)
                .SingleOrDefault(m => m.IDMovie == id);
            if (movie == null)
            {
                return new HttpNotFoundResult();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditConfirmed(int id, FormCollection collection,IEnumerable<HttpPostedFileBase> files)
        {
            Movie movieToUpdate = db.Movies.Find(id);
            if (TryUpdateModel(movieToUpdate,"",new string[] { "Title","Genre","Duration"}))
            {
                if (movieToUpdate.UploadedPosters!=null)
                {
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var poster = new UploadedPosters
                            {
                                Name = System.IO.Path.GetFileName(file.FileName),
                                ContentType = file.ContentType
                            };
                            using (var reader = new System.IO.BinaryReader(file.InputStream))
                            {
                                poster.Content = reader.ReadBytes(file.ContentLength);
                            }
                            movieToUpdate.UploadedPosters.Add(poster);
                        }
                    }
                }

                db.Entry(movieToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movieToUpdate);
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies
                .Include(m => m.UploadedPosters)
                .SingleOrDefault(m => m.IDMovie == id);
            if (movie == null)
            {
                return new HttpNotFoundResult();
            }
            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            db.UploadedPosters.RemoveRange(db.UploadedPosters.Where(m => m.MovieIDMovie == id));
            db.Movies.Remove(db.Movies.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Actors(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies
                .SingleOrDefault(m => m.IDMovie == id);
            if (movie == null)
            {
                return new HttpNotFoundResult();
            }
            List<Person> people = new List<Person>();
            
            foreach (var item in db.People)
            {
                if (!movie.People.Contains(item))
                {
                    people.Add(item);
                }
            }
            var other = db.People.ToList().Where(p => !movie.People.Contains(p)).ToList();
            ViewBag.otherActors = other ?? new List<Person>();
            TempData["movieID"] = id;
            TempData.Keep("movieID");
            return View(movie.People);
        }

        [HttpPost]
        [ActionName("Actors")]
        public ActionResult ActorsConfirmed([Bind(Include = "IDPerson")] Person person)
        {
            int? movieId = (int)TempData["movieID"];
            if (movieId == null || person?.IDPerson == 0)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(movieId);
            Person person_ = db.People.Find(person.IDPerson);
            movie.People.Add(person_);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
