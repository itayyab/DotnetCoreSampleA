﻿using DotnetCoreSampleA.Models;

using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetCoreSampleA;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using Microsoft.CodeAnalysis;
using Duende.IdentityServer.EntityFramework.Options;

namespace DotnetCoreSampleA.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }
        public DbSet<DotnetCoreSampleA.Category> Categories { get; set; }
        public DbSet<DotnetCoreSampleA.Models.Product> Product { get; set; }
        public DbSet<DotnetCoreSampleA.Models.Cart> Cart { get; set; }
        public DbSet<DotnetCoreSampleA.Models.CartDetails> CartDetails { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
