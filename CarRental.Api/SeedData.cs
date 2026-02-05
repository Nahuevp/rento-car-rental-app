using CarRental.Api.Data;
using CarRental.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRental.Api
{
    public static class SeedData
    {
        public static void Initialize(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Users.Any())
                return; // Ya hay usuarios

            var hasher = new PasswordHasher<User>();

            var alice = new User
            {
                Username = "alice"
            };
            alice.PasswordHash = hasher.HashPassword(alice, "1234");

            var bob = new User
            {
                Username = "bob"
            };
            bob.PasswordHash = hasher.HashPassword(bob, "1234");

            context.Users.AddRange(alice, bob);
            context.SaveChanges();
        }
    }
}
