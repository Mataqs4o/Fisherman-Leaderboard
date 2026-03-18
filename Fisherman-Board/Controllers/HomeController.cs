using System.Diagnostics;
using Fisherman_Board.Data;
using Fisherman_Board.Models;
using Fisherman_Board.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fisherman_Board.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new HomeDashboardViewModel
        {
            DatabaseAvailable = CanConnect(),
            ReportCards =
            [
                new()
                {
                    Title = "Изтичащи разрешителни",
                    Description = "Следи корабите с разрешително, което изтича през следващите 30 дни.",
                    Controller = "Reports",
                    Action = "ExpiringPermits"
                },
                new()
                {
                    Title = "Класация на любителите",
                    Description = "Подрежда рибарите любители по общ улов за последната една година.",
                    Controller = "Reports",
                    Action = "TopRecreational"
                },
                new()
                {
                    Title = "Статистика за кораби",
                    Description = "Събира излети, продължителност и улов за активните кораби през годината.",
                    Controller = "Reports",
                    Action = "VesselStats"
                },
                new()
                {
                    Title = "Въглероден отпечатък",
                    Description = "Изчислява изразходваното гориво за 1 кг уловена риба.",
                    Controller = "Reports",
                    Action = "CarbonFootprint"
                }
            ],
            RegistryCards =
            [
                new()
                {
                    Title = "Fisherman",
                    Description = "Таблица с рибари, търсене и редакция на основните им данни.",
                    Controller = "Registry",
                    Action = "Fishermen"
                },
                new()
                {
                    Title = "Лодки",
                    Description = "Таблица с лодки и техните идентификационни и технически характеристики.",
                    Controller = "Registry",
                    Action = "Boats"
                },
                new()
                {
                    Title = "Улов",
                    Description = "Таблица с уловите по излет, вид риба и количество.",
                    Controller = "Registry",
                    Action = "Catches"
                }
            ]
        };

        if (!model.DatabaseAvailable)
        {
            return View(model);
        }

        var now = DateTime.UtcNow;
        var yearStart = new DateTime(now.Year, 1, 1);

        model.TotalVessels = await _context.FishingVessels.AsNoTracking().CountAsync();
        model.ActivePermits = await _context.FishingPermits
            .AsNoTracking()
            .CountAsync(permit => !permit.IsRevoked && permit.ValidTo >= now);
        model.TripsThisYear = await _context.FishingTrips
            .AsNoTracking()
            .CountAsync(trip => trip.StartTime >= yearStart);
        model.RecreationalTickets = await _context.RecreationalTickets
            .AsNoTracking()
            .CountAsync(ticket => ticket.ValidTo >= now);

        var topRecreational = await _context.RecreationalCatches
            .AsNoTracking()
            .Where(catchRecord => catchRecord.CatchDate >= now.AddYears(-1))
            .GroupBy(catchRecord => new
            {
                catchRecord.RecreationalTicket.PersonId,
                catchRecord.RecreationalTicket.Person.FullName
            })
            .Select(group => new
            {
                Fisher = group.Key.FullName,
                TotalKg = group.Sum(item => item.QuantityKg)
            })
            .OrderByDescending(item => item.TotalKg)
            .FirstOrDefaultAsync();

        var nextExpiring = await _context.FishingPermits
            .AsNoTracking()
            .Where(permit => !permit.IsRevoked && permit.ValidTo >= now)
            .OrderBy(permit => permit.ValidTo)
            .Select(permit => new
            {
                permit.FishingVessel.Marking,
                permit.ValidTo
            })
            .FirstOrDefaultAsync();

        model.TopInsight = topRecreational is null
            ? "Все още няма регистриран любителски улов за последната година."
            : $"{topRecreational.Fisher} води с {topRecreational.TotalKg:F1} кг общ улов.";

        model.AlertInsight = nextExpiring is null
            ? "Няма активни разрешителни с наближаващ срок."
            : $"{nextExpiring.Marking} е следващият кораб с изтичащо разрешително на {nextExpiring.ValidTo:dd.MM.yyyy}.";

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

    private bool CanConnect()
    {
        try
        {
            return _context.Database.CanConnect();
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "The dashboard could not connect to the configured database.");
            return false;
        }
    }
}
