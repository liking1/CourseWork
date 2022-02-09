using System.Collections.Generic;

namespace PizzaOrder.Models.ViewModel
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public Food Foods { get; set; }
        public IEnumerable<Category> Categories { get; set; }
    }
}
