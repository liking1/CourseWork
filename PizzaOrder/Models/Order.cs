using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PizzaOrder.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string Phone { get; set; }
        public bool CashSettlement { get; set; } = false;
        public bool NonCashCalculation { get; set; } = true;
        public int FoodId { get; set; }
        public Food Food { get; set; }
        public bool IsAddedToCart { get; set; }
        public ConfirmType Types { get; set; } = ConfirmType.Expected;
        public enum ConfirmType
        {
            Expected,
            [Description("In Progress")]
            InProgress,
            Completed
        }
    }
}
