using System.Linq;
using System.Net;
using System.Web.Mvc;
using COMP306_DBTEST.Models;

namespace COMP306_DBTEST.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {


            return View();
        }

        [HttpPost]
        public ActionResult Index(Customer customer)
        {
            using (TestShopDBEntities db = new TestShopDBEntities())
            {
                var usr = db.Customers.Where(u => u.FirstName == customer.FirstName && u.LastName == customer.LastName)
                    .FirstOrDefault();
                if (usr != null)
                {
                    Session["UserId"] = usr.CustomerId.ToString();
                    Session["FirstName"] = usr.FirstName;
                    return RedirectToAction("LoggedIn", new { id = usr.CustomerId });
                }
                else
                {
                    ModelState.AddModelError("", "user name not found.");
                }
            }
            return View();
        }

        public ActionResult LoggedIn(int? id)
        {
            Customer customer;
            if (Session["UserId"] != null)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                using (TestShopDBEntities db = new TestShopDBEntities())
                {
                    customer = db.Customers.Find(id);
                }

                if (customer == null)
                {
                    return HttpNotFound();
                }
                return View(customer);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
    }
}