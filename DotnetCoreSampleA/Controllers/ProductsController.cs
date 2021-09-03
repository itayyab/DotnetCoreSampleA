using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetCoreSampleA.Data;
using DotnetCoreSampleA.Models;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace DotnetCoreSampleA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
        {
            _context = context;
            _logger = logger;

        }

        // GET: api/Products
        [HttpGet("ByCategory")]
        public async Task<ActionResult<IEnumerable<Category>>> GetProductsByCategory()
        {
            var result = await _context.Categories.Include(p => p.Products).Select(x =>
   new Category { Cat_id = x.Cat_id, Cat_name = x.Cat_name, Products = x.Products.ToList() }).ToListAsync();
            return Ok(result);            
        }

        // GET: api/Products
        [HttpGet("ByCategory/{id}")]
        public async Task<ActionResult<IEnumerable<Category>>> GetProductsByCategoryID(long id)
        {
            var result = await _context.Categories.Where(p=>p.Cat_id==id).Include(p => p.Products).Select(x =>
   new Category { Cat_id = x.Cat_id, Cat_name = x.Cat_name, Products = x.Products.ToList() }).ToListAsync();
            return Ok(result);
        }
        [HttpGet("WithCategory")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductWithCat()
        {
            var result = await _context.Product.Include(p => p.Category.Products).Select(x =>
   new Product { Pr_id = x.Pr_id, Pr_desc = x.Pr_desc,Pr_name=x.Pr_name,Pr_Picture=x.Pr_Picture,Pr_price=x.Pr_price, Category=new Category {Cat_id=x.Category.Cat_id,Cat_name=x.Category.Cat_name } }).ToListAsync();
            return Ok(result);
        }
        [HttpGet("WithCategory/{id}")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductWithCatByID(long id)
        {
            var result = await _context.Product.Where(p=>p.Pr_id==id).Include(p => p.Category.Products).Select(x =>
   new Product { Pr_id = x.Pr_id, Pr_desc = x.Pr_desc, Category = new Category { Cat_id = x.Category.Cat_id, Cat_name = x.Category.Cat_name } }).ToListAsync();
            return Ok(result);
        }


        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var result = await _context.Product.ToListAsync();
            return Ok(result);
        }



        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(long id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> PutProduct(long id, Product product)
        {
            if (id != product.Pr_id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Products
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();
           
            return CreatedAtAction("GetProduct", new { id = product.Pr_id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<Product>> DeleteProduct(long id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        private bool ProductExists(long id)
        {
            return _context.Product.Any(e => e.Pr_id == id);
        }
        [HttpPost("UploadFile")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<String>> OnPostUploadAsync(IFormFile file)
        {

           // var filePath = System.AppContext.BaseDirectory+"/Images/";//Path.GetTempPath();
            var folderName = Path.Combine("StaticFiles", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var fullPath = Path.Combine(pathToSave, fileName);

            _logger.LogDebug("file."+ fullPath);
            var dbPath = Path.Combine(folderName, fileName);
            using (var stream = System.IO.File.Create(fullPath))
                    {
                        await file.CopyToAsync(stream);
                    }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(dbPath);
        }
    }
}
