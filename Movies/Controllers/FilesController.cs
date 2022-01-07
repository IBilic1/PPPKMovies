using System.Web.Mvc;

namespace Movies.Controllers
{
    public class FilesController : Controller
    {
        private readonly ModelContainer db = new ModelContainer();
        ~FilesController()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }

        // GET: Files
        public ActionResult Index(int id)
        {
            var uploadedPoster=db.UploadedPosters.Find(id);
            return File(uploadedPoster.Content,uploadedPoster.ContentType);
        }

        // DELETE
        public ActionResult Delete(int id)
        {
            db.UploadedPosters.Remove(db.UploadedPosters.Find(id));
            db.SaveChanges();
            return Redirect(Request.UrlReferrer.AbsolutePath);
        }
    }
}