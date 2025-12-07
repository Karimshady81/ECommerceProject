using ECommerceAPI.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Application.Helpers
{
    internal class JwtTokenGenerator : IJwtService
    {
        public string GenerateToken(string userId, string email)
        {
            throw new NotImplementedException();
        }
    }
}
