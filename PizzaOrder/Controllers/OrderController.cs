using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using PizzaOrder.Data;
using PizzaOrder.Helpers;
using PizzaOrder.Models;
using PizzaOrder.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace PizzaOrder.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        private readonly IViewRender _viewRender;
        private readonly IToastNotification _toastNotification;
        public OrderController(ApplicationDbContext context, 
            IEmailSender emailSender,
            IViewRender viewRender,
            IToastNotification toastNotification)
        {
            _context = context;
            _emailSender = emailSender;
            _viewRender = viewRender;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            return View(_context.Orders.Include(nameof(Order.Food)));
        }
        public IActionResult AddOrder()
        {
            IEnumerable<SelectListItem> foods = _context.Foods.Select(e => new SelectListItem()
            { Text = e.Title, Value = e.Id.ToString() });
            ViewBag.FoodList = foods;

            return View();
        }
        private IEnumerable<Food> GetFoodsFromSession()
        {
            List<ShoppingOrder> products = HttpContext.Session.GetObject<List<ShoppingOrder>>("ShoppingOrders");
            if (products == null)
                products = new List<ShoppingOrder>();

            int[] productIds = products.Select(i => i.FoodId).ToArray();

            return _context.Foods.Where(c => productIds.Contains(c.Id));
        }
        [HttpPost]
        public IActionResult AddOrder(Order newOrder)
        {
            if (!ModelState.IsValid) return View();
            _context.Orders.Add(newOrder);
            _context.SaveChanges();

            string userEmail = User.Identity.Name;
            var items = GetFoodsFromSession();

            var html = this._viewRender.Render("Mails/OrderSummary", new OrderMail
            {
                UserName = userEmail,
                Foods = items,
                TotalPrice = items.Sum(i => i.Price)
            });

            _emailSender.SendEmailAsync(userEmail, "Order Summary", html);

            _toastNotification.AddSuccessToastMessage("Order was added. </br> Check your email");

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id <= 0) return NotFound();
            var orderToRemove = _context.Orders.Find(id);
            if (orderToRemove == null) return NotFound();
            _context.Orders.Remove(orderToRemove);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Order was deleted.");

            return RedirectToAction(nameof(Index)); // back to Index 
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0) return NotFound();

            var order = _context.Orders.Find(id);

            if (order == null) return NotFound();

            return View(order);
        }

        [HttpPost]
        public IActionResult Edit(Order obj)
        {
            if (!ModelState.IsValid) return View();

            _context.Orders.Update(obj);
            _context.SaveChanges();

            _toastNotification.AddSuccessToastMessage("Order was edited.");

            return RedirectToAction(nameof(Index));
        }
    }
}
