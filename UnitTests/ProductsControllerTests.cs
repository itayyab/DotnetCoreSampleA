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
using Duende.IdentityServer.EntityFramework.Options;
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
   public class ProductsControllerTests
    {
        private readonly ITestOutputHelper output;
        private readonly ApplicationDbContext applicationDbContext;

        public ProductsControllerTests(ITestOutputHelper output)
        {
            this.output = output;
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
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
                one.Pr_id = 3;
                one.Pr_name = "Test";
                one.Pr_price = 100;
                one.Pr_Picture = "";
                one.Pr_desc = "Test desc";

                var two = new Product();
                two.Pr_id = 4;
                two.Pr_name = "Test";
                two.Pr_price = 100;
                two.Pr_Picture = "";
                two.Pr_desc = "Test desc";
                context.AddRange(one, two);
                context.SaveChanges();
            }
        }
         [Fact]
         public async Task Save_new_Product()
         {
             using (var context = applicationDbContext)
             {
                ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                var controller = new ProductsController(context,logger);
                Product cat = new Product();
                 cat.Pr_id = 3;
                 cat.Pr_name = "Test";
                cat.Pr_price =100;
                cat.Pr_Picture = "";
                cat.Pr_desc = "Test desc";
               var actionResult = await controller.PostProduct(cat);         
                 var item = (Product)((CreatedAtActionResult)actionResult.Result).Value;
                 output.WriteLine("This is output from {0}", item.Pr_name);
                 Xunit.Assert.IsType<Product>(item);
                 Xunit.Assert.Equal(cat.Pr_id, item.Pr_id);
                 Xunit.Assert.Equal(cat.Pr_name, item.Pr_name);
             }
         }
        
       [Fact]
        public async Task Get_products()
        {
            using (var context = applicationDbContext)
            {
                // Given
                var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                context.Product.AddRange(manager,manager2);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<ProductsController>();
                var controller = new ProductsController(context,logger);

                  var actionResult = await controller.GetProduct();

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Product>;
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(2, lstUsers.Count());
            }

           
        }
        [Fact]
        public async Task GetProductsByCategory()
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
                var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc", CategoryForeignKey = 1 };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc", CategoryForeignKey=2 };
                context.Product.AddRange(manager, manager2);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<ProductsController>();
                var controller = new ProductsController(context, logger);

                var actionResult = await controller.GetProductsByCategory();

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Category>;
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(2, lstUsers.Count());
            }
        }
        [Fact]
        public async Task GetProductsByCategoryID()
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
                var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc", CategoryForeignKey = 1 };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc", CategoryForeignKey = 1 };
                context.Product.AddRange(manager, manager2);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<ProductsController>();
                var controller = new ProductsController(context, logger);

                var actionResult = await controller.GetProductsByCategoryID(1);

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Category>;
                
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(2, lstUsers.FirstOrDefault().Products.Count());
            }
        }
        [Fact]
        public async Task GetProductWithCat()
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
                var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc", CategoryForeignKey = 1 };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc", CategoryForeignKey = 2 };
                context.Product.AddRange(manager, manager2);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<ProductsController>();
                var controller = new ProductsController(context, logger);

                var actionResult = await controller.GetProductWithCat();

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Product>;
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(2, lstUsers.Count());
            }
        }
        [Fact]
        public async Task GetProductWithCatByID()
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
                var manager = new Product { Pr_id = 1, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc", CategoryForeignKey = 1 };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc", CategoryForeignKey = 1 };
                context.Product.AddRange(manager, manager2);
                context.SaveChanges();
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<ProductsController>();
                var controller = new ProductsController(context, logger);

                var actionResult = await controller.GetProductWithCatByID(1);

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Product>;

                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Single(lstUsers);
            }
        }
        [Fact]
         public async Task Get_Product_By_ID()
         {
             using (var context = applicationDbContext)
             {
                // Given
                var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                context.Product.AddRange(manager, manager2);
                 context.SaveChanges();
                 // ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                 using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                 var logger = loggerFactory.CreateLogger<ProductsController>();
                 var controller = new ProductsController(context, logger);
                 var actionResult = await controller.GetProduct(3);

                 var lstUsers = ((OkObjectResult)actionResult.Result).Value as Product;
                 Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                 Xunit.Assert.Equal(manager, lstUsers);
             }


         }
         [Fact]
         public async Task Get_Product_By_ID_Not_found()
         {
             //IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
             //using (var context = new ApplicationDbContext(options,operationalStoreOptions))
             using (var context = applicationDbContext)
             {
                 // ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                 using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                 var logger = loggerFactory.CreateLogger<ProductsController>();
                 var controller = new ProductsController(context, logger);
                 var actionResult = await controller.GetProduct(5);
                 Xunit.Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);

             }


         }
          [Fact]
          public async Task Update_Product_By_ID_Bad_Request()
          {
              using (var context = applicationDbContext)
              {

                var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                context.Product.AddRange(manager, manager2);
                  context.SaveChanges();
                  // ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                  using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                  var logger = loggerFactory.CreateLogger<ProductsController>();
                  var controller = new ProductsController(context, logger);

                  var actionResult = await controller.PutProduct(5,manager);
                  Xunit.Assert.IsAssignableFrom<BadRequestResult>(actionResult);
              }

          }
         [Fact]
         public async Task Update_Product_By_ID_OK()
         {
             using (var context = applicationDbContext)
             {
                var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                context.Product.AddRange(manager, manager2);
                 context.SaveChanges();
                 // ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                 using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                 var logger = loggerFactory.CreateLogger<ProductsController>();
                 var controller = new ProductsController(context, logger);
                 var actionResult = await controller.PutProduct(3, manager);
                 Xunit.Assert.IsAssignableFrom<OkResult>(actionResult);
             }


         }
         [Fact]
          public async Task Delete_Product_Not_found()
          {
              using (var context = applicationDbContext)
              {
                  ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                  var controller = new ProductsController(context, logger);
                var actionResult = await controller.DeleteProduct(14);
                Xunit.Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
                //output.WriteLine("This is output from {0}", actionResult.Result);
              }
          }
          [Fact]
         public async Task Delete_Product()
         {
             using (var context = applicationDbContext)
             {
                 ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                 var controller = new ProductsController(context, logger);
                 var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                context.Product.Add(manager);
                 context.SaveChanges();
                var actionResult = await controller.DeleteProduct(3);
                 var lstUsers = ((OkObjectResult)actionResult.Result).Value as Product;
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                output.WriteLine("This is output from {0}", lstUsers.Pr_name);
             }
         }

         [Fact]
         public async Task  Check_Product_Exists()
         {
             using (var context = applicationDbContext)
             {
                var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                context.Product.AddRange(manager);
                 context.SaveChanges();

                 // ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                 using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                 var logger = loggerFactory.CreateLogger<ProductsController>();
                 var controller = new ProductsController(context, logger);
                 var actionResult = await controller.PutProduct(4, manager2);
                 Xunit.Assert.IsAssignableFrom<NotFoundResult>(actionResult);
             }
             }
        [Fact]
        public async Task Upload_Product_PIC()
        {
            using (var context = applicationDbContext)
            {



                ILogger<ProductsController> logger = new Logger<ProductsController>(new NullLoggerFactory());
                var controller = new ProductsController(context, logger);
              //  IFormFile file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.txt");

                var fileMock = new Mock<IFormFile>();
                //Setup mock file using a memory stream
                var content = "Hello World from a Fake File";
                var fileName = "test.jpg";
                var ms = new MemoryStream();
                var writer = new StreamWriter(ms);
                writer.Write(content);
                writer.Flush();
                ms.Position = 0;
                fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
                fileMock.Setup(_ => _.FileName).Returns(fileName);
                fileMock.Setup(_ => _.Length).Returns(ms.Length);

              ///  var sut = new MyController();
                var file = fileMock.Object;
             //   output.WriteLine("This is output from {0}", file.FileName+file.Name);
                var actionResult = await controller.OnPostUploadAsync(file);
                /*var manager = new Product { Pr_id = 3, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
                context.Product.Add(manager);
                context.SaveChanges();
                var actionResult = await controller.DeleteProduct(3);
                var lstUsers = ((OkObjectResult)actionResult.Result).Value as Product;*/
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
               // output.WriteLine("This is output from {0}", actionResult.Result);
            }
        }
    }
}
