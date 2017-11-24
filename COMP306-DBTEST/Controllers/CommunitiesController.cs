using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using COMP306_DBTEST.Models;
using COMP306_DBTEST.Services;
using static System.Configuration.ConfigurationSettings;

namespace COMP306_DBTEST.Controllers
{
    public class CommunitiesController : Controller
    {
        private TestShopDBEntities db = new TestShopDBEntities();

        private ImageUploader _imageUploader;
       

        //public CommunitiesController()
        //{
        //     _imageUploader=new ImageUploader("com306-lab3-user-image");
        //}
        //public CommunitiesController(ImageUploader imageUploader)
        //{
        //    _imageUploader = imageUploader;
        //}

        // GET: Communities
        public ActionResult Index()
        {
            return View(db.Communities.ToList());
        }

        // GET: Communities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Community community = db.Communities.Find(id);
            if (community == null)
            {
                return HttpNotFound();
            }
            return View(community);
        }

        // GET: Communities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Communities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Community community,HttpPostedFileBase image)
        {
            var bucketName = WebConfigurationManager.AppSettings["COMP306-DBTEST:BucketName"];
             _imageUploader=new ImageUploader(bucketName);
            if (ModelState.IsValid)
            {
                db.Communities.Add(community);
                db.SaveChanges();
                if (image != null)
                {
                    var imageUrl = await _imageUploader.UploadImage(image, community.Id);
                    community.ImageUrl = imageUrl;
                    db.Communities.AddOrUpdate(community);
                    db.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }

            return View(community);
        }

        // GET: Communities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Community community = db.Communities.Find(id);
            if (community == null)
            {
                return HttpNotFound();
            }
            return View(community);
        }

        // POST: Communities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Community community)
        {
            if (ModelState.IsValid)
            {
                db.Entry(community).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(community);
        }

        // GET: Communities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Community community = db.Communities.Find(id);
            if (community == null)
            {
                return HttpNotFound();
            }
            return View(community);
        }

        // POST: Communities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Community community = db.Communities.Find(id);
            if (community != null) db.Communities.Remove(community);
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
