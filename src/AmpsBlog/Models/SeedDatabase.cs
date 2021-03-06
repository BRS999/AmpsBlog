﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmpsBlog.Models
{
    public class SeedDatabase
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;

        public SeedDatabase(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            await CreateRoles();
            await CreateUsersAsync();
            await CreatePostStatuses();
        }

        private async Task CreateRoles()
        {
            var roles = _context.Roles.Count();
            if (roles == 0)
            {
                _context.Roles.AddRange(
                new IdentityRole { Name = "Admin" },
                new IdentityRole { Name = "Author" },
                new IdentityRole { Name = "Registered" });

                await _context.SaveChangesAsync();
            }
        }

        private async Task CreateUsersAsync()
        {
            var user = _userManager.Users.Count();
            if (user == 0)
            {
                try
                {
                    var seedUser = new ApplicationUser { UserName = "admin@ampsblog.com", Email = "admin@ampsblog.com", FirstName = "AmpsBlog", LastName = "Admin" };
                    await _userManager.CreateAsync(seedUser, "P@ssw0rd!");

                    await _userManager.AddToRoleAsync(seedUser, "Admin");
                }
                catch (Exception ex)
                {

                    
                }
            }
        }

        private async Task CreatePostStatuses()
        {
            var postStatus = _context.PostStatuses.Count();
            if (postStatus == 0)
            {
                _context.PostStatuses.AddRange(
                new PostStatus { Status = "Draft" },
                new PostStatus { Status = "Publish" },
                new PostStatus { Status = "Archive" });

                await _context.SaveChangesAsync();
            }
        }
    }
}
