using Fisherman_Board.Data;
using Fisherman_Board.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Fisherman_Board.Data;
using Fisherman_Board.Models;


namespace Fisherman_Board.Models

{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Справка 1: изтичащи разрешителни
        public IActionResult ExpiringPermits()
        {
            var now = DateTime.UtcNow;
            var nextMonth = now.AddMonths(1);

            var model = _context.FishingPermits
                .Include(fp => fp.FishingVessel)
                .Where(fp => !fp.IsRevoked &&
                             fp.ValidTo >= now &&
                             fp.ValidTo <= nextMonth)
                .Select(fp => new ExpiringPermitViewModel
                {
                    VesselMarking = fp.FishingVessel.Marking,
                    InternationalNumber = fp.FishingVessel.InternationalNumber,
                    ValidTo = fp.ValidTo
                })
                .ToList();

            return View(model);
        }

        // Справка 2: топ любители
        public IActionResult TopRecreational()
        {
            var oneYearAgo = DateTime.UtcNow.AddYears(-1);

            var model = _context.RecreationalCatches
                .Include(rc => rc.RecreationalTicket)
                    .ThenInclude(t => t.Person)
                .Where(rc => rc.CatchDate >= oneYearAgo)
                .GroupBy(rc => rc.RecreationalTicket.Person)
                .Select(g => new TopRecreationalViewModel
                {
                    FisherName = g.Key.FullName,
                    TotalKg = g.Sum(x => x.QuantityKg)
                })
                .OrderByDescending(x => x.TotalKg)
                .ToList();

            return View(model);
        }

        // Справка 3: статистика за кораби
        public IActionResult VesselStats()
        {
            var yearStart = new DateTime(DateTime.UtcNow.Year, 1, 1);

            var model = _context.FishingTrips
                .Include(t => t.FishingVessel)
                .Include(t => t.Catches)
                .Where(t => t.StartTime >= yearStart)
                .GroupBy(t => t.FishingVessel)
                .Select(g => new VesselStatsViewModel
                {
                    VesselMarking = g.Key.Marking,
                    TripsCount = g.Count(),
                    TotalCatchKg = g.Sum(t => t.Catches.Sum(c => c.QuantityKg)),
                    AvgTripDurationHours = g.Average(t => EF.Functions.DateDiffMinute(t.StartTime, t.EndTime)) / 60.0,
                    MinTripDurationHours = g.Min(t => EF.Functions.DateDiffMinute(t.StartTime, t.EndTime)) / 60.0,
                    MaxTripDurationHours = g.Max(t => EF.Functions.DateDiffMinute(t.StartTime, t.EndTime)) / 60.0,
                    AvgCatchPerTrip = g.Average(t => t.Catches.Sum(c => c.QuantityKg)),
                    MinCatchPerTrip = g.Min(t => t.Catches.Sum(c => c.QuantityKg)),
                    MaxCatchPerTrip = g.Max(t => t.Catches.Sum(c => c.QuantityKg))
                })
                .OrderByDescending(x => x.TotalCatchKg)
                .ToList();

            return View(model);
        }

        // Справка 4: въглероден отпечатък
        public IActionResult CarbonFootprint()
        {
            var year = DateTime.UtcNow.Year;
            var yearStart = new DateTime(year, 1, 1);
            var yearEnd = new DateTime(year, 12, 31);

            var validVessels = _context.FishingPermits
                .Where(fp => !fp.IsRevoked &&
                             fp.ValidTo >= yearEnd)
                .Select(fp => fp.FishingVesselId)
                .Distinct();

            var model = _context.FishingTrips
                .Include(t => t.FishingVessel)
                    .ThenInclude(v => v.Engine)
                .Include(t => t.Catches)
                .Where(t => t.StartTime >= yearStart &&
                            t.EndTime <= yearEnd &&
                            validVessels.Contains(t.FishingVesselId))
                .GroupBy(t => t.FishingVessel)
                .Select(g => new
                {
                    VesselMarking = g.Key.Marking,
                    TotalCatchKg = g.Sum(t => t.Catches.Sum(c => c.QuantityKg)),
                    TotalHours = g.Sum(t => EF.Functions.DateDiffMinute(t.StartTime, t.EndTime)) / 60.0,
                    FuelPerHour = g.Key.Engine.AvgFuelPerHour
                })
                .Where(x => x.TotalCatchKg > 0)
                .Select(x => new CarbonFootprintViewModel
                {
                    VesselMarking = x.VesselMarking,
                    TotalCatchKg = x.TotalCatchKg,
                    TotalFuel = x.TotalHours * x.FuelPerHour,
                    CarbonPerKg = (x.TotalHours * x.FuelPerHour) / x.TotalCatchKg
                })
                .OrderBy(x => x.CarbonPerKg)
                .ToList();

            return View(model);
        }
    }
}
