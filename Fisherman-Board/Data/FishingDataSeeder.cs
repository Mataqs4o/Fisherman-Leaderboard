using Fisherman_Board.Models;

namespace Fisherman_Board.Data
{
    public static class FishingDataSeeder
    {
        public static void Seed(ApplicationDbContext dbContext)
        {
            if (dbContext.Fisherman.Any())
            {
                return;
            }

            var fishermen = new List<Fisherman>
            {
                new()
                {
                    FullName = "Ivan Petrov",
                    LicenseNumber = "FISH-001",
                    HomePort = "Burgas",
                    PhoneNumber = "+359 888 100 001"
                },
                new()
                {
                    FullName = "Georgi Dimitrov",
                    LicenseNumber = "FISH-002",
                    HomePort = "Varna",
                    PhoneNumber = "+359 888 100 002"
                },
                new()
                {
                    FullName = "Nikolay Stoyanov",
                    LicenseNumber = "FISH-003",
                    HomePort = "Sozopol",
                    PhoneNumber = "+359 888 100 003"
                }
            };

            dbContext.Fisherman.AddRange(fishermen);
            dbContext.SaveChanges();

            var boats = new List<Boat>
            {
                new()
                {
                    Name = "Black Sea Star",
                    RegistrationNumber = "BS-1001",
                    LengthMeters = 12.40m,
                    EngineModel = "Yanmar 6LY",
                    FuelConsumptionPerHour = 18.50m,
                    FishermanId = fishermen[0].Id
                },
                new()
                {
                    Name = "Blue Wave",
                    RegistrationNumber = "VN-2044",
                    LengthMeters = 9.75m,
                    EngineModel = "Volvo Penta D4",
                    FuelConsumptionPerHour = 14.20m,
                    FishermanId = fishermen[1].Id
                },
                new()
                {
                    Name = "Sea Falcon",
                    RegistrationNumber = "SZ-3308",
                    LengthMeters = 11.10m,
                    EngineModel = "Cummins QSB",
                    FuelConsumptionPerHour = 16.80m,
                    FishermanId = fishermen[2].Id
                }
            };

            dbContext.Boats.AddRange(boats);
            dbContext.SaveChanges();

            var hunts = new List<Hunt>
            {
                new()
                {
                    FishermanId = fishermen[0].Id,
                    BoatId = boats[0].Id,
                    StartedAt = DateTime.Today.AddDays(-6).AddHours(4),
                    EndedAt = DateTime.Today.AddDays(-6).AddHours(12),
                    FishType = "Sprat",
                    QuantityKg = 320.50m,
                    FishingArea = "Burgas Bay"
                },
                new()
                {
                    FishermanId = fishermen[1].Id,
                    BoatId = boats[1].Id,
                    StartedAt = DateTime.Today.AddDays(-4).AddHours(5),
                    EndedAt = DateTime.Today.AddDays(-4).AddHours(11),
                    FishType = "Mackerel",
                    QuantityKg = 214.00m,
                    FishingArea = "Varna Coast"
                },
                new()
                {
                    FishermanId = fishermen[2].Id,
                    BoatId = boats[2].Id,
                    StartedAt = DateTime.Today.AddDays(-2).AddHours(3),
                    EndedAt = DateTime.Today.AddDays(-2).AddHours(10),
                    FishType = "Anchovy",
                    QuantityKg = 186.75m,
                    FishingArea = "Sozopol Waters"
                }
            };

            dbContext.Hunt.AddRange(hunts);
            dbContext.SaveChanges();
        }
    }
}
