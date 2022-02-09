using System.ComponentModel.DataAnnotations;

namespace PizzaOrder.Models
{
    public class Food
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public double Weight { get; set; }
        public string Image { get; set; }
        public int? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
