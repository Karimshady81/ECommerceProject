using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.DTOs.Request
{
    public class RegisterUserRequestDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(8), PasswordPropertyText]
        public string Password { get; set; }

        [Required, Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        [Required, Phone]
        public string PhoneNumber { get; set; }
    }
}
