using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Movies.Controllers
{
    public class ActorMovieController : Controller
    {
        private readonly ModelContainer db = new ModelContainer();

        ~ActorMovieController()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
        // GET: ActorMovie
        public ActionResult Create(int? movieId,int? actorId)
        {
            if (movieId==null || actorId==null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(movieId);
            Person person = db.People.Find(actorId);
            movie.People.Add(person);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.AbsolutePath);
        }
        public ActionResult Delete(int? movieId, int? actorId)
        {
            if (movieId == null || actorId == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(movieId);
            Person person = db.People.Find(actorId);
            movie.People.Remove(person);
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.AbsolutePath);
        }
    }
}