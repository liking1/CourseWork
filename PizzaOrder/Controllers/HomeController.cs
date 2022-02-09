using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using PizzaOrder.Data;
using PizzaOrder.Helpers;
using PizzaOrder.Models;
using PizzaOrder.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PizzaOrder.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString)
        {
            var foods = from e in _context.Foods
                         select e;

            if (!String.IsNullOrEmpty(searchString))
            {
                foods = foods.Where(s => s.Title!.Contains(searchString));
            }
            
            return View(foods);
        }

        public IActionResult FoodDetails(int id)
        {
            Food food = _context.Foods.Find(id);
            if (food == null) return NotFound();

            _context.Entry(food).Reference(nameof(Food.Category)).Load();

            bool isAddedToCart = false;
            List<Order> products = HttpContext.Session.GetObject<List<Order>>("ShoppingOrders");
            if (products != null)
            {
                if (products.FirstOrDefault(i => i.FoodId == id) != null)
                    isAddedToCart = true;
            }
            return View(new FoodDetailsViewModel() { Food = food, IsAddedToCart = isAddedToCart });
        }

        public IActionResult AddToCart(int id)
        {
            List<Order> products = HttpContext.Session.GetObject<List<Order>>("ShoppingOrders");
            if (products == null)
            {
                products = new List<Order>();
            }

            products.Add(new Order() { FoodId = id, Name = "Some data" });
            HttpContext.Session.SetObject("ShoppingOrders", products);

            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveFromCart(int id)
        {
            List<Order> products = HttpContext.Session.GetObject<List<Order>>("ShoppingOrders");
            if (products != null)
            {
                products.Remove(products.FirstOrDefault(i => i.FoodId == id));
            }

            HttpContext.Session.SetObject("ShoppingOrders", products);
            
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Contacts()
        {
            return View();
        }
        public IActionResult AboutCreator() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
