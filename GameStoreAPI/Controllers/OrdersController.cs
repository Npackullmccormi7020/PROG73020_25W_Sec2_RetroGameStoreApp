//this controller handles everything related to ORDERS in our Game Store.
//you can create a new order, view all orders, fetch a single one, update or delete it.
//it connects users to the games they have bought

using GameStoreData;
using GameStoreData.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;   //for authorization features

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]             //this requires authentication for ALL endpoints in this controller
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly GameDbContext _context;

        //inject db context so we can access the Orders table
        public OrdersController(GameDbContext context)
        {
            _context = context;
        }

        //GET: api/Orders
        //returns a list of all orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetAllOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        //GET: api/Orders/5
        //get one specific order by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound(); //if it is not there, let em know

            return order;
        }

        //POST: api/Orders
        //place a new order
        [HttpPost]
        public async Task<ActionResult<Order>> PlaceOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            //give back a 201 created response
            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }

        //PUT: api/Orders/5
        //update an existing order
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            if (id != order.OrderId)
                return BadRequest();        //mismatch in IDs? no bueno

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();          //try saving changes
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Orders.Any(e => e.OrderId == id))
                    return NotFound();

                throw;          //something else went wrong
            }

            return NoContent();         //updated, but nothing to return
        }

        //DELETE: api/Orders/5
        //cancel/delete an order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();     //poof, it is gone
        }
    }
}