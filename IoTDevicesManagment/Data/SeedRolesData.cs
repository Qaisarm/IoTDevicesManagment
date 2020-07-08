using IoTDevicesManagment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IoTDevicesManagment.Data
{
    public class SeedRolesData
    {
        public static async Task IntializeAsync(IServiceProvider service)
        {
            var options = service.GetRequiredService<DbContextOptions<ApplicationDbContext>>();

            using (var context = new ApplicationDbContext(options))
            {
                var userManager = service.GetRequiredService<UserManager<ApplicationUsers>>();
                var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

                var roleNames = new[] { "Admin", "User" };

                await SeedRoles(roleManager, roleNames);

                var adminEmail = "admin@domain.com";
                var adminPassword = "BygMig123!";

                await SeedAdmin(userManager, roleManager, roleNames, adminEmail, adminPassword);
            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager, string[] roleNames)
        {
            foreach (var name in roleNames)
            {
                if (await roleManager.RoleExistsAsync(name)) continue;

                var role = new IdentityRole { Name = name };
                var result = await roleManager.CreateAsync(role);

                if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
            }
        }
        private static async Task SeedAdmin(UserManager<ApplicationUsers> userManager, RoleManager<IdentityRole> roleManager, string[] roleNames, string AdminEmail, string AdminPassword)
        {
            var foundUser = await userManager.FindByEmailAsync(AdminEmail);

            if (foundUser != null) return;

            var user = new ApplicationUsers
            {
                UserName = AdminEmail,
                Email = AdminEmail

            };

            var addUserResult = await userManager.CreateAsync(user, AdminPassword);

            if (!addUserResult.Succeeded) throw new Exception(string.Join("\n", addUserResult.Errors));


            var adminUser = await userManager.FindByNameAsync(AdminEmail);

            foreach (var role in roleNames)
            {
                if (role == "Admin")
                {
                    if (await userManager.IsInRoleAsync(adminUser, role)) continue;

                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, role);

                    if (!addToRoleResult.Succeeded) throw new Exception(string.Join("\n", addToRoleResult.Errors));
                }
            }
        }

    }
}