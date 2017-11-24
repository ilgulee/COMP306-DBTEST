using System.Web.Mvc;
using COMP306_DBTEST.Models;

namespace COMP306_DBTEST.Controllers
{
    public class RegisterController : Controller
    {
        private TestShopDBEntities db=new TestShopDBEntities();
        // GET: Register
        public ActionResult Index()
        {
            
            return View(new Customer());
        }
        public RedirectToRouteResult Register(Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(customer);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Register");


        }
    }
}