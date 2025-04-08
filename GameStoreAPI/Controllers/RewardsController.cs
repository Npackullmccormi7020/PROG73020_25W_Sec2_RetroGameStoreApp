//this controller handles REWARDS for users in our Game Store.
//it helps us manage point balances, like getting points for buying games.
//we can fetch, create, update, or delete reward records

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GameStoreData;
using GameStoreData.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RewardsController : ControllerBase
    {
        private readonly GameDbContext _context;

        //inject the database context so we can talk to Rewards table
        public RewardsController(GameDbContext context)
        {
            _context = context;
        }

        //GET: api/Rewards
        //returns a list of all rewards (points) from the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rewards>>> GetAllRewards()
        {
            return await _context.Rewards.ToListAsync();
        }

        //GET: api/Rewards/2
        //fetch a specific reward record by its ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Rewards>> GetRewardById(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);

            if (reward == null)
                return NotFound();          //if not found, return 404

            return reward;
        }

        //POST: api/Rewards
        //create a new reward record (ex: new user starts with points)
        [HttpPost]
        public async Task<ActionResult<Rewards>> AddReward(Rewards reward)
        {
            _context.Rewards.Add(reward);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRewardById), new { id = reward.RewardId }, reward);
        }

        //PUT: api/Rewards/3
        //update an existing reward (ex: after a purchase)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReward(int id, Rewards reward)
        {
            if (id != reward.RewardId)
                return BadRequest();        //make sure ID in URL matches body

            _context.Entry(reward).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Rewards.Any(r => r.RewardId == id))
                    return NotFound();      //can't update what doesn't exist

                throw;      //something else blew up
            }

            return NoContent();     //success, nothing fancy to return
        }

        //DELETE: api/Rewards/3
        //remove a reward record (maybe if user deletes their account)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReward(int id)
        {
            var reward = await _context.Rewards.FindAsync(id);

            if (reward == null)
                return NotFound();

            _context.Rewards.Remove(reward);
            await _context.SaveChangesAsync();

            return NoContent();     //reward deleted
        }
    }
}