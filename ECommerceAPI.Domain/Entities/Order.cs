using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    internal class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderNumber { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } /// "Pending", "Processing", "Shipped", "Delivered", "Cancelled"
        public string ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; }
    }
}
