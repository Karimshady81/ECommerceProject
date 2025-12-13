using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Request
{
    public class CreateOrderRequestDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int quantity { get; set; }
        public string ShippingAddress { get; set; }
    }
}
