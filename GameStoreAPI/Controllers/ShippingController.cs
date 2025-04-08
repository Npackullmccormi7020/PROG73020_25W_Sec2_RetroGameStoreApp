//this controller handles SHIPPING info for orders in our Game Store.
//every time someone orders a game, this helps us track where it is going and when it shipped.
//you can fetch, add, update, or delete shipping records

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameStoreData;
using GameStoreData.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly GameDbContext _context;

        //inject the db context so we can access Shipping table
        public ShippingController(GameDbContext context)
        {
            _context = context;
        }

        //GET: api/Shipping
        //show all shipping records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShippingInfo>>> GetAllShippingInfo()
        {
            return await _context.Shippings.ToListAsync();
        }

        //GET: api/Shipping/5
        //get one specific shipping entry
        [HttpGet("{id}")]
        public async Task<ActionResult<ShippingInfo>> GetShippingById(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);

            if (shipping == null)
                return NotFound();          //nothing found? 404

            return shipping;
        }

        //POST: api/Shipping
        //add new shipping info (like after placing an order)
        [HttpPost]
        public async Task<ActionResult<ShippingInfo>> AddShippingInfo(ShippingInfo shipping)
        {
            _context.Shippings.Add(shipping);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShippingById), new { id = shipping.ShippingId }, shipping);
        }

        //PUT: api/Shipping/5
        //update shipping info (maybe a new address or delay)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateShippingInfo(int id, ShippingInfo shipping)
        {
            if (id != shipping.ShippingId)
                return BadRequest();        //mismatch = nope

            _context.Entry(shipping).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();      //save updates
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Shippings.Any(s => s.ShippingId == id))
                    return NotFound();

                throw;
            }

            return NoContent();         //all good
        }

        //DELETE: api/Shipping/5
        //remove shipping entry
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShippingInfo(int id)
        {
            var shipping = await _context.Shippings.FindAsync(id);

            if (shipping == null)
                return NotFound();

            _context.Shippings.Remove(shipping);
            await _context.SaveChangesAsync();

            return NoContent();         //deleted
        }
    }
}