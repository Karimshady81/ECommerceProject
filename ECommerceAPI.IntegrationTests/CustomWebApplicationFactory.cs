using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ECommerceAPI.Infrastructure.Data;
using ECommerceAPI.Domain.Entities;

namespace ECommerceAPI.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Step 1: Remove the real database registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<AppDbContext>));

                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Step 2: Add in-memory database instead
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase");
                });

                // Step 3: Ensure database is created and seed test data
                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<AppDbContext>();

                    db.Database.EnsureCreated();

                    // Seed test data
                    SeedTestData(db);
                }
            });
        }

        protected void SeedTestData(AppDbContext context)
        {
            //Add initial test data that all tests can use

            context.Users.Add(new User
            {
                Id = 1,
                Email = "Test@mail.com",
                PasswordHash = "123456789",
                FirstName = "test",
                LastName = "test",
                Phone = "123456789"
            });

            context.Products.Add(new Product
            {
                Id = 1,
                Name = "Test Product",
                Price = 99.99m,
                StockQuantity = 10
            });

            context.Categories.Add(new Category
            {
                Id = 1,
                Name = "Test Category"
            });

            context.SaveChanges();
        }
    }
}
