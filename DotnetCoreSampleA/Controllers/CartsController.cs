using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DotnetCoreSampleA.Data;
using DotnetCoreSampleA.Models;
using Microsoft.Extensions.Logging;

namespace DotnetCoreSampleA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public CartsController(ApplicationDbContext context, ILogger<CartsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("GetCartCount/{userid}")]
        public async Task<ActionResult<long>> GetCartCount(string userid)
        {
            if (CheckCartExistsForUser(userid))
            {
                var result = await _context.Cart.SingleOrDefaultAsync(p => p.UserID == userid && p.Status == "PENDING");
                return Ok(result.TotalQty);
            }
            else
            {
                long zero =0;
                return Ok(zero);
            }
        }

        // GET: api/Carts
        [HttpGet("{userid}")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCart(string userid)
        {
            var result = await _context.Cart.Where(p => p.UserID == userid&&p.Status=="PENDING").Include(p => p.CartDetails).Select(x =>
   new Cart { Cart_id = x.Cart_id, UserID = x.UserID, TotalAmount = x.TotalAmount, TotalQty = x.TotalQty, Status = x.Status,CartDetails= x.CartDetails.Select(x => new CartDetails { CD_id = x.CD_id, CD_Pr_id = x.CD_Pr_id, ProductForeignKey = x.ProductForeignKey, CD_Pr_Amnt = x.CD_Pr_Amnt, CD_Pr_price = x.CD_Pr_price, CD_Pr_Qty = x.CD_Pr_Qty, CartForeignKey = x.CartForeignKey, Product = x.Product })
            .ToList()
   }).ToListAsync();
            return Ok(result);
        }
        [HttpGet("GetCartByID/{userid}/{cartid}")]
        public async Task<ActionResult<IEnumerable<Cart>>> GetCartByID(string userid, long cartid)
        {
            var result = await _context.Cart.Where(p => p.UserID == userid && p.Cart_id == cartid && p.Status == "CONFIRMED").Include(p => p.CartDetails).Select(x =>
       new Cart
       {
           Cart_id = x.Cart_id,
           UserID = x.UserID,
           TotalAmount = x.TotalAmount,
           TotalQty = x.TotalQty,
           Status = x.Status,
           CartDetails = x.CartDetails.Select(x => new CartDetails { CD_id = x.CD_id, CD_Pr_id = x.CD_Pr_id, ProductForeignKey = x.ProductForeignKey, CD_Pr_Amnt = x.CD_Pr_Amnt, CD_Pr_price = x.CD_Pr_price, CD_Pr_Qty = x.CD_Pr_Qty, CartForeignKey = x.CartForeignKey, Product = x.Product })
                .ToList()
       }).ToListAsync();
            return Ok(result);
        }

        // POST: api/Carts
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            if (CheckCartExistsForUser(cart.UserID))
            {
                var cartid = GetPendingCartIDForUser(cart.UserID);
                var prid = cart.CartDetails.FirstOrDefault().CD_Pr_id;
                var cartdeatilsid = GetProductIDPendingCartIDForUser(cartid, prid);
                if (CheckCartDetailsProductExists(cartid, prid)){
                    //var totalqty = _context.Cart.Where(t => t.Cart_id == cartid).Sum(i => i.Price);
                    var prqty = _context.CartDetails.Where(t => t.CartForeignKey == cartid&& t.CD_Pr_id==prid).Select(u => u.CD_Pr_Qty)
                    .SingleOrDefault() + 1;
                    var prprice = getProductPrice(prid); //_context.CartDetails.Where(t => t.CartForeignKey == cartid).Select(u => u.CD_Pr_price).SingleOrDefault();
                    var totalpramnt = prqty * prprice;
                    var prdid = cart.CartDetails.FirstOrDefault().CD_Pr_id;
                    CartDetails cartDetails= new CartDetails {CD_id= cartdeatilsid, CD_Pr_id = prdid, ProductForeignKey = prdid, CD_Pr_Amnt = totalpramnt, CD_Pr_price = prprice,CD_Pr_Qty= prqty,CartForeignKey= cartid };
                    //_context.CartDetails.Add(cartDetails);
                    _context.Entry(cartDetails).State = EntityState.Modified;
                    await _context.SaveChangesAsync();         
                }
                else
                {
                    var prqty = 1;//cart.CartDetails.FirstOrDefault().CD_Pr_Qty;
                    var prprice = getProductPrice(prid);// cart.CartDetails.FirstOrDefault().CD_Pr_price;
                    var totalpramnt = prqty * prprice;
                    var prdid = cart.CartDetails.FirstOrDefault().CD_Pr_id;
                    CartDetails cartDetails = new CartDetails { CD_id = cartdeatilsid, CD_Pr_id = prdid, ProductForeignKey = prdid, CD_Pr_Amnt = totalpramnt, CD_Pr_price = prprice, CD_Pr_Qty = prqty, CartForeignKey = cartid };
                    _context.CartDetails.Add(cartDetails);
                    //_context.Entry(cartDetails).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                var prtotalqty = _context.CartDetails.Where(t => t.CartForeignKey == cartid).Sum(i => i.CD_Pr_Qty);
                var totalamnt = _context.CartDetails.Where(t => t.CartForeignKey == cartid).Sum(i => i.CD_Pr_Amnt);
             //   var totalamnt = prtotalqty * prtotalprice;
                Cart cartDx = new Cart { Cart_id = cartid, TotalAmount = totalamnt, TotalQty = prtotalqty, Status = "PENDING", UserID = cart.UserID };
                _context.Entry(cartDx).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCart", new { id = cartDx.Cart_id }, cart);
            }
            else
            {
                var prdid = cart.CartDetails.FirstOrDefault().CD_Pr_id;
                var prqty = 1;//cart.CartDetails.FirstOrDefault().CD_Pr_Qty;
                var prprice = getProductPrice(prdid);// cart.CartDetails.FirstOrDefault().CD_Pr_price;
                var totalpramnt = prqty * prprice;             
                Cart cartDx = new Cart { Cart_id = 0, TotalAmount = totalpramnt, TotalQty = prqty, Status = "PENDING", UserID = cart.UserID };
                _context.Cart.Add(cartDx);
                await _context.SaveChangesAsync();

                var result = cartDx.Cart_id;

                CartDetails cartDetails = new CartDetails { CD_id = 0, CD_Pr_id = prdid, ProductForeignKey = prdid, CD_Pr_Amnt = totalpramnt, CD_Pr_price = prprice, CD_Pr_Qty = prqty, CartForeignKey = result };
                _context.CartDetails.Add(cartDetails);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCart", new { id = cartDx.Cart_id }, cart);
            }
        }

        [HttpPost("DeleteProduct")]
        public async Task<ActionResult<Cart>> DetelteProducctFromCart(Cart cart)
        {
            if (CheckCartExistsForUser(cart.UserID))
            {
                var cartid = GetPendingCartIDForUser(cart.UserID);
                var prid = cart.CartDetails.FirstOrDefault().CD_Pr_id;

                var cartdeatilsid = GetProductIDPendingCartIDForUser(cartid, prid);

                if (CheckCartDetailsProductExists(cartid, prid))
                {
                    var cartDetailsdel = await _context.CartDetails.Where(t => t.CartForeignKey == cartid && t.CD_Pr_id == prid).ToListAsync();
                    if (cartDetailsdel == null)
                    {
                        return NotFound();
                    }
                    _context.CartDetails.Remove(cartDetailsdel.FirstOrDefault());
                    await _context.SaveChangesAsync();

                }
                else
                {
                    return NotFound();
                }
                var prtotalqty = _context.CartDetails.Where(t => t.CartForeignKey == cartid).Sum(i => i.CD_Pr_Qty);
                var totalamnt = _context.CartDetails.Where(t => t.CartForeignKey == cartid).Sum(i => i.CD_Pr_Amnt);
            //    var totalamnt = prtotalqty * prtotalprice;
                Cart cartDx = new Cart { Cart_id = cartid, TotalAmount = totalamnt, TotalQty = prtotalqty, Status = "PENDING", UserID = cart.UserID };
                _context.Entry(cartDx).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCart", new { id = cartDx.Cart_id }, cartDx);
            }
            else
            {
                return NotFound();

            }
        }
        [HttpPost("Checkout")]
        public async Task<ActionResult<Cart>> CheckoutCart(Cart cart)
        {
            if (CheckCartExistsForUser(cart.UserID))
            {
                var cartid = GetPendingCartIDForUser(cart.UserID);
                var prtotalqty = _context.CartDetails.Where(t => t.CartForeignKey == cartid).Sum(i => i.CD_Pr_Qty);
                var totalamnt = _context.CartDetails.Where(t => t.CartForeignKey == cartid).Sum(i => i.CD_Pr_Amnt);
                //    var totalamnt = prtotalqty * prtotalprice;
                Cart cartDx = new Cart { Cart_id = cartid, TotalAmount = totalamnt, TotalQty = prtotalqty, Status = "CONFIRMED", UserID = cart.UserID };
                _context.Entry(cartDx).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetCart", new { id = cartDx.Cart_id }, cartDx);
            }
            else
            {
                return NotFound();
            }
        }

        /*  private bool CartExists(long id)
          {
              return _context.Cart.Any(e => e.Cart_id == id);
          }*/
        private bool CheckCartExistsForUser(string user)
        {
            return _context.Cart.Any(e => e.UserID == user && e.Status=="PENDING");
        }
        private long GetPendingCartIDForUser(string user)
        { 

            return _context.Cart.Where(u => u.UserID == user && u.Status == "PENDING")
                    .Select(u => u.Cart_id)
                    .SingleOrDefault();
        }
        private bool CheckCartDetailsProductExists(long cid, long prid)
        {
            return _context.CartDetails.Any(e => e.CartForeignKey == cid && e.CD_Pr_id == prid);
        }
        private long GetProductIDPendingCartIDForUser(long cid,long prid)
        {

            return _context.CartDetails.Where(u => u.CartForeignKey == cid && u.CD_Pr_id == prid)
                    .Select(u => u.CD_id)
                    .SingleOrDefault();
        }
        private long getProductPrice(long id)
        {
            return _context.Product.Where(u => u.Pr_id==id)
                    .Select(u => u.Pr_price)
                    .SingleOrDefault();
        }
    }
}
