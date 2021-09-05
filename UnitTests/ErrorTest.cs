using DotnetCoreSampleA.Pages;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace UnitTests
{
   public class ErrorTest
    {
        [Fact]
        public void ErrorTestReqID()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<ErrorModel>();
            //arrange
            var pageModel = new ErrorModel(logger);
            pageModel.RequestId = "123";

            var req = pageModel.ShowRequestId;
            Assert.True(req);
        }
       /* [Fact]
        public void ErrorTestOnGet()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();

            var factory = serviceProvider.GetService<ILoggerFactory>();

            var logger = factory.CreateLogger<ErrorModel>();
            //arrange
            var pageModel = new ErrorModel(logger);
            pageModel.RequestId = "123";

            var req = pageModel.ShowRequestId;
            pageModel.OnGet();
            Assert.True(req);
        }*/
    }
}
