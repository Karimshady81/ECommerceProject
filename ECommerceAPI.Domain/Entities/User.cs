using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public enum UserRole
    {
        Customer,
        Admin
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public UserRole Role { get; set; } = UserRole.Customer;
        public DateTime CreatedAt { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
