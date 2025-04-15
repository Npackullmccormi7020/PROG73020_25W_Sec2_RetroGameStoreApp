//this controller is all about handling USERS in our Game Store.
//it lets us grab all users, fetch a specific one, add a new user, update them, or delete them.
//basically: CRUD operations for players in our system

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameStoreData;
using GameStoreData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;   //for authorization features

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]             //this requires authentication for ALL endpoints in this controller
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly GameDbContext _context;

        //inject the GameDbContext so we can use it to talk to the database
        public UsersController(GameDbContext context)
        {
            _context = context;
        }

        //GET: api/Users
        //this grabs a list of all users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        //GET: api/Users/2
        //gets a single user by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();          //if not found, tell them that

            return user;        //otherwise return the user
        }

        //POST: api/Users
        //add a new user to the system
        [HttpPost]
        public async Task<ActionResult<User>> AddUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            //return 201 Created with the route to access the new user
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        //PUT: api/Users/2
        //update an existing users info
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, User user)
        {
            if (id != user.UserId)
                return BadRequest();        //IDs must match

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();      //try saving changes
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Users.Any(u => u.UserId == id))
                    return NotFound();

                throw;          //something else went wrong
            }

            return NoContent();         //success, nothing to return
        }

        //DELETE: api/Users/2
        //delete a user by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();         //successfully removed
        }
    }
}