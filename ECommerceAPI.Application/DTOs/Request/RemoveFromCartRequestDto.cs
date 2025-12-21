using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Request
{
    public class RemoveFromCartRequestDto
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
}
