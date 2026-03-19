using System.Diagnostics;
using Fisherman_Board.Data;
using Fisherman_Board.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fisherman_Board.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext dbContext, ILogger<HomeController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var model = new DashboardViewModel
            {
                Fishermen = await _dbContext.Fisherman
                    .AsNoTracking()
                    .OrderBy(item => item.FullName)
                    .ToListAsync(),
                Boats = await _dbContext.Boats
                    .AsNoTracking()
                    .Include(item => item.Fisherman)
                    .OrderBy(item => item.Name)
                    .ToListAsync(),
                Hunts = await _dbContext.Hunt
                    .AsNoTracking()
                    .Include(item => item.Fisherman)
                    .Include(item => item.Boat)
                    .OrderByDescending(item => item.StartedAt)
                    .ToListAsync()
            };

            return View(model);
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
