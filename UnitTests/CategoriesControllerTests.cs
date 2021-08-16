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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Xunit.Abstractions;
using System.Threading.Tasks;
using IdentityServer4.EntityFramework.Options;
using Microsoft.Extensions.Options;

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

        public CategoriesControllerTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public async Task Save_new_Category()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                 .UseInMemoryDatabase(Guid.NewGuid().ToString())
                 .Options;
            OperationalStoreOptions storeOptions = new OperationalStoreOptions
            {
                //populate needed members
            };

            IOptions<OperationalStoreOptions> operationalStoreOptions = Options.Create(storeOptions);
            using (var context = new ApplicationDbContext(options, operationalStoreOptions))
            {
                var controller = new CategoriesController(context);


                Categories cat = new Categories();
                cat.Cat_id = 0;
                cat.Cat_name = "Test";

                // 

                 var actionResult = await controller.PostCategories(cat);
               // var actionResult = await controller.PostCategories(cat);

              // var result = controller.PostCategories(cat).Result;
              //  var result = service.Get(bookID).Result as ObjectResult;
              //  var actualtResult = result.Value;

                //Assert
                //Return proper HTTPStatus code\
                
                var item = (Categories)((CreatedAtActionResult)actionResult.Result).Value;
                //   Xunit.Assert.Equal(cat, (actionResult.Result as CreatedAtActionResult).Value); // SHOULD WORK!!
                output.WriteLine("This is output from {0}", item.Cat_name);
                Xunit.Assert.IsType<Categories>(item);

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

    }
}
