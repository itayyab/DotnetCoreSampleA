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

namespace UnitTests
{
   public class StartupTest
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        IConfigurationRoot configuration;


        public StartupTest()
        {
          //  configuration = new ConfigurationBuilder()
          //.SetBasePath(AppContext.BaseDirectory)
          //.AddJsonFile("appsettings.json")
          //.Build();

          //  WebHostBuilder webHostBuilder = new WebHostBuilder();
          //  webHostBuilder.ConfigureServices(s =>
          //  {
          //      s.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
          //  });


          //  webHostBuilder.UseStartup<Startup>();

          //  _server = new TestServer(webHostBuilder);
          //  _client = _server.CreateClient();
        }
        [Fact]
        public void Startup_TEST()
        {
            var webHost = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            Assert.NotNull(webHost);
            // Assert
            //Assert.NotNull(_server);
            //Assert.NotNull(_client);
        }
    }
}
