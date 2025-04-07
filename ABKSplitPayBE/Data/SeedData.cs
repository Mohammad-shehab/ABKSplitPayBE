using ABKSplitPayBE.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ABKSplitPayBE.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            // Ensure the database is created
            await context.Database.EnsureCreatedAsync();

            // Seed Roles
            string adminRole = "Admin";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            string userRole = "User";
            if (!await roleManager.RoleExistsAsync(userRole))
            {
                await roleManager.CreateAsync(new IdentityRole(userRole));
            }

            // Seed Admin User
            string adminUserName = "admin";
            string adminEmail = "admin@abksplitpay.com";
            string adminPassword = "Admin_135";

            var adminUser = await userManager.FindByNameAsync(adminUserName);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminUserName,
                    Email = adminEmail,
                    FullName = "Admin User",
                    ProfilePictureUrl = "https://images.unsplash.com/photo-1511367461989-f85a21fda167?q=80&w=2070&auto=format&fit=crop", // Real image
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create admin user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(adminUser, adminRole);
                Console.WriteLine("Admin user created successfully.");
            }

            // Seed Test User
            string testUserName = "testuser";
            string testEmail = "testuser@abksplitpay.com";
            string testPassword = "Test@123";

            var testUser = await userManager.FindByNameAsync(testUserName);
            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = testUserName,
                    Email = testEmail,
                    FullName = "Test User",
                    ProfilePictureUrl = "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?q=80&w=2070&auto=format&fit=crop", // Real image
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(testUser, testPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Failed to create test user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(testUser, userRole);
                Console.WriteLine("Test user created successfully.");
            }
        }
    }
}