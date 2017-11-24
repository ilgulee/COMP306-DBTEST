using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web.Mvc;
using COMP306_DBTEST.Models;

namespace COMP306_DBTEST.Controllers
{
    public class ShoppingCartController : Controller
    {
        private TestShopDBEntities db = new TestShopDBEntities();

        public ViewResult Index(string returnUrl)
        {
            return View(new ShoppingCartViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        private ShoppingCartModel GetCart()
        {
            ShoppingCartModel cart = (ShoppingCartModel)Session["Cart"];
            if (cart == null)
            {
                cart = new ShoppingCartModel();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public RedirectToRouteResult AddToCart(int productId, string returnUrl)
        {
            Product product = db.Products.SingleOrDefault(p => p.ProductId == productId);
            if (product != null)
            {
                GetCart().AddItem(product, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl)
        {
            GetCart().RemoveItem(productId);
            return RedirectToAction("Index", new { returnUrl });
        }

        public ActionResult ShippingInfo()
        {
            if (Session["UserId"] != null)
            {
                return View(new ShippingInfo());
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
            
        }

        [HttpPost]
        public ActionResult ShippingInfo(ShippingInfo shippingInfo)
        {

            if (ModelState.IsValid)
            {
                ShoppingCartModel cart = GetCart();
                cart.ShippingInfo = shippingInfo;
                return RedirectToAction("BillingInfo");
            }
            else
            {
                return View(shippingInfo);
            }



        }

        public ViewResult BillingInfo()
        {
            return View(new BillingInfo());
        }

        [HttpPost]
        public ViewResult BillingInfo(BillingInfo billingInfo)
        {
            if (ModelState.IsValid)
            {
                ShoppingCartModel cart = GetCart();
                cart.BillingInfo = billingInfo;
                ProcessOrder(cart);
                cart.Clear();
                return View("OrderComplete");
            }
            else
            {
                return View(billingInfo);
            }
        }

        private void ProcessOrder(ShoppingCartModel cart)
        {
            Customer customer;
            if (Session["UserId"] != null)
            {
                int id = Convert.ToInt32(Session["UserId"]);
                customer = db.Customers.Find(id);
                if (customer != null)
                {
                    customer.BillingAddress = cart.BillingInfo.Address;
                    customer.BillingPostalCode = cart.BillingInfo.PostalCode;
                    db.Customers.AddOrUpdate(customer);
                    db.SaveChanges();

                    Order order = new Order
                    {
                        CustomerId = customer.CustomerId,
                        OrderDate = DateTime.Now,
                        IsDelivery = cart.ShippingInfo.IsDelivery,
                        ShippingAddress = cart.ShippingInfo.Address,
                        ShippingPostalCode = cart.ShippingInfo.PostalCode
                    };
                    db.Orders.Add(order);
                    db.SaveChanges();

                    foreach (var item in cart.Items)
                    {
                        OrderLine orderLine = new OrderLine
                        {
                            OrderId = order.OrderId,
                            ProductId = item.Product.ProductId,
                            Quantity = item.Quantity
                        };
                        db.OrderLines.Add(orderLine);
                    }
                }
                db.SaveChanges();
            }

        }
    }
}