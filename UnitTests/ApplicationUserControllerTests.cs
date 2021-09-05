using DotnetCoreSampleA;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using DotnetCoreSampleA.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Mvc;

using Xunit.Abstractions;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using DotnetCoreSampleA.Controllers;
using DotnetCoreSampleA.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Identity;

namespace UnitTests
{
    [TestClass]
    public class ApplicationUserControllerTests
    {
        private readonly ITestOutputHelper output;
        private readonly ApplicationDbContext applicationDbContext;

        public ApplicationUserControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .EnableSensitiveDataLogging()
                 .Options;
            OperationalStoreOptions storeOptions = new OperationalStoreOptions
            {
                //populate needed members
            };

            IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
            applicationDbContext = new ApplicationDbContext(options, operationalStoreOptions);
            //Seed();
        }
        [Fact]
        public async Task ApplicationUser_Register_Test()
        {




            ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
            var store = new Mock<IUserStore<ApplicationUser>>();

            var user = new ApplicationUser { FullName = "tayyab", UserName = "tayyab", Email = "tayyab", Id = "12345678" };
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Setup(p => p.FindByNameAsync("tayyab")).Returns(Task.FromResult(user));
            mgr.Setup(p => p.CheckPasswordAsync(user, "1234")).Returns(Task.FromResult(true));

            mgr.Setup(p => p.GetRolesAsync(user)).Returns(Task.FromResult((IList<string>)new List<string>() { "Admin" }));


            //   var mockUser = new Mock<UserManager<ApplicationUser>>();
            /*      mgr.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
      .ReturnsAsync(new IdentityUser { });
                  mockUser.Setup(userManager => userManager.IsInRoleAsync(It.IsAny<IdentityUser>(), "SweetTooth"))
                      .ReturnsAsync(true);*/
            var applicationSettings = new ApplicationSettings { JWT_Secret = "1234567890123456", Client_URL = "http://localhost:4200" };
            IOptions<ApplicationSettings> operationalStoreOptions = Options.Create(applicationSettings);

            var controller = new ApplicationUserController(mgr.Object, null, operationalStoreOptions);

            Xunit.Assert.NotNull(controller);
            var applicationSettingsx = new ApplicationUserModel { FullName = "tayyab", UserName = "tayyab", Email = "tayyab", Role = "Admin", Password = "1234" };
            var actionResult = await controller.PostApplicationUser(applicationSettingsx);
            var item = (Object)actionResult;
            Xunit.Assert.NotNull(item);
            applicationSettingsx = new ApplicationUserModel { FullName = null, UserName = null, Email = null, Role = null, Password = null };
            actionResult = await controller.PostApplicationUser(applicationSettingsx);
            Xunit.Assert.NotNull(actionResult);
            var login = new LoginModel { UserName = "tayyabas", Password = "1234aa" };
            actionResult = await controller.Login(login);
            Xunit.Assert.IsType<BadRequestObjectResult>(actionResult);
            Xunit.Assert.NotNull(actionResult);

            login = new LoginModel { UserName = "tayyab", Password = "1234" };
            actionResult = await controller.Login(login);

            Xunit.Assert.NotNull(actionResult);


            /* var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
             List<CartDetails> cartx = new List<CartDetails>();
             cartx.Add(cartdetils1);
             var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

             var actionResult = await controller.PostCart(manager);
             var item = (Cart)((CreatedAtActionResult)actionResult.Result).Value;
             output.WriteLine("This is output from {0}", item.Cart_id);
             Xunit.Assert.IsType<Cart>(item);
             Xunit.Assert.Equal(manager.Cart_id, item.Cart_id);*/
            //Xunit.Assert.Equal(cat.Pr_name, item.Pr_name);

        }
        [Fact]
        public async Task ApplicationUser_Login_Test_BadRequest()
        {




            ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
            var store = new Mock<IUserStore<ApplicationUser>>();

            var user = new ApplicationUser { FullName = "tayyab", UserName = "tayyab", Email = "tayyab", Id = "12345678" };
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Setup(p => p.FindByNameAsync("tayyab")).Returns(Task.FromResult(user));
            mgr.Setup(p => p.CheckPasswordAsync(user, "1234")).Returns(Task.FromResult(true));

            mgr.Setup(p => p.GetRolesAsync(user)).Returns(Task.FromResult((IList<string>)new List<string>() { "Admin" }));


            //   var mockUser = new Mock<UserManager<ApplicationUser>>();
            /*      mgr.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
      .ReturnsAsync(new IdentityUser { });
                  mockUser.Setup(userManager => userManager.IsInRoleAsync(It.IsAny<IdentityUser>(), "SweetTooth"))
                      .ReturnsAsync(true);*/
            var applicationSettings = new ApplicationSettings { JWT_Secret = "1234567890123456", Client_URL = "http://localhost:4200" };
            IOptions<ApplicationSettings> operationalStoreOptions = Options.Create(applicationSettings);

            var controller = new ApplicationUserController(mgr.Object, null, operationalStoreOptions);


            var login = new LoginModel { UserName = "tayyabas", Password = "1234aa" };
            var actionResult = await controller.Login(login);
            Xunit.Assert.IsType<BadRequestObjectResult>(actionResult);
            Xunit.Assert.NotNull(actionResult);

        }


        [Fact]
        public async Task ApplicationUser_Register_Exception_Test()
        {




            ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
            var store = new Mock<IUserStore<ApplicationUser>>();

            var user = new ApplicationUser { FullName = "tayyab", UserName = "tayyab", Email = "tayyab", Id = "12345678" };
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Setup(p => p.FindByNameAsync("tayyab")).Returns(Task.FromResult(user));
            mgr.Setup(p => p.CheckPasswordAsync(user, "1234")).Returns(Task.FromResult(true));

            mgr.Setup(p => p.GetRolesAsync(user)).Returns(Task.FromResult((IList<string>)new List<string>() { "Admin" }));
            mgr.Setup(p => p.CreateAsync(user, "1234")).Throws(new Exception());
            mgr.Setup(p => p.AddToRoleAsync(user, "1234")).Throws(new Exception());
         


            //   var mockUser = new Mock<UserManager<ApplicationUser>>();
            /*      mgr.Setup(userManager => userManager.FindByIdAsync(It.IsAny<string>()))
      .ReturnsAsync(new IdentityUser { });
                  mockUser.Setup(userManager => userManager.IsInRoleAsync(It.IsAny<IdentityUser>(), "SweetTooth"))
                      .ReturnsAsync(true);*/
            var applicationSettings = new ApplicationSettings { JWT_Secret = "1234567890123456", Client_URL = "http://localhost:4200" };
            IOptions<ApplicationSettings> operationalStoreOptions = Options.Create(applicationSettings);

            var controller = new ApplicationUserController(mgr.Object, null, operationalStoreOptions);

            Xunit.Assert.NotNull(controller);
            var applicationSettingsx = new ApplicationUserModel { FullName = "tayyab", UserName = "tayyab", Email = "tayyab", Role = "Admin", Password = "1234" };
            var actionResult = await controller.PostApplicationUser(applicationSettingsx);
            var item = (Object)actionResult;
            Xunit.Assert.NotNull(item);



            /* var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
             List<CartDetails> cartx = new List<CartDetails>();
             cartx.Add(cartdetils1);
             var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

             var actionResult = await controller.PostCart(manager);
             var item = (Cart)((CreatedAtActionResult)actionResult.Result).Value;
             output.WriteLine("This is output from {0}", item.Cart_id);
             Xunit.Assert.IsType<Cart>(item);
             Xunit.Assert.Equal(manager.Cart_id, item.Cart_id);*/
            //Xunit.Assert.Equal(cat.Pr_name, item.Pr_name);

        }
    }
}
