using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using PizzaOrder.Data;
using PizzaOrder.Helpers;
using PizzaOrder.Models;
using PizzaOrder.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace PizzaOrder.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IViewRender _viewRender;
        private readonly IHttpContextAccessor _session;

        public CartController(ApplicationDbContext context, 
            IEmailSender emailSender, 
            IViewRender viewRender,
            IHttpContextAccessor session)
        {
            _context = context;
            _emailSender = emailSender;
            _viewRender = viewRender;
            _session = session;
        }
        
        [AllowAnonymous]
        public IActionResult Index()
        {
            CartListViewModel viewModel = new CartListViewModel()
            {
                Foods = GetFoodsFromSession()
            };
            return View(viewModel);
        }
        private IEnumerable<Food> GetFoodsFromSession()
        {
            List<ShoppingOrder> products = HttpContext.Session.GetObject<List<ShoppingOrder>>("ShoppingOrders");
            if (products == null)
                products = new List<ShoppingOrder>();

            int[] productIds = products.Select(i => i.FoodId).ToArray();

            return _context.Foods.Where(c => productIds.Contains(c.Id));
        }

        public IActionResult ClearCart()
        {
            _session.HttpContext.Session.Clear();
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        // no need
        public IActionResult Confirm()
        {
            //string userEmail = User.Identity.Name;
            //var items = GetFoodsFromSession();

            //var html = this._viewRender.Render("Mails/OrderSummary", new OrderSummaryMail
            //{
            //    UserName = userEmail,
            //    Foods = items,
            //    TotalPrice = items.Sum(i => i.Price)
            //});

            //_emailSender.SendEmailAsync(userEmail, "Order Summary", html);

            return View();
        }
    }
}
