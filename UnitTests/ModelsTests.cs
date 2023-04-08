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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotnetCoreSampleA.Models;

namespace UnitTests
{
    [TestClass]
    public class ModelsTests
    {
        [Fact]
        public  void Test_Login_Model()
        {
            var login = new LoginModel { UserName ="tayyab", Password = "test@test.fr" };
            Xunit.Assert.Equal("tayyab", login.UserName);
            Xunit.Assert.Equal("test@test.fr", login.Password);
        }
        [Fact]
        public void Test_ApplicationSettings()
        {
            var applicationSettings = new ApplicationSettings { JWT_Secret = "tayyab", Client_URL = "test@test.fr" };
            Xunit.Assert.Equal("tayyab", applicationSettings.JWT_Secret);
            Xunit.Assert.Equal("test@test.fr", applicationSettings.Client_URL);
        }
        [Fact]
        public void Test_ApplicationUser()
        {
            var applicationSettings = new ApplicationUser { FullName = "tayyab",UserName = "tayyab", Email = "tayyab" };
            Xunit.Assert.Equal("tayyab", applicationSettings.FullName);
        }
        [Fact]
        public void Test_ApplicationUserModel()
        {
            var applicationSettings = new ApplicationUserModel { FullName = "tayyab", UserName = "tayyab", Email = "tayyab",Role="Admin",Password="1234" };
            Xunit.Assert.Equal("tayyab", applicationSettings.FullName);
            Xunit.Assert.Equal("tayyab", applicationSettings.UserName);
            Xunit.Assert.Equal("tayyab", applicationSettings.Email);
            Xunit.Assert.Equal("Admin", applicationSettings.Role);
            Xunit.Assert.Equal("1234", applicationSettings.Password);
        }
        [Fact]
        public void Test_CartDetails()
        {
            
            var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1, Product=null,Cart=null };
           // List<CartDetails> cartx = new List<CartDetails>();
           // cartx.Add(cartdetils1);
          //  var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };
            var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc" };
            
            
            Xunit.Assert.Equal(1, cartdetils1.CD_id);
            Xunit.Assert.Equal(1, cartdetils1.CD_Pr_id);
            Xunit.Assert.Equal(1, cartdetils1.ProductForeignKey);
            Xunit.Assert.Equal(100, cartdetils1.CD_Pr_Amnt);
            Xunit.Assert.Equal(100, cartdetils1.CD_Pr_price);
            Xunit.Assert.Equal(1, cartdetils1.CD_Pr_Qty);
            Xunit.Assert.Equal(1, cartdetils1.CartForeignKey);
            Xunit.Assert.Null(cartdetils1.Product);
            Xunit.Assert.Null(cartdetils1.Cart);

        }
        [Fact]
        public void Test_Cart()
        {

            var cartdetils1 = new CartDetails { CD_id = 1, CD_Pr_id = 1, ProductForeignKey = 1, CD_Pr_Amnt = 100, CD_Pr_price = 100, CD_Pr_Qty = 1, CartForeignKey = 1, Product = null, Cart = null };
            List<CartDetails> cartx = new List<CartDetails>();
             cartx.Add(cartdetils1);
              var manager = new Cart { Cart_id = 1, UserID = "123", TotalQty = 2, TotalAmount = 100, Status = "PENDING", CartDetails = cartx };
            

            Xunit.Assert.Equal(1, cartdetils1.CD_id);
            Xunit.Assert.Equal(1, cartdetils1.CD_Pr_id);
            Xunit.Assert.Equal(1, cartdetils1.ProductForeignKey);
            Xunit.Assert.Equal(100, cartdetils1.CD_Pr_Amnt);
            Xunit.Assert.Equal(100, cartdetils1.CD_Pr_price);
            Xunit.Assert.Equal(1, cartdetils1.CD_Pr_Qty);
            Xunit.Assert.Equal(1, cartdetils1.CartForeignKey);
            Xunit.Assert.Null(cartdetils1.Product);
            Xunit.Assert.Null(cartdetils1.Cart);

            Xunit.Assert.Equal(1, manager.Cart_id);
            Xunit.Assert.Equal("123", manager.UserID);
            Xunit.Assert.Equal(2, manager.TotalQty);
            Xunit.Assert.Equal(100, manager.TotalAmount);
            Xunit.Assert.Equal("PENDING", manager.Status);
            Xunit.Assert.Equal(cartx, manager.CartDetails);

        }
        [Fact]
        public void Test_Product()
        {
            var manager2 = new Product { Pr_id = 4, Pr_name = "Test", Pr_price = 100, Pr_Picture = "", Pr_desc = "Test desc",Category=null,CategoryForeignKey=1 };
            Xunit.Assert.Equal(4, manager2.Pr_id);
            Xunit.Assert.Equal("Test", manager2.Pr_name);
            Xunit.Assert.Equal(100, manager2.Pr_price);
            Xunit.Assert.Equal("", manager2.Pr_Picture);
            Xunit.Assert.Equal("Test desc", manager2.Pr_desc);
            Xunit.Assert.Equal(1, manager2.CategoryForeignKey);
            Xunit.Assert.Null(manager2.Category);

        }

    }
}
