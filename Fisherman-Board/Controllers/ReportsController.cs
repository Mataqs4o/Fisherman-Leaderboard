using Fisherman_Board.Data;
using Fisherman_Board.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Fisherman_Board.Controllers;

[Authorize]
public class ReportsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ReportsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new ReportsIndexViewModel
        {
            DatabaseAvailable = CanConnect()
        };

        if (!model.DatabaseAvailable)
        {
            return View(model);
        }

        var now = DateTime.UtcNow;
        var nextMonth = now.AddMonths(1);
        var yearStart = new DateTime(now.Year, 1, 1);

        model.ExpiringPermitCount = await _context.FishingPermits
            .AsNoTracking()
            .CountAsync(permit => !permit.IsRevoked && permit.ValidTo >= now && permit.ValidTo <= nextMonth);

        model.RecreationalRankingCount = await _context.RecreationalCatches
            .AsNoTracking()
            .Where(catchRecord => catchRecord.CatchDate >= now.AddYears(-1))
            .GroupBy(catchRecord => catchRecord.RecreationalTicket.PersonId)
            .CountAsync();

        model.VesselStatsCount = await _context.FishingTrips
            .AsNoTracking()
            .Where(trip => trip.StartTime >= yearStart)
            .GroupBy(trip => trip.FishingVesselId)
            .CountAsync();

        model.CarbonRankingCount = await GetCarbonFootprintQuery().CountAsync();

        var leader = await _context.RecreationalCatches
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

        var lowestFootprint = await GetCarbonFootprintQuery()
            .FirstOrDefaultAsync();

        model.RecreationalLeaderSummary = leader is null
            ? "Няма данни за класацията на любителите."
            : $"{leader.Fisher} е водещ рибар любител с {leader.TotalKg:F1} кг.";

        model.CarbonLeaderSummary = lowestFootprint is null
            ? "Няма валидни данни за въглеродния отпечатък."
            : $"{lowestFootprint.VesselMarking} е най-ефективният кораб с {lowestFootprint.CarbonPerKg:F2} л/кг.";

        return View(model);
    }

    public async Task<IActionResult> ExpiringPermits(string? search)
    {
        var now = DateTime.UtcNow;
        var nextMonth = now.AddMonths(1);

        ViewData["Search"] = search;

        if (!CanConnect())
        {
            return View(Array.Empty<ExpiringPermitViewModel>());
        }

        var query = _context.FishingPermits
            .AsNoTracking()
            .Where(permit => !permit.IsRevoked && permit.ValidTo >= now && permit.ValidTo <= nextMonth)
            .Select(permit => new ExpiringPermitViewModel
            {
                VesselMarking = permit.FishingVessel.Marking,
                InternationalNumber = permit.FishingVessel.InternationalNumber,
                ValidTo = permit.ValidTo
            });

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item =>
                item.VesselMarking.Contains(search) ||
                item.InternationalNumber.Contains(search));
        }

        var model = await query
            .OrderBy(item => item.ValidTo)
            .ToListAsync();

        return View(model);
    }

    public async Task<IActionResult> TopRecreational(string? search)
    {
        var oneYearAgo = DateTime.UtcNow.AddYears(-1);

        ViewData["Search"] = search;

        if (!CanConnect())
        {
            return View(Array.Empty<TopRecreationalViewModel>());
        }

        var query = _context.RecreationalCatches
            .AsNoTracking()
            .Where(catchRecord => catchRecord.CatchDate >= oneYearAgo)
            .GroupBy(catchRecord => new
            {
                catchRecord.RecreationalTicket.PersonId,
                catchRecord.RecreationalTicket.Person.FullName
            })
            .Select(group => new TopRecreationalViewModel
            {
                FisherName = group.Key.FullName,
                TotalKg = group.Sum(item => item.QuantityKg)
            });

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item => item.FisherName.Contains(search));
        }

        var model = await query
            .OrderByDescending(item => item.TotalKg)
            .ToListAsync();

        return View(model);
    }

    public async Task<IActionResult> VesselStats(string? search)
    {
        var yearStart = new DateTime(DateTime.UtcNow.Year, 1, 1);

        ViewData["Search"] = search;

        if (!CanConnect())
        {
            return View(Array.Empty<VesselStatsViewModel>());
        }

        var query = _context.FishingTrips
            .AsNoTracking()
            .Where(trip => trip.StartTime >= yearStart)
            .GroupBy(trip => new
            {
                trip.FishingVesselId,
                trip.FishingVessel.Marking
            })
            .Select(group => new VesselStatsViewModel
            {
                VesselMarking = group.Key.Marking,
                TripsCount = group.Count(),
                TotalCatchKg = group.Sum(trip => trip.Catches.Sum(catchRecord => catchRecord.QuantityKg)),
                AvgTripDurationHours = group.Average(trip => EF.Functions.DateDiffMinute(trip.StartTime, trip.EndTime)) / 60.0,
                MinTripDurationHours = group.Min(trip => EF.Functions.DateDiffMinute(trip.StartTime, trip.EndTime)) / 60.0,
                MaxTripDurationHours = group.Max(trip => EF.Functions.DateDiffMinute(trip.StartTime, trip.EndTime)) / 60.0,
                AvgCatchPerTrip = group.Average(trip => trip.Catches.Sum(catchRecord => catchRecord.QuantityKg)),
                MinCatchPerTrip = group.Min(trip => trip.Catches.Sum(catchRecord => catchRecord.QuantityKg)),
                MaxCatchPerTrip = group.Max(trip => trip.Catches.Sum(catchRecord => catchRecord.QuantityKg))
            });

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item => item.VesselMarking.Contains(search));
        }

        var model = await query
            .OrderByDescending(item => item.TotalCatchKg)
            .ToListAsync();

        return View(model);
    }

    public async Task<IActionResult> CarbonFootprint(string? search)
    {
        ViewData["Search"] = search;

        if (!CanConnect())
        {
            return View(Array.Empty<CarbonFootprintViewModel>());
        }

        var query = GetCarbonFootprintQuery();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(item => item.VesselMarking.Contains(search));
        }

        var model = await query.ToListAsync();

        return View(model);
    }

    private IQueryable<CarbonFootprintViewModel> GetCarbonFootprintQuery()
    {
        var year = DateTime.UtcNow.Year;
        var yearStart = new DateTime(year, 1, 1);
        var yearEndExclusive = yearStart.AddYears(1);
        var validToCutoff = yearEndExclusive.AddDays(-1);

        var validVessels = _context.FishingPermits
            .AsNoTracking()
            .Where(permit => !permit.IsRevoked && permit.ValidTo >= validToCutoff)
            .Select(permit => permit.FishingVesselId)
            .Distinct();

        return _context.FishingTrips
            .AsNoTracking()
            .Where(trip => trip.StartTime >= yearStart &&
                           trip.EndTime < yearEndExclusive &&
                           validVessels.Contains(trip.FishingVesselId))
            .GroupBy(trip => new
            {
                trip.FishingVesselId,
                trip.FishingVessel.Marking,
                FuelPerHour = trip.FishingVessel.Engine.AvgFuelPerHour
            })
            .Select(group => new
            {
                VesselMarking = group.Key.Marking,
                TotalCatchKg = group.Sum(trip => trip.Catches.Sum(catchRecord => catchRecord.QuantityKg)),
                TotalHours = group.Sum(trip => EF.Functions.DateDiffMinute(trip.StartTime, trip.EndTime)) / 60.0,
                FuelPerHour = group.Key.FuelPerHour
            })
            .Where(item => item.TotalCatchKg > 0)
            .Select(item => new CarbonFootprintViewModel
            {
                VesselMarking = item.VesselMarking,
                TotalCatchKg = item.TotalCatchKg,
                TotalFuel = item.TotalHours * item.FuelPerHour,
                CarbonPerKg = (item.TotalHours * item.FuelPerHour) / item.TotalCatchKg
            })
            .OrderBy(item => item.CarbonPerKg);
    }

    private bool CanConnect()
    {
        try
        {
            return _context.Database.CanConnect();
        }
        catch
        {
            return false;
        }
    }
}
