using e_commerce_AYCS.Models;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;

namespace e_commerce_AYCS.Controllers
{
    public class CheckOutController : Controller
    {
        public IActionResult Index()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
{
new ProductEntity
{
Product = "Balotelli",
Rate = 100000,
Quanity= 1,
ImagePath = "img/image1.jpg"
},
new ProductEntity
{
Product = "Super pedro tovar",
Rate = 100000000,
Quanity= 1,
ImagePath = "img/image2.jpg"
}
};
            return View(productList);
        }
        public IActionResult OrderConfirmation()
        {
            var service = new SessionService();
            Session session = service.Get(TempData["Session"].ToString());
            if (session.PaymentStatus == "paid")
            {
                var transaction = session.PaymentIntentId.ToString(); return View("Success");
            }
            return View("Login");
        }
        public IActionResult Success()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult CheckOut()
        {
            List<ProductEntity> productList = new List<ProductEntity>();
            productList = new List<ProductEntity>
{
new ProductEntity
{
Product = "Balotelli",
Rate = 100000,
Quanity= 1,
ImagePath = "image1.jpg"
},
new ProductEntity
{
Product = "super pedro tovar",
Rate = 100000000,
Quanity= 1,
ImagePath = "image2.jpg"
},
new ProductEntity
{
Product = "alejandrito",
Rate = 3000,
Quanity= 2,
ImagePath = "image2.jpg"
}
};
            var domain = "http://localhost:5156/";
            var options = new SessionCreateOptions()
            {
                SuccessUrl = domain + $"CheckOut/OrderConfirmation",
                CancelUrl = domain + "CheckOut/Login",
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                CustomerEmail = "alejandritocantu5@gmail.com"
            };
            foreach (var item in productList)
            {
                var sessionListItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Rate * item.Quanity),
                        Currency = "inr",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.ToString(),
                        }
                    },
                    Quantity = item.Quanity
                };
                options.LineItems.Add(sessionListItem);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            TempData["Session"] = session.Id;
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}
