using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetCoreSampleA.Data;
using DotnetCoreSampleA.Models;

namespace DotnetCoreSampleA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartDetailsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartDetailsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CartDetails
      /* [HttpGet]
        public async Task<ActionResult<CartDetails>> GetCartDetails()
        {
            long pr = 1;
            var cartDetailsdel = await _context.CartDetails.Where(t => t.CartForeignKey == 2 && t.CD_Pr_id == pr).ToListAsync();//await _context.CartDetails.FindAsync(pr);

            if (cartDetailsdel == null)
            {
                return NotFound();
            }
            return cartDetailsdel.FirstOrDefault();//await _context.CartDetails.ToListAsync();
        }

         // GET: api/CartDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CartDetails>> GetCartDetails(long id)
        {
            var cartDetails = await _context.CartDetails.FindAsync(id);

            if (cartDetails == null)
            {
                return NotFound();
            }

            return cartDetails;
        }

        // PUT: api/CartDetails/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCartDetails(long id, CartDetails cartDetails)
        {
            if (id != cartDetails.CD_id)
            {
                return BadRequest();
            }

            _context.Entry(cartDetails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartDetailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CartDetails
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CartDetails>> PostCartDetails(CartDetails cartDetails)
        {
            _context.CartDetails.Add(cartDetails);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCartDetails", new { id = cartDetails.CD_id }, cartDetails);
        }

        // DELETE: api/CartDetails/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<CartDetails>> DeleteCartDetails(long id)
        {
            var cartDetails = await _context.CartDetails.FindAsync(id);
            if (cartDetails == null)
            {
                return NotFound();
            }

            _context.CartDetails.Remove(cartDetails);
            await _context.SaveChangesAsync();

            return cartDetails;
        }

        private bool CartDetailsExists(long id)
        {
            return _context.CartDetails.Any(e => e.CD_id == id);
        }*/
    }
}
