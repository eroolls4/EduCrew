using EduCrew.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EduCrew
{
    public class RoleInitializer
    {
        public static void InitializeRolesAndAdminUser()
        {
            using (var context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                // Create roles if they don't exist
                if (!roleManager.RoleExists("Admin"))
                {
                    var role = new IdentityRole("Admin");
                    roleManager.Create(role);
                }

                if (!roleManager.RoleExists("Member"))
                {
                    var role = new IdentityRole("Member");
                    roleManager.Create(role);
                }

                // Create the admin user if it doesn't exist
                var adminEmail = "admin@gmail.com";
                var adminUser = userManager.FindByEmail(adminEmail);

                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail
                    };
                    var adminResult = userManager.Create(adminUser, "Admin@123"); // Default password

                    if (adminResult.Succeeded)
                    {
                        // Assign Admin role
                        userManager.AddToRole(adminUser.Id, "Admin");
                    }
                }
            }
        }

    }
      
 }