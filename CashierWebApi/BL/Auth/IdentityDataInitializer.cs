﻿using CashierDB.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CashierWebApi.BL.Auth
{
    public class IdentityDataInitializer
    {
        readonly IConfiguration configuration;
        readonly RoleManager<IdentityRole> roleManager;
        readonly UserManager<UserDbDto> userManager;

        public IdentityDataInitializer(IConfiguration configuration, 
            RoleManager<IdentityRole> roleManager, UserManager<UserDbDto> userManager)
        {
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public async Task InitializeAsync()
        {
            await AddRole("Admin");
            await AddRole("Manager");
            await AddRole("Cashier");
            var defautUsers = configuration
                .GetSection("DefaultUsers")
                .GetChildren();
            foreach (var user in defautUsers)
            {
                await AddUser(user["UserName"], user["Email"], user["Password"], user["Roles"].Split(","));
            }
        }

        async Task AddRole(string role)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        async Task AddUser(string username, string email, string password, IEnumerable<string> roles)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new UserDbDto()
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true,
                };
                await userManager.CreateAsync(user, password);
            }
            foreach (string role in roles)
            {
                await AddUserToRole(user, role);
            }
        }

        async Task AddUserToRole(UserDbDto user, string role)
        {
            if (user != null && !await userManager.IsInRoleAsync(user, role))
            {
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }

    public class SetupIdentityDataInitializer : IHostedService
    {
        private readonly IServiceProvider serviceProvider;

        public SetupIdentityDataInitializer(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetRequiredService<IdentityDataInitializer>();
                await initializer.InitializeAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
