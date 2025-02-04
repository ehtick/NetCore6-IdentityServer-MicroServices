﻿using IdentityModel;
using MicroServices.IdentityServer.Configuration;
using MicroServices.IdentityServer.Model;
using MicroServices.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MicroServices.IdentityServer.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly SqlServerContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(
            SqlServerContext context,
            UserManager<ApplicationUser> user,
            RoleManager<IdentityRole> role
        )
        {
            _context = context;
            _user = user;
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null) return;

            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

            ApplicationUser admin = new()
            {
                UserName = "felipe-admin",
                Email = "felipe-admin@felipe.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (19) 12245-6789",
                FirstName = "Felipe",
                LastName = "Admin"
            };

            _user.CreateAsync(admin, "Felipe123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

            _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
            }).GetAwaiter().GetResult();

            ApplicationUser client = new()
            {
                UserName = "felipe-client",
                Email = "felipe-client@felipe.com.br",
                EmailConfirmed = true,
                PhoneNumber = "+55 (19) 12245-6789",
                FirstName = "Felipe",
                LastName = "Client"
            };

            _user.CreateAsync(client, "Felipe123$").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

            _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
            }).GetAwaiter().GetResult();
        }
    }
}
