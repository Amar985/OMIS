using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineMusic.DataAccess.Repository.IRepository;
using OnlineMusic.Models;
using OnlineMusicStore.Utility;
using System.Diagnostics;
using System.Security.Claims;

namespace OnlineMusicStore.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                HttpContext.Session.SetInt32(SD.SesionCart,
                _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }
            IEnumerable<Product> productlist = _unitOfWork.Product.GetAll(includeProperties:"Category");
            return View(productlist);
        }
        public IActionResult Details(int productId)
        {
            ShoppingCart cart = new ShoppingCart()
            {
                Product = _unitOfWork.Product.Get(u => u.Id == productId),
                Count=1,
                ProductId=productId
            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]

        // Adding items to the cart
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingCart.ApplicationUserId = userId;

            var product = _unitOfWork.Product.Get(u => u.Id == shoppingCart.ProductId);

            if (shoppingCart.Count < 1)
            {
                TempData["ErrorMessage"] = "The count must be at least 1.";
                return RedirectToAction("Details", new { productId = shoppingCart.ProductId });
            }

            if (product == null || product.stock_quantity < shoppingCart.Count)
            {
                TempData["ErrorMessage"] = $"The product '{product?.InstrumentName}' is not available in the requested quantity.";
                return RedirectToAction("Details", new { productId = shoppingCart.ProductId });
            }

            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u=>u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);
            
            if(cartFromDb != null)
            {
                //shopping cart exists
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                //add cart record
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();
                int cartCount = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count();
                HttpContext.Session.SetInt32(SD.SesionCart, cartCount);
            }
            TempData["Success"] = "Cart Added Successfully";
            
            
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
