using Fisherman_Board.Data;
using Fisherman_Board.Models;
using Fisherman_Board.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fisherman_Board.Controllers;

[Authorize]
public class RegistryController : Controller
{
    private readonly ApplicationDbContext _context;

    public RegistryController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var model = new RegistryIndexViewModel
        {
            DatabaseAvailable = CanConnect()
        };

        if (!model.DatabaseAvailable)
        {
            return View(model);
        }

        model.FishermenCount = await _context.People.AsNoTracking().CountAsync();
        model.BoatsCount = await _context.FishingVessels.AsNoTracking().CountAsync();
        model.CatchesCount = await _context.CatchRecords.AsNoTracking().CountAsync();

        return View(model);
    }

    public async Task<IActionResult> Fishermen(string? search)
    {
        ViewData["Search"] = search;

        if (!CanConnect())
        {
            return View(Array.Empty<Person>());
        }

        var query = _context.People
            .AsNoTracking()
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(person => person.FullName.Contains(search));
        }

        var model = await query
            .OrderBy(person => person.FullName)
            .ToListAsync();

        return View(model);
    }

    public IActionResult CreateFisherman()
    {
        return View(new Person { BirthDate = DateTime.Today.AddYears(-18) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFisherman(Person person)
    {
        if (!ModelState.IsValid)
        {
            return View(person);
        }

        _context.People.Add(person);
        await _context.SaveChangesAsync();

        SetStatus("Рибарят беше добавен успешно.");
        return RedirectToAction(nameof(Fishermen));
    }

    public async Task<IActionResult> EditFisherman(int id)
    {
        var person = await _context.People.FindAsync(id);
        return person is null ? NotFound() : View(person);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFisherman(int id, Person person)
    {
        if (id != person.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(person);
        }

        _context.Update(person);
        await _context.SaveChangesAsync();

        SetStatus("Данните за рибаря бяха обновени.");
        return RedirectToAction(nameof(Fishermen));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFisherman(int id)
    {
        var person = await _context.People.FindAsync(id);
        if (person is null)
        {
            SetStatus("Рибарят не беше намерен.", "warning");
            return RedirectToAction(nameof(Fishermen));
        }

        try
        {
            _context.People.Remove(person);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            SetStatus("Рибарят не може да бъде изтрит, защото участва в други записи.", "warning");
            return RedirectToAction(nameof(Fishermen));
        }

        SetStatus("Рибарят беше изтрит.");
        return RedirectToAction(nameof(Fishermen));
    }

    public async Task<IActionResult> Engines(string? search)
    {
        ViewData["Search"] = search;

        if (!CanConnect())
        {
            return View(Array.Empty<Engine>());
        }

        var query = _context.Engines
            .AsNoTracking()
            .Include(engine => engine.Vessels)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(engine =>
                engine.Type.Contains(search) ||
                engine.FuelType.Contains(search));
        }

        var model = await query
            .OrderBy(engine => engine.Type)
            .ThenBy(engine => engine.FuelType)
            .ToListAsync();

        return View(model);
    }

    public IActionResult CreateEngine()
    {
        return View(new Engine());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateEngine(Engine engine)
    {
        if (!ModelState.IsValid)
        {
            return View(engine);
        }

        _context.Engines.Add(engine);
        await _context.SaveChangesAsync();

        SetStatus("Двигателят беше добавен успешно.");
        return RedirectToAction(nameof(Engines));
    }

    public async Task<IActionResult> EditEngine(int id)
    {
        var engine = await _context.Engines.FindAsync(id);
        return engine is null ? NotFound() : View(engine);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditEngine(int id, Engine engine)
    {
        if (id != engine.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            return View(engine);
        }

        _context.Update(engine);
        await _context.SaveChangesAsync();

        SetStatus("Двигателят беше обновен.");
        return RedirectToAction(nameof(Engines));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteEngine(int id)
    {
        var engine = await _context.Engines
            .Include(item => item.Vessels)
            .FirstOrDefaultAsync(item => item.Id == id);

        if (engine is null)
        {
            SetStatus("Записът не беше намерен.", "warning");
            return RedirectToAction(nameof(Engines));
        }

        if (engine.Vessels.Count > 0)
        {
            SetStatus("Двигателят не може да бъде изтрит, защото е свързан с регистрирани кораби.", "warning");
            return RedirectToAction(nameof(Engines));
        }

        _context.Engines.Remove(engine);
        await _context.SaveChangesAsync();

        SetStatus("Двигателят беше изтрит.");
        return RedirectToAction(nameof(Engines));
    }

    public async Task<IActionResult> Vessels(string? search)
    {
        ViewData["Search"] = search;

        if (!CanConnect())
        {
            return View("Vessels", Array.Empty<FishingVessel>());
        }

        var query = _context.FishingVessels
            .AsNoTracking()
            .Include(vessel => vessel.Engine)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(vessel =>
                vessel.Marking.Contains(search) ||
                vessel.CallSign.Contains(search) ||
                vessel.InternationalNumber.Contains(search) ||
                vessel.Engine.Type.Contains(search));
        }

        var model = await query
            .OrderBy(vessel => vessel.Marking)
            .ToListAsync();

        return View("Vessels", model);
    }

    public Task<IActionResult> Boats(string? search) => Vessels(search);

    public async Task<IActionResult> CreateVessel()
    {
        await PopulateEngineOptionsAsync();
        return View("CreateVessel", new FishingVessel());
    }

    public Task<IActionResult> CreateBoat() => CreateVessel();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> CreateBoat(FishingVessel vessel) => CreateVessel(vessel);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateVessel(FishingVessel vessel)
    {
        if (!ModelState.IsValid)
        {
            await PopulateEngineOptionsAsync(vessel.EngineId);
            return View("CreateVessel", vessel);
        }

        _context.FishingVessels.Add(vessel);
        await _context.SaveChangesAsync();

        SetStatus("Лодката беше добавена успешно.");
        return RedirectToAction(nameof(Boats));
    }

    public async Task<IActionResult> EditVessel(int id)
    {
        var vessel = await _context.FishingVessels.FindAsync(id);
        if (vessel is null)
        {
            return NotFound();
        }

        await PopulateEngineOptionsAsync(vessel.EngineId);
        return View("EditVessel", vessel);
    }

    public Task<IActionResult> EditBoat(int id) => EditVessel(id);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> EditBoat(int id, FishingVessel vessel) => EditVessel(id, vessel);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditVessel(int id, FishingVessel vessel)
    {
        if (id != vessel.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateEngineOptionsAsync(vessel.EngineId);
            return View("EditVessel", vessel);
        }

        _context.Update(vessel);
        await _context.SaveChangesAsync();

        SetStatus("Лодката беше обновена.");
        return RedirectToAction(nameof(Boats));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVessel(int id)
    {
        var vessel = await _context.FishingVessels.FindAsync(id);
        if (vessel is null)
        {
            SetStatus("Лодката не беше намерена.", "warning");
            return RedirectToAction(nameof(Boats));
        }

        try
        {
            _context.FishingVessels.Remove(vessel);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            SetStatus("Лодката не може да бъде изтрита, защото има свързани разрешителни или излети.", "warning");
            return RedirectToAction(nameof(Boats));
        }

        SetStatus("Лодката беше изтрита.");
        return RedirectToAction(nameof(Boats));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public Task<IActionResult> DeleteBoat(int id) => DeleteVessel(id);

    public async Task<IActionResult> Catches(string? search)
    {
        ViewData["Search"] = search;

        if (!CanConnect())
        {
            return View(Array.Empty<CatchRecord>());
        }

        var query = _context.CatchRecords
            .AsNoTracking()
            .Include(catchRecord => catchRecord.FishingTrip)
                .ThenInclude(trip => trip.FishingVessel)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(catchRecord =>
                catchRecord.Species.Contains(search) ||
                catchRecord.FishingTrip.FishingVessel.Marking.Contains(search));
        }

        var model = await query
            .OrderByDescending(catchRecord => catchRecord.FishingTrip.StartTime)
            .ToListAsync();

        return View(model);
    }

    public async Task<IActionResult> CreateCatch()
    {
        await PopulateTripOptionsAsync();
        return View(new CatchRecord());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCatch(CatchRecord catchRecord)
    {
        if (!ModelState.IsValid)
        {
            await PopulateTripOptionsAsync(catchRecord.FishingTripId);
            return View(catchRecord);
        }

        _context.CatchRecords.Add(catchRecord);
        await _context.SaveChangesAsync();

        SetStatus("Уловът беше добавен успешно.");
        return RedirectToAction(nameof(Catches));
    }

    public async Task<IActionResult> EditCatch(int id)
    {
        var catchRecord = await _context.CatchRecords.FindAsync(id);
        if (catchRecord is null)
        {
            return NotFound();
        }

        await PopulateTripOptionsAsync(catchRecord.FishingTripId);
        return View(catchRecord);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCatch(int id, CatchRecord catchRecord)
    {
        if (id != catchRecord.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            await PopulateTripOptionsAsync(catchRecord.FishingTripId);
            return View(catchRecord);
        }

        _context.Update(catchRecord);
        await _context.SaveChangesAsync();

        SetStatus("Уловът беше обновен.");
        return RedirectToAction(nameof(Catches));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCatch(int id)
    {
        var catchRecord = await _context.CatchRecords.FindAsync(id);
        if (catchRecord is null)
        {
            SetStatus("Записът за улов не беше намерен.", "warning");
            return RedirectToAction(nameof(Catches));
        }

        _context.CatchRecords.Remove(catchRecord);
        await _context.SaveChangesAsync();

        SetStatus("Уловът беше изтрит.");
        return RedirectToAction(nameof(Catches));
    }

    private async Task PopulateEngineOptionsAsync(int? selectedEngineId = null)
    {
        var engines = await _context.Engines
            .AsNoTracking()
            .OrderBy(engine => engine.Type)
            .ThenBy(engine => engine.PowerKw)
            .Select(engine => new
            {
                engine.Id,
                Label = $"{engine.Type} / {engine.FuelType} / {engine.PowerKw:F0} kW"
            })
            .ToListAsync();

        ViewBag.EngineOptions = new SelectList(engines, "Id", "Label", selectedEngineId);
    }

    private async Task PopulateTripOptionsAsync(int? selectedTripId = null)
    {
        var trips = await _context.FishingTrips
            .AsNoTracking()
            .OrderByDescending(trip => trip.StartTime)
            .Select(trip => new
            {
                trip.Id,
                Label = $"{trip.FishingVessel.Marking} / {trip.StartTime:dd.MM.yyyy HH:mm}"
            })
            .ToListAsync();

        ViewBag.TripOptions = new SelectList(trips, "Id", "Label", selectedTripId);
    }

    private void SetStatus(string message, string type = "success")
    {
        TempData["StatusMessage"] = message;
        TempData["StatusType"] = type;
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
