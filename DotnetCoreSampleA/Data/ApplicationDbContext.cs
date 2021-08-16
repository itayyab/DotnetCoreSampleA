using DotnetCoreSampleA.Models;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetCoreSample;
using DotnetCoreSampleA;
using System.Data.Entity.Core.Metadata.Edm;
using System.Globalization;
using Microsoft.CodeAnalysis;

namespace DotnetCoreSampleA.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions) : base(options, operationalStoreOptions)
        {
        }

        public DbSet<DotnetCoreSample.User> User { get; set; }
        public DbSet<DotnetCoreSampleA.Categories> Categories { get; set; }
    }
}
