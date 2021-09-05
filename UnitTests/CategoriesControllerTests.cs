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


namespace UnitTests
{
    [TestClass]
   public class CategoriesControllerTests
    {

        //[TestMethod]
        //   public void CreateBlog_saves_a_blog_via_context()
        //   {

        //   var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //         .UseInMemoryDatabase(Guid.NewGuid().ToString())
        //         .Options;

        //  // var mockSet = new Mock<ApplicationDbContext<Categories>>();
        //  // var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //  //.UseInMemoryDatabase(databaseName: "MovieListDatabase")
        //  //.Options;
        //   ApplicationDbContext applicationDbContext = new ApplicationDbContext(options,null);


        //   }
        private readonly ITestOutputHelper output;
        private readonly ApplicationDbContext applicationDbContext;

        public CategoriesControllerTests(ITestOutputHelper output)
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

                var one = new Category();
                one.Cat_id = 1;
                one.Cat_name = "Test";

                var two = new Category();
                two.Cat_id = 2;
                two.Cat_name = "Test";

                //var three = new Item("ItemThree");
                //three.AddTag("Tag31");
                //three.AddTag("Tag31");
                //three.AddTag("Tag31");
                //three.AddTag("Tag32");
                //three.AddTag("Tag32");

                context.AddRange(one, two);

                context.SaveChanges();
            }
        }
         [Fact]
         public async Task Save_new_Category()
         {

             using (var context = applicationDbContext)
             {
                ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                var controller = new CategoriesController(context,logger);


                 Category cat = new Category();
                 cat.Cat_id = 3;
                 cat.Cat_name = "Test";

                 // 

                  var actionResult = await controller.PostCategories(cat);
                // var actionResult = await controller.PostCategories(cat);

               // var result = controller.PostCategories(cat).Result;
               //  var result = service.Get(bookID).Result as ObjectResult;
               //  var actualtResult = result.Value;

                 //Assert
                 //Return proper HTTPStatus code\

                 var item = (Category)((CreatedAtActionResult)actionResult.Result).Value;
                 //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                 output.WriteLine("This is output from {0}", item.Cat_name);
                 Xunit.Assert.IsType<Category>(item);

                 Xunit.Assert.Equal(cat.Cat_id, item.Cat_id);
                 Xunit.Assert.Equal(cat.Cat_name, item.Cat_name);

                 //  Xunit.Assert.IsType<OkObjectResult>(result);
                 //OR
                 //Xunit.Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
                 //Addtional asserts
                 //   Xunit.Assert.Equal(document.Author, ((Book)actualtResult).Author);
                 // Xunit.Assert.Equal(document.BookName, ((Book)actualtResult).BookName);
                 //   Xunit.Assert.Equal(document.Id, ((Book)actualtResult).Id);
                 //  var result = actionResult as Task<ActionResult<Categories>>;
                 //

                 //var viewResult = result.Result as ViewResult;


                 //    var obj1Str = JsonConvert.SerializeObject(item);
                 //   var obj2Str = JsonConvert.SerializeObject(cat);
                 //  Xunit.Assert.Equal(obj1Str, obj2Str);
                 // var act=CreatedAtAction("GetCategories", new { id = categories.Cat_id }, categories);
                 //Xunit.Assert.Equal(cat, item);
             }
         }
        
        [Fact]
        public async Task Get_Categorries()
        {

            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //.UseInMemoryDatabase(databaseName: $"ApplicationDatabase{Guid.NewGuid()}")
            //.Options;
            //OperationalStoreOptions storeOptions = new OperationalStoreOptions
            //{
            //    //populate needed members
            //};

            //IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
            //using (var context = new ApplicationDbContext(options,operationalStoreOptions))
            using (var context = applicationDbContext)
            {
                // Given
                var manager = new Category { Cat_id = 1, Cat_name = "test@test.fr" };
                var manager2 = new Category { Cat_id = 2, Cat_name = "test@test.fr" };
                context.Categories.AddRange(manager,manager2);
                //context.Categories.Add(manager);
                //context.Categories.Add(manager2);
                context.SaveChanges();


                // ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CategoriesController>();
                var controller = new CategoriesController(context,logger);


                 //var i= await context.Categories.ToListAsync();
                  var actionResult = await controller.GetCategories();

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as IEnumerable<Category>;
                // var item = ((ActionResult)actionResult.Result);
                //var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(2, lstUsers.Count());
               // Xunit.Assert.Equal(manager, lstUsers); // SHOULD WORK!!
              //  output.WriteLine("This is output from 2 {0}", lstUsers);
                // When 
                //var repo = new UserRepository(context);
                // var user = repo.GetUserByUsername("Ombrelin");

                // Then
                // Assert.Equal("Ombrelin", user.Username);
            }

            /*using (var context = applicationDbContext)
            {
                var controller = new CategoriesController(context);


                Categories cat = new Categories();
                cat.Cat_id = 3;
                cat.Cat_name = "Test";

                // 

                var actionResultx = await controller.PostCategories(cat);
                // var controller = new CategoriesController(context);
                var itemx = (Categories)((CreatedAtActionResult)actionResultx.Result).Value;
                output.WriteLine("This is output from {0}", itemx.Cat_name);


                // 

                var actionResult = await controller.GetCategories();
                // var actionResult = await controller.PostCategories(cat);

                // var result = controller.PostCategories(cat).Result;
                //  var result = service.Get(bookID).Result as ObjectResult;
                //  var actualtResult = result.Value;

                //Assert
                //Return proper HTTPStatus code\

                var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                output.WriteLine("This is output from 2 {0}", item);
                // Xunit.Assert.IsType<Categories>(item);

                //Xunit.Assert.Equal(cat.Cat_id, item.Cat_id);
                // Xunit.Assert.Equal(cat.Cat_name, item.Cat_name);

                //  Xunit.Assert.IsType<OkObjectResult>(result);
                //OR
                //Xunit.Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
                //Addtional asserts
                //   Xunit.Assert.Equal(document.Author, ((Book)actualtResult).Author);
                // Xunit.Assert.Equal(document.BookName, ((Book)actualtResult).BookName);
                //   Xunit.Assert.Equal(document.Id, ((Book)actualtResult).Id);
                //  var result = actionResult as Task<ActionResult<Categories>>;
                //

                //var viewResult = result.Result as ViewResult;


                //    var obj1Str = JsonConvert.SerializeObject(item);
                //   var obj2Str = JsonConvert.SerializeObject(cat);
                //  Xunit.Assert.Equal(obj1Str, obj2Str);
                // var act=CreatedAtAction("GetCategories", new { id = categories.Cat_id }, categories);
                //Xunit.Assert.Equal(cat, item);
            }*/
        }
        [Fact]
        public async Task Get_Categorrie_By_ID()
        {

            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //.UseInMemoryDatabase(databaseName: $"ApplicationDatabase{Guid.NewGuid()}")
            //.Options;
            //OperationalStoreOptions storeOptions = new OperationalStoreOptions
            //{
            //    //populate needed members
            //};

            //IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
            //using (var context = new ApplicationDbContext(options,operationalStoreOptions))
            using (var context = applicationDbContext)
            {
                // Given
                var manager = new Category { Cat_id = 1, Cat_name = "test@test.fr" };
                var manager2 = new Category { Cat_id = 2, Cat_name = "test@test.fr" };
                context.Categories.AddRange(manager, manager2);
                //context.Categories.Add(manager);
                //context.Categories.Add(manager2);
                context.SaveChanges();


                // ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CategoriesController>();
                var controller = new CategoriesController(context, logger);


                //var i= await context.Categories.ToListAsync();
                var actionResult = await controller.GetCategories(1);

                var lstUsers = ((OkObjectResult)actionResult.Result).Value as Category;
                // var item = ((ActionResult)actionResult.Result);
                //var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                Xunit.Assert.Equal(manager, lstUsers);
                // Xunit.Assert.Equal(manager, lstUsers); // SHOULD WORK!!
                //  output.WriteLine("This is output from 2 {0}", lstUsers);
                // When 
                //var repo = new UserRepository(context);
                // var user = repo.GetUserByUsername("Ombrelin");

                // Then
                // Assert.Equal("Ombrelin", user.Username);
            }

            /*using (var context = applicationDbContext)
            {
                var controller = new CategoriesController(context);


                Categories cat = new Categories();
                cat.Cat_id = 3;
                cat.Cat_name = "Test";

                // 

                var actionResultx = await controller.PostCategories(cat);
                // var controller = new CategoriesController(context);
                var itemx = (Categories)((CreatedAtActionResult)actionResultx.Result).Value;
                output.WriteLine("This is output from {0}", itemx.Cat_name);


                // 

                var actionResult = await controller.GetCategories();
                // var actionResult = await controller.PostCategories(cat);

                // var result = controller.PostCategories(cat).Result;
                //  var result = service.Get(bookID).Result as ObjectResult;
                //  var actualtResult = result.Value;

                //Assert
                //Return proper HTTPStatus code\

                var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                output.WriteLine("This is output from 2 {0}", item);
                // Xunit.Assert.IsType<Categories>(item);

                //Xunit.Assert.Equal(cat.Cat_id, item.Cat_id);
                // Xunit.Assert.Equal(cat.Cat_name, item.Cat_name);

                //  Xunit.Assert.IsType<OkObjectResult>(result);
                //OR
                //Xunit.Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
                //Addtional asserts
                //   Xunit.Assert.Equal(document.Author, ((Book)actualtResult).Author);
                // Xunit.Assert.Equal(document.BookName, ((Book)actualtResult).BookName);
                //   Xunit.Assert.Equal(document.Id, ((Book)actualtResult).Id);
                //  var result = actionResult as Task<ActionResult<Categories>>;
                //

                //var viewResult = result.Result as ViewResult;


                //    var obj1Str = JsonConvert.SerializeObject(item);
                //   var obj2Str = JsonConvert.SerializeObject(cat);
                //  Xunit.Assert.Equal(obj1Str, obj2Str);
                // var act=CreatedAtAction("GetCategories", new { id = categories.Cat_id }, categories);
                //Xunit.Assert.Equal(cat, item);
            }*/
        }
        [Fact]
        public async Task Get_Categorrie_By_ID_Not_found()
        {

            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //.UseInMemoryDatabase(databaseName: $"ApplicationDatabase{Guid.NewGuid()}")
            //.Options;
            //OperationalStoreOptions storeOptions = new OperationalStoreOptions
            //{
            //    //populate needed members
            //};

            //IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
            //using (var context = new ApplicationDbContext(options,operationalStoreOptions))
            using (var context = applicationDbContext)
            {
                // Given
                


                // ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CategoriesController>();
                var controller = new CategoriesController(context, logger);


                //var i= await context.Categories.ToListAsync();
                var actionResult = await controller.GetCategories(5);

               // var lstUsers = ((NotFoundResult)actionResult.Result).Value as Categories;
                // var item = ((ActionResult)actionResult.Result);
                //var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                Xunit.Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
               // Xunit.Assert.Equal(manager, lstUsers);
                // Xunit.Assert.Equal(manager, lstUsers); // SHOULD WORK!!
                //  output.WriteLine("This is output from 2 {0}", lstUsers);
                // When 
                //var repo = new UserRepository(context);
                // var user = repo.GetUserByUsername("Ombrelin");

                // Then
                // Assert.Equal("Ombrelin", user.Username);
            }

            /*using (var context = applicationDbContext)
            {
                var controller = new CategoriesController(context);


                Categories cat = new Categories();
                cat.Cat_id = 3;
                cat.Cat_name = "Test";

                // 

                var actionResultx = await controller.PostCategories(cat);
                // var controller = new CategoriesController(context);
                var itemx = (Categories)((CreatedAtActionResult)actionResultx.Result).Value;
                output.WriteLine("This is output from {0}", itemx.Cat_name);


                // 

                var actionResult = await controller.GetCategories();
                // var actionResult = await controller.PostCategories(cat);

                // var result = controller.PostCategories(cat).Result;
                //  var result = service.Get(bookID).Result as ObjectResult;
                //  var actualtResult = result.Value;

                //Assert
                //Return proper HTTPStatus code\

                var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                output.WriteLine("This is output from 2 {0}", item);
                // Xunit.Assert.IsType<Categories>(item);

                //Xunit.Assert.Equal(cat.Cat_id, item.Cat_id);
                // Xunit.Assert.Equal(cat.Cat_name, item.Cat_name);

                //  Xunit.Assert.IsType<OkObjectResult>(result);
                //OR
                //Xunit.Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
                //Addtional asserts
                //   Xunit.Assert.Equal(document.Author, ((Book)actualtResult).Author);
                // Xunit.Assert.Equal(document.BookName, ((Book)actualtResult).BookName);
                //   Xunit.Assert.Equal(document.Id, ((Book)actualtResult).Id);
                //  var result = actionResult as Task<ActionResult<Categories>>;
                //

                //var viewResult = result.Result as ViewResult;


                //    var obj1Str = JsonConvert.SerializeObject(item);
                //   var obj2Str = JsonConvert.SerializeObject(cat);
                //  Xunit.Assert.Equal(obj1Str, obj2Str);
                // var act=CreatedAtAction("GetCategories", new { id = categories.Cat_id }, categories);
                //Xunit.Assert.Equal(cat, item);
            }*/
        }
        [Fact]
        public async Task GUpdate_Categorrie_By_ID_Bad_Request()
        {

            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //.UseInMemoryDatabase(databaseName: $"ApplicationDatabase{Guid.NewGuid()}")
            //.Options;
            //OperationalStoreOptions storeOptions = new OperationalStoreOptions
            //{
            //    //populate needed members
            //};

            //IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
            //using (var context = new ApplicationDbContext(options,operationalStoreOptions))
            using (var context = applicationDbContext)
            {
                // Given

                var manager = new Category { Cat_id = 1, Cat_name = "test@test.fr" };
                var manager2 = new Category { Cat_id = 2, Cat_name = "test@test.fr" };
                context.Categories.AddRange(manager, manager2);
                //context.Categories.Add(manager);
                //context.Categories.Add(manager2);
                context.SaveChanges();

                // ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CategoriesController>();
                var controller = new CategoriesController(context, logger);


                //var i= await context.Categories.ToListAsync();
                var actionResult = await controller.PutCategories(5,manager);

                // var lstUsers = ((NotFoundResult)actionResult.Result).Value as Categories;
                // var item = ((ActionResult)actionResult.Result);
                //var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                Xunit.Assert.IsAssignableFrom<BadRequestResult>(actionResult);
                // Xunit.Assert.Equal(manager, lstUsers);
                // Xunit.Assert.Equal(manager, lstUsers); // SHOULD WORK!!
                //  output.WriteLine("This is output from 2 {0}", lstUsers);
                // When 
                //var repo = new UserRepository(context);
                // var user = repo.GetUserByUsername("Ombrelin");

                // Then
                // Assert.Equal("Ombrelin", user.Username);
            }

            /*using (var context = applicationDbContext)
            {
                var controller = new CategoriesController(context);


                Categories cat = new Categories();
                cat.Cat_id = 3;
                cat.Cat_name = "Test";

                // 

                var actionResultx = await controller.PostCategories(cat);
                // var controller = new CategoriesController(context);
                var itemx = (Categories)((CreatedAtActionResult)actionResultx.Result).Value;
                output.WriteLine("This is output from {0}", itemx.Cat_name);


                // 

                var actionResult = await controller.GetCategories();
                // var actionResult = await controller.PostCategories(cat);

                // var result = controller.PostCategories(cat).Result;
                //  var result = service.Get(bookID).Result as ObjectResult;
                //  var actualtResult = result.Value;

                //Assert
                //Return proper HTTPStatus code\

                var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                output.WriteLine("This is output from 2 {0}", item);
                // Xunit.Assert.IsType<Categories>(item);

                //Xunit.Assert.Equal(cat.Cat_id, item.Cat_id);
                // Xunit.Assert.Equal(cat.Cat_name, item.Cat_name);

                //  Xunit.Assert.IsType<OkObjectResult>(result);
                //OR
                //Xunit.Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
                //Addtional asserts
                //   Xunit.Assert.Equal(document.Author, ((Book)actualtResult).Author);
                // Xunit.Assert.Equal(document.BookName, ((Book)actualtResult).BookName);
                //   Xunit.Assert.Equal(document.Id, ((Book)actualtResult).Id);
                //  var result = actionResult as Task<ActionResult<Categories>>;
                //

                //var viewResult = result.Result as ViewResult;


                //    var obj1Str = JsonConvert.SerializeObject(item);
                //   var obj2Str = JsonConvert.SerializeObject(cat);
                //  Xunit.Assert.Equal(obj1Str, obj2Str);
                // var act=CreatedAtAction("GetCategories", new { id = categories.Cat_id }, categories);
                //Xunit.Assert.Equal(cat, item);
            }*/
        }
        [Fact]
        public async Task Update_Categorrie_By_ID_OK()
        {

            //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            //.UseInMemoryDatabase(databaseName: $"ApplicationDatabase{Guid.NewGuid()}")
            //.Options;
            //OperationalStoreOptions storeOptions = new OperationalStoreOptions
            //{
            //    //populate needed members
            //};

            //IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
            //using (var context = new ApplicationDbContext(options,operationalStoreOptions))
            using (var context = applicationDbContext)
            {
                // Given

                var manager = new Category { Cat_id = 1, Cat_name = "test@test.fr" };
                var manager2 = new Category { Cat_id = 2, Cat_name = "test@test.fr" };
                context.Categories.AddRange(manager, manager2);
                //context.Categories.Add(manager);
                //context.Categories.Add(manager2);
                context.SaveChanges();

                // ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CategoriesController>();
                var controller = new CategoriesController(context, logger);


                //var i= await context.Categories.ToListAsync();
                var actionResult = await controller.PutCategories(1, manager);

                // var lstUsers = ((NotFoundResult)actionResult.Result).Value as Categories;
                // var item = ((ActionResult)actionResult.Result);
                //var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                Xunit.Assert.IsAssignableFrom<OkResult>(actionResult);
                // Xunit.Assert.Equal(manager, lstUsers);
                // Xunit.Assert.Equal(manager, lstUsers); // SHOULD WORK!!
                //  output.WriteLine("This is output from 2 {0}", lstUsers);
                // When 
                //var repo = new UserRepository(context);
                // var user = repo.GetUserByUsername("Ombrelin");

                // Then
                // Assert.Equal("Ombrelin", user.Username);
            }

            /*using (var context = applicationDbContext)
            {
                var controller = new CategoriesController(context);


                Categories cat = new Categories();
                cat.Cat_id = 3;
                cat.Cat_name = "Test";

                // 

                var actionResultx = await controller.PostCategories(cat);
                // var controller = new CategoriesController(context);
                var itemx = (Categories)((CreatedAtActionResult)actionResultx.Result).Value;
                output.WriteLine("This is output from {0}", itemx.Cat_name);


                // 

                var actionResult = await controller.GetCategories();
                // var actionResult = await controller.PostCategories(cat);

                // var result = controller.PostCategories(cat).Result;
                //  var result = service.Get(bookID).Result as ObjectResult;
                //  var actualtResult = result.Value;

                //Assert
                //Return proper HTTPStatus code\

                var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                output.WriteLine("This is output from 2 {0}", item);
                // Xunit.Assert.IsType<Categories>(item);

                //Xunit.Assert.Equal(cat.Cat_id, item.Cat_id);
                // Xunit.Assert.Equal(cat.Cat_name, item.Cat_name);

                //  Xunit.Assert.IsType<OkObjectResult>(result);
                //OR
                //Xunit.Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
                //Addtional asserts
                //   Xunit.Assert.Equal(document.Author, ((Book)actualtResult).Author);
                // Xunit.Assert.Equal(document.BookName, ((Book)actualtResult).BookName);
                //   Xunit.Assert.Equal(document.Id, ((Book)actualtResult).Id);
                //  var result = actionResult as Task<ActionResult<Categories>>;
                //

                //var viewResult = result.Result as ViewResult;


                //    var obj1Str = JsonConvert.SerializeObject(item);
                //   var obj2Str = JsonConvert.SerializeObject(cat);
                //  Xunit.Assert.Equal(obj1Str, obj2Str);
                // var act=CreatedAtAction("GetCategories", new { id = categories.Cat_id }, categories);
                //Xunit.Assert.Equal(cat, item);
            }*/
        }
        [Fact]
        public async Task Delete_Category_Not_found()
        {
            
            using (var context = applicationDbContext)
            {
                ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                var controller = new CategoriesController(context, logger);


                Category cat = new Category();
                cat.Cat_id = 1;
                cat.Cat_name = "Test";

                // 

                var actionResult = await controller.DeleteCategories(1);
                Xunit.Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
                // var actionResult = await controller.PostCategories(cat);

                // var result = controller.PostCategories(cat).Result;
                //  var result = service.Get(bookID).Result as ObjectResult;
                //  var actualtResult = result.Value;

                //Assert
                //Return proper HTTPStatus code\

                // var item = (Categories)((ActionResult)actionResult.Result);
                //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                output.WriteLine("This is output from {0}", actionResult.Result);
              //  Xunit.Assert.IsType<Categories>(item);

                //Xunit.Assert.Equal(cat.Cat_id, item.Cat_id);
                //Xunit.Assert.Equal(cat.Cat_name, item.Cat_name);

                //  Xunit.Assert.IsType<OkObjectResult>(result);
                //OR
                //Xunit.Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
                //Addtional asserts
                //   Xunit.Assert.Equal(document.Author, ((Book)actualtResult).Author);
                // Xunit.Assert.Equal(document.BookName, ((Book)actualtResult).BookName);
                //   Xunit.Assert.Equal(document.Id, ((Book)actualtResult).Id);
                //  var result = actionResult as Task<ActionResult<Categories>>;
                //

                //var viewResult = result.Result as ViewResult;


                //    var obj1Str = JsonConvert.SerializeObject(item);
                //   var obj2Str = JsonConvert.SerializeObject(cat);
                //  Xunit.Assert.Equal(obj1Str, obj2Str);
                // var act=CreatedAtAction("GetCategories", new { id = categories.Cat_id }, categories);
                //Xunit.Assert.Equal(cat, item);
            }
        }
        [Fact]
        public async Task Delete_Category()
        {

            using (var context = applicationDbContext)
            {
                ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                var controller = new CategoriesController(context, logger);
                var manager = new Category { Cat_id = 1, Cat_name = "test@test.fr" };
                
                context.Categories.Add(manager);
                context.SaveChanges();

                Category cat = new Category();
                cat.Cat_id = 1;
                cat.Cat_name = "Test";

                // 

                var actionResult = await controller.DeleteCategories(1);
                // var actionResult = await controller.PostCategories(cat);

                // var result = controller.PostCategories(cat).Result;
                //  var result = service.Get(bookID).Result as ObjectResult;
                //  var actualtResult = result.Value;

                //Assert
                //Return proper HTTPStatus code\
                var lstUsers = ((OkObjectResult)actionResult.Result).Value as Category;
                // var item = (Categories)((ActionResult)actionResult.Result);
                //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                output.WriteLine("This is output from {0}", lstUsers.Cat_name);
                Xunit.Assert.IsAssignableFrom<OkObjectResult>(actionResult.Result);
                //  Xunit.Assert.IsType<Categories>(item);

                //Xunit.Assert.Equal(cat.Cat_id, item.Cat_id);
                //Xunit.Assert.Equal(cat.Cat_name, item.Cat_name);

                //  Xunit.Assert.IsType<OkObjectResult>(result);
                //OR
                //Xunit.Assert.Equal(HttpStatusCode.OK, (HttpStatusCode)result.StatusCode);
                //Addtional asserts
                //   Xunit.Assert.Equal(document.Author, ((Book)actualtResult).Author);
                // Xunit.Assert.Equal(document.BookName, ((Book)actualtResult).BookName);
                //   Xunit.Assert.Equal(document.Id, ((Book)actualtResult).Id);
                //  var result = actionResult as Task<ActionResult<Categories>>;
                //

                //var viewResult = result.Result as ViewResult;


                //    var obj1Str = JsonConvert.SerializeObject(item);
                //   var obj2Str = JsonConvert.SerializeObject(cat);
                //  Xunit.Assert.Equal(obj1Str, obj2Str);
                // var act=CreatedAtAction("GetCategories", new { id = categories.Cat_id }, categories);
                //Xunit.Assert.Equal(cat, item);
                //entity.Timestamp = new byte[] { 1 };
                //repository.Update(entity);
                //await context.SaveAsync(); // <-- this will throw a DbUpdateConcurrencyException
            }
        }

        [Fact]
        public async Task  Check_Category_Exists()
        {

            //using (var context = applicationDbContext)
            //{
            //    ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
            //    var controller = new CategoriesController(context, logger);

            //    //var exits =  controller.CategoriesExists(1);
            //    //Xunit.Assert.False(exits);
               
            //}
            using (var context = applicationDbContext)
            {
                // Given

                var manager = new Category { Cat_id = 1, Cat_name = "test@test.fr" };
                var manager2 = new Category { Cat_id = 2, Cat_name = "test@test.fr" };
                context.Categories.AddRange(manager);
                //context.Categories.Add(manager);
                //context.Categories.Add(manager2);
                context.SaveChanges();

                // ILogger<CategoriesController> logger = new Logger<CategoriesController>(new NullLoggerFactory());
                using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
                var logger = loggerFactory.CreateLogger<CategoriesController>();
                var controller = new CategoriesController(context, logger);


                //var i= await context.Categories.ToListAsync();
                var actionResult = await controller.PutCategories(2, manager2);

                // var lstUsers = ((NotFoundResult)actionResult.Result).Value as Categories;
                // var item = ((ActionResult)actionResult.Result);
                //var item = (IEnumerable<Categories>)((ActionResult)actionResult.Result);
                Xunit.Assert.IsAssignableFrom<NotFoundResult>(actionResult);
            }
            }

    }
}
