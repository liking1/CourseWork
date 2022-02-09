using System.Collections.Generic;

namespace PizzaOrder.Models
{
    public class OrderMail
    {
        public IEnumerable<Food> Foods { get; set; }
        public int TotalPrice { get; set; }
        public string UserName { get; set; }
    }
}
