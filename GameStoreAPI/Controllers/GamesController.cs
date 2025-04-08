using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;         //for building API controllers
using Microsoft.EntityFrameworkCore;    //so we can use EF Core features like ToListAsync, FindAsync, etc.
using GameStoreData.Entities;           //where the Games model is defined
using GameStoreData;                    //our DbContext lives here

namespace GameStoreAPI.Controllers
{
    //route for all requests to this controller will start with "api/games"
    [Route("api/[controller]")]
    [ApiController]             //tells ASP.NET this is a REST-style controller
    public class GamesController : ControllerBase
    {
        private readonly GameDbContext _context;

        //constructor: injects the database context so we can use it inside the controller
        public GamesController(GameDbContext context)
        {
            _context = context;
        }

        //GET: api/Games
        //returns all games in the database
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Games>>> GetAllGames()
        {
            //grab all rows from the Games table
            return await _context.Games.ToListAsync();
        }

        //GET: api/Games/5
        //returns one game by its ID (like GET /api/games/3)
        [HttpGet("{id}")]
        public async Task<ActionResult<Games?>> GetGameById(int id)
        {
            var game = await _context.Games.FindAsync(id);

            if (game == null)
            {
                return NotFound();          //return 404 if not found
            }

            return game;
        }

        //PUT: api/Games/5
        //updates a game if it exists (like PUT /api/games/3)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGame(int id, Games game)
        {
            if (id != game.Id)
            {
                return BadRequest();        //ID in URL must match ID in body
            }

            //mark this game as modified in the EF tracker
            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();          //try saving changes
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GameExists(id))
                {
                    return NotFound();          //game was deleted before we could save
                }
                else
                {
                    throw;          //something else went wrong
                }
            }

            return NoContent();         //all good, but no content to return
        }

        //POST: api/Games
        //adds a new game to the database
        [HttpPost]
        public async Task<ActionResult<Games>> CreateGame(Games game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();

            //returns 201 Created + link to the new resource
            return CreatedAtAction(nameof(GetGameById), new { id = game.Id }, game);
        }

        //DELETE: api/Games/5
        //deletes a game by ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int id)
        {
            var game = await _context.Games.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            _context.Games.Remove(game);
            await _context.SaveChangesAsync();

            return NoContent();         //successfully deleted
        }

        //helper method to check if a game exists in the DB
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}