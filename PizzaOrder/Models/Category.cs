using System.Collections.Generic;

namespace PizzaOrder.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<Food> Foods { get; set; }
    }
}
