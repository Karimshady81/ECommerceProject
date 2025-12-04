using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Domain.Entities
{
    public enum PaymentMethod
    {
        CreditCard,
        PayPal,
        Cash
    }
    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed
    }

    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public PaymentStatus PaymentStatus { get; set; } 
        public DateTime PaymentDate { get; set; }


        public Order Order { get; set; }
    }
}
