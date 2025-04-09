using System.Diagnostics;
using GameStoreApp.Models;
using GameStoreData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameStoreApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GameDbContext _context;

        public HomeController(ILogger<HomeController> logger, GameDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public async Task <IActionResult> Index()
        {
            var games = await _context.Games.ToListAsync();
            return View(games);
        }

        [HttpGet]
        public IActionResult GetAccountPage()
        {
            return View("~/Views/Game/Login.cshtml");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
