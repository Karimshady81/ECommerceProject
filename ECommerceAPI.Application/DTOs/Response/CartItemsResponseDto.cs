using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Response
{
    public class CartItemsResponseDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }

        public decimal UnitPrice { get; set; }   // price *per item*
        public int Quantity { get; set; }

        public decimal TotalPrice => UnitPrice * Quantity; 
    }
}
