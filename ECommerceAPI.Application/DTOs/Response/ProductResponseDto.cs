using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Response
{
    public class ProductResponseDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string AddedDate { get; set; }
        public string UpdatedAt { get; set; }
    }
}
