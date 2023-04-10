using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Text;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreSampleA;
using DotnetCoreSampleA.Data;
using Moq;
using DotnetCoreSampleA.Models;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace UnitTests
{
   public class StartupTest
    {
       private readonly TestServer _server;
        private readonly HttpClient _client;
        IConfigurationRoot configuration;

 
        public StartupTest()
        {
         /*   var myConfiguration = new Dictionary<string, string>
{
    {"ApplicationSettings:JWT_Secret", "1234567890123456"},
    {"ApplicationSettings:Client_URL", "http://localhost:4200"}
};

            configuration = new ConfigurationBuilder()
          .SetBasePath(AppContext.BaseDirectory)
          //.AddJsonFile("appsettings.json")
          .AddInMemoryCollection(myConfiguration)
          .Build();

            WebHostBuilder webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureServices(s =>
            {
                s.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
               
               // s.Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));
            });
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<ApplicationUser>()
             .AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<ApplicationDbContext>();

            // s.Configure<ApplicationSettings>(configuration.GetSection("ApplicationSettings"));

            Startup startup = new Startup(configuration);
            startup.ConfigureServices(services);*/
            ///webHostBuilder.Configure(configuration);
           /* webHostBuilder.UseStartup<Startup>();

            _server = new TestServer(webHostBuilder);
            _client = _server.CreateClient();*/
        }
        //[Fact]
        //public void Startup_TEST()
        //{
        //    var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
        //    Assert.NotNull(webHost);
        //    // Assert
        //    //Assert.NotNull(_server);
        //    //Assert.NotNull(_client);
        //}
    }
}
