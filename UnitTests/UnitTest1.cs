using DotnetCoreSampleA;
using DotnetCoreSampleA.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using DotnetCoreSampleA.Data;
using DotnetCoreSampleA.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using DotnetCoreSample;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace UnitTests
{
    public class UnitTest1
    {
       /* private readonly TestServer _server;
        private readonly HttpClient _client;

        public UnitTest1()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory)
           .AddJsonFile("appsettings.json")
           .Build();
            
            var builder = new WebHostBuilder().UseEnvironment("Development").ConfigureServices(s =>
            {
                s.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
                //s.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                //.AddEntityFrameworkStores<ApplicationDbContext>();

                //s.AddIdentityServer()
                //    .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();
                //s.AddAuthentication()
                //.AddIdentityServerJwt();
            })
            .UseStartup(typeof(Startup));

            _server = new TestServer(builder);

            _client = _server.CreateClient();
        }*/
       /* public UnitTest1()
        {
            // Arrange
            //_server = new TestServer(new WebHostBuilder()
            //    .UseEnvironment("Development")
            //   .UseStartup<Startup>());

            IConfigurationRoot configuration = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory)
           //.AddJsonFile("appsettings.json")
           .Build();

            WebHostBuilder webHostBuilder = new WebHostBuilder();
            webHostBuilder.ConfigureServices(s => {
                s.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
                s.AddIdentityServer()
                .AddAspNetIdentity<ApplicationUser>();
                s.AddAuthentication()
                .AddIdentityServerJwt();
               
            }
            );
           
           
            
            //webHostBuilder.ConfigureServices(s=>s.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseSqlServer(
            //        Configuration.GetConnectionString("DefaultConnection"))));
            webHostBuilder.UseStartup<Startup>();

             _server = new TestServer(webHostBuilder);

            //Client = server.CreateClient();
            _client = _server.CreateClient();
        }
       */
        //[Fact]
        //public void PassingTest()
        //{
        //    Assert.Equal(4, Add(2, 2));
        //}

        //[Fact]
        //public void FailingTest()
        //{
        //    Assert.Equal(5, Add(2, 2));
        //}

        //int Add(int x, int y)
        //{
        //    return x + y;
        //}
        //[Fact]
        //public async Task GetUser()
        //{
        //    // Act
        //    var response = await _client.GetAsync("/api/Users/1");
        //    //response.EnsureSuccessStatusCode();
        //    //var responseString = await response.Content.ReadAsStringAsync();
        //    //// Assert
        //    //Assert.Equal("Hello World!", responseString);
        //}
    }
}
