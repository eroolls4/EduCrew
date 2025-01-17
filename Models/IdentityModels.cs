﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using EduCrew.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace EduCrew.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Bio { get; set; }
        public string Description { get; set; }

        public string ProfileImage { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }




    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("aha", throwIfV1Schema: false)
        {
        }




        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<Post> Posts { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}