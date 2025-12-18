using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ECommerceAPI.IntegrationTests
{
    //This runs my app in memory, not on localhost.
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
    }
}
