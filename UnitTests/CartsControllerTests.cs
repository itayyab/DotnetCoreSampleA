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

namespace UnitTests
{
    [TestClass]
   public class CartsControllerTests
    {
        private readonly ITestOutputHelper output;
        private readonly ApplicationDbContext applicationDbContext;

        public CartsControllerTests(ITestOutputHelper output)
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
            applicationDbContext =new ApplicationDbContext(options, operationalStoreOptions);
            Seed();
        }

        private void Seed()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            OperationalStoreOptions storeOptions = new OperationalStoreOptions
            {
                //populate needed members
            };

            IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
            using (var context= new ApplicationDbContext(options, operationalStoreOptions))
            {
                //applicationDbContext.Database.EnsureDeleted();
                //applicationDbContext.Database.EnsureCreated();


                var one = new Product();
                one.Pr_id = 1;
                one.Pr_name = "Test";
                one.Pr_price = 100;
                one.Pr_Picture = "";
                one.Pr_desc = "Test desc";

                var two = new Product();
                two.Pr_id =2;
                two.Pr_name = "Test";
                two.Pr_price = 100;
                two.Pr_Picture = "";
                two.Pr_desc = "Test desc";
                context.AddRange(one, two);
                context.SaveChanges();
                
            }
        }
         [Fact]
         public async Task PostCart_Test_NewCart()
         {
             using (var context = applicationDbContext)
             {
                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context,logger);
                
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetils1);
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING",CartDetails=cartx };

                var actionResult = await controller.PostCart(manager);         
                 var item = (Cart)((CreatedAtActionResult)actionResult.Result).Value;
                 output.WriteLine("This is output from {0}", item.Cart_id);
                 Xunit.Assert.IsType<Cart>(item);
                 Xunit.Assert.Equal(manager.Cart_id, item.Cart_id);
                 //Xunit.Assert.Equal(cat.Pr_name, item.Pr_name);
             }
         }
        [Fact]
        public async Task PostCart_Test_CartAlreadyExists()
        {
            using (var context = applicationDbContext)
            {
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 2, ProductForeignKey = 2, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                context.Cart.Add(manager);
                context.CartDetails.Add(cartdetils1);
               // context.Entry<CartDetails>(cartdetils1).State = EntityState.Detached;
               // context.Entry<Cart>(manager).State = EntityState.Detached;
                context.SaveChanges();
                context.Entry<CartDetails>(cartdetils1).State = EntityState.Detached;
                context.Entry<Cart>(manager).State = EntityState.Detached;


                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context, logger);


                var cartdetilsnew = new CartDetails { CD_id = 0, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1};
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetilsnew);
                var managernew = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

                var actionResult = await controller.PostCart(managernew);
                var item = (Cart)((CreatedAtActionResult)actionResult.Result).Value;
                output.WriteLine("This is output from {0}", item.Cart_id);
                Xunit.Assert.IsType<Cart>(item);
                Xunit.Assert.Equal(manager.Cart_id, item.Cart_id);
                //Xunit.Assert.Equal(cat.Pr_name, item.Pr_name);
            }
        }
        [Fact]
        public async Task DeleteProductFromCart_Test_CartNotFound()
        {
            using (var context = applicationDbContext)
            {
            
                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context, logger);
                var cartdetilsnew = new CartDetails { CD_id = 0, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetilsnew);
                var managernew = new Cart { Cart_id = 1, UserID = "123456", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

                var actionResult = await controller.DetelteProducctFromCart(managernew);
                 var item = (NotFoundResult)((NotFoundResult)actionResult.Result);
                // output.WriteLine("This is output from {0}", item.Cart_id);
                // Xunit.Assert.IsType<Cart>(item);
                Xunit.Assert.IsAssignableFrom<NotFoundResult>(item);
            }
        }
        [Fact]
        public async Task DeleteProductFromCart_Test_ProductNotFound()
        {
            using (var context = applicationDbContext)
            {
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                context.Cart.Add(manager);
                context.CartDetails.Add(cartdetils1);
                
                context.SaveChanges();

                 context.Entry<CartDetails>(cartdetils1).State = EntityState.Detached;
                  context.Entry<Cart>(manager).State = EntityState.Detached;

                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context, logger);
                
                var cartdetilsnew = new CartDetails { CD_id = 1, CD_Pr_id =4, ProductForeignKey = 4, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetilsnew);
                var managernew = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

                var actionResult = await controller.DetelteProducctFromCart(managernew);
                var item = (NotFoundResult)((NotFoundResult)actionResult.Result);
                // output.WriteLine("This is output from {0}", item.Cart_id);
                // Xunit.Assert.IsType<Cart>(item);
                Xunit.Assert.IsAssignableFrom<NotFoundResult>(item);
                // Xunit.Assert.Equal(manager.Cart_id, item.Cart_id);
                //Xunit.Assert.Equal(cat.Pr_name, item.Pr_name);
            }
        }
        [Fact]
        public async Task DeleteProductFromCart_Test_CartProductExists()
        {
            using (var context = applicationDbContext)
            {
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                context.Cart.Add(manager);
                context.CartDetails.Add(cartdetils1);

                context.SaveChanges();

                context.Entry<CartDetails>(cartdetils1).State = EntityState.Detached;
                context.Entry<Cart>(manager).State = EntityState.Detached;

                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context, logger);

              

                var cartdetilsnew = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetilsnew);
                var managernew = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

                var actionResult = await controller.DetelteProducctFromCart(managernew);
              /*  var check = controller.CheckCartDetailsProductExists(1,1);
              Xunit.Assert.True(check);
              output.WriteLine("This is output from {0}", check);*/
                var item = (Cart)((CreatedAtActionResult)actionResult.Result).Value;
                output.WriteLine("This is output from {0}", item.Cart_id);
                Xunit.Assert.IsType<Cart>(item);
                Xunit.Assert.Equal(manager.Cart_id, item.Cart_id);
            }
        }
       /* [Fact]
        public async Task DeleteProductFromCart_Test_CartCartdetailsNotFound()
        {
            using (var context = applicationDbContext)
            {
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                context.Cart.Add(manager);
                context.CartDetails.Add(cartdetils1);

                context.SaveChanges();

                context.Entry<CartDetails>(cartdetils1).State = EntityState.Detached;
                context.Entry<Cart>(manager).State = EntityState.Detached;

                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context, logger);



                var cartdetilsnew = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetilsnew);
                var managernew = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

                var actionResult = await controller.DetelteProducctFromCart(managernew);
                var item = (NotFoundResult)((NotFoundResult)actionResult.Result);
                // output.WriteLine("This is output from {0}", item.Cart_id);
                // Xunit.Assert.IsType<Cart>(item);
                Xunit.Assert.IsAssignableFrom<NotFoundResult>(item);
            }
        }*/
        [Fact]
        public async Task PostCart_Test_CartProductAlreadyExists()
        {
            using (var context = applicationDbContext)
            {
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                context.Cart.Add(manager);
                context.CartDetails.Add(cartdetils1);

                context.SaveChanges();

                context.Entry<CartDetails>(cartdetils1).State = EntityState.Detached;
                context.Entry<Cart>(manager).State = EntityState.Detached;

                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context, logger);

                var cartdetilsnew = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetilsnew);
                var managernew = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

                var actionResult = await controller.PostCart(managernew);
                var item = (Cart)((CreatedAtActionResult)actionResult.Result).Value;
                output.WriteLine("This is output from {0}", item.Cart_id);
                Xunit.Assert.IsType<Cart>(item);
                // Xunit.Assert.Equal(manager.Cart_id, item.Cart_id);
                //Xunit.Assert.Equal(cat.Pr_name, item.Pr_name);
            }
        }
        [Fact]
        public async Task CheckoutCart_Test_NotFound()
        {
            using (var context = applicationDbContext)
            {

                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context, logger);

                var cartdetilsnew = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetilsnew);
                var managernew = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

                var actionResult = await controller.CheckoutCart(managernew);
                var item = (NotFoundResult)((NotFoundResult)actionResult.Result);
                // output.WriteLine("This is output from {0}", item.Cart_id);
                // Xunit.Assert.IsType<Cart>(item);
                Xunit.Assert.IsAssignableFrom<NotFoundResult>(item);
            }
        }
        [Fact]
        public async Task CheckoutCart_Test_Sucess()
        {
            using (var context = applicationDbContext)
            {
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                context.Cart.Add(manager);
                context.CartDetails.Add(cartdetils1);

                context.SaveChanges();

                context.Entry<CartDetails>(cartdetils1).State = EntityState.Detached;
                context.Entry<Cart>(manager).State = EntityState.Detached;

                ILogger<CartsController> logger = new Logger<CartsController>(new NullLoggerFactory());
                var controller = new CartsController(context, logger);

                var cartdetilsnew = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                List<CartDetails> cartx = new List<CartDetails>();
                cartx.Add(cartdetilsnew);
                var managernew = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };

                var actionResult = await controller.CheckoutCart(managernew);
                var item = (Cart)((CreatedAtActionResult)actionResult.Result).Value;
                output.WriteLine("This is output from {0}", item.Cart_id);
                Xunit.Assert.IsType<Cart>(item);
                // Xunit.Assert.Equal(manager.Cart_id, item.Cart_id);
                //Xunit.Assert.Equal(cat.Pr_name, item.Pr_name);
            }
        }
        [Fact]
        public async Task Get_Cart_Count()
        {
            using (var context = applicationDbContext)
            {
                // Given
                var manager = new Cart { Cart_id=1,UserID="123",TotalQty=2,TotalAmount=100,Status= "PENDING" };
                var manager2 = new Cart { Cart_id = 2, UserID = "1234", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                context.Cart.AddRange(manager,manager2);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CartsController>();
                var controller = new CartsController(context,logger);

                  var actionResult = await controller.GetCartCount("123");

                //  var lstUsers = ((OkObjectResult)actionResult.Result).Value;
                var lstUsers = ((OkObjectResult)actionResult.Result).Value as long?;
              //var xx=(long)((OkObjectResult)actionResult.Result).Value;
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(2, lstUsers);
            }

           
        }
        [Fact]
        public async Task Get_Cart_Count_Zero()
        {
            using (var context = applicationDbContext)
            {
                // Given
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var manager2 = new Cart { Cart_id = 2, UserID = "1234", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                context.Cart.AddRange(manager, manager2);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CartsController>();
                var controller = new CartsController(context, logger);

                var actionResult = await controller.GetCartCount("12346");

                //  var lstUsers = ((OkObjectResult)actionResult.Result).Value;
                var lstUsers = (long)((OkObjectResult)actionResult.Result).Value;
                //var xx=(long)((OkObjectResult)actionResult.Result).Value;
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(0, lstUsers);
            }


        }
        [Fact]
        public async Task GetCart()
        {
            using (var context = applicationDbContext)
            {
                // Given
                var cat1 = new Category { Cat_id = 1, Cat_name = "test@test.fr" };
                var cat2 = new Category { Cat_id = 2, Cat_name = "test@test.fr" };
                context.Categories.AddRange(cat1, cat2);
                //context.Categories.Add(manager);
                //context.Categories.Add(manager2);
                context.SaveChanges();
                // Given
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var manager2 = new Cart { Cart_id = 2, UserID = "1234", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                context.Cart.AddRange(manager, manager2);
                context.CartDetails.Add(cartdetils1);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CartsController>();
                var controller = new CartsController(context, logger);

                var actionResult = await controller.GetCart("123");

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Cart>;
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(1, lstUsers.FirstOrDefault().Cart_id);
                Xunit.Assert.Single(lstUsers);
            }
        }
        [Fact]
        public async Task GetCartByID()
        {
            using (var context = applicationDbContext)
            {
                // Given
                var cat1 = new Category { Cat_id = 1, Cat_name = "test@test.fr" };
                var cat2 = new Category { Cat_id = 2, Cat_name = "test@test.fr" };
                context.Categories.AddRange(cat1, cat2);
                //context.Categories.Add(manager);
                //context.Categories.Add(manager2);
                context.SaveChanges();
                // Given
                var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING" };
                var manager2 = new Cart { Cart_id = 2, UserID = "1234", TotalQty = 2, TotalAmount = 100, Status = "CONFIRMED" };
                var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1 };
                var cartdetils2 = new CartDetails { CD_id =2, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 2 };
                context.Cart.AddRange(manager, manager2);
                context.CartDetails.AddRange(cartdetils1,cartdetils2);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CartsController>();
                var controller = new CartsController(context, logger);

                var actionResult = await controller.GetCartByID("1234", 2);

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Cart>;
                
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Single(lstUsers);
                Xunit.Assert.Equal(2, lstUsers.FirstOrDefault().Cart_id);

                // Xunit.Assert.Equal(2, lstUsers.FirstOrDefault().Cart_id=2);
            }
        }

    }
}
