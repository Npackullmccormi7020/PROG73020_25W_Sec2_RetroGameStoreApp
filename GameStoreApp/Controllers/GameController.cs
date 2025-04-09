using Microsoft.AspNetCore.Mvc;
using GameStoreData;
using GameStoreData.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApp.Controllers
{
    public class GameController : Controller
    {
        private readonly GameDbContext _context;

        public GameController(GameDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GameInfo(int id)
        {
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == id);
            if (game == null)
                return NotFound();

            return View("GameInfo", game);
        }

        [HttpGet]
        public IActionResult GetTrackingOrderPage()
        {
            return View("~/Views/Game/TrackingOrder.cshtml"); 
        }

        [HttpPost]
        public async Task<IActionResult> TrackOrder(int shippingId)
        {
            var order = await _context.Shippings
                .FirstOrDefaultAsync(s => s.ShippingId == shippingId);

            if (order == null)
            {
                ViewBag.Error = "This is not a valid order ID.";
                return View("~/Views/Game/TrackingOrder.cshtml");
            }

            return View("~/Views/Game/OrderReview.cshtml", order);
        }
    }
}
