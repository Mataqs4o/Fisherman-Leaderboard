using Fisherman_Board.Models;
using Fisherman_Board.Tests.TestUtilities;

namespace Fisherman_Board.Tests.Models;

public class EngineValidationTests
{
    [Fact]
    public void Validate_WhenRequiredFieldsAreMissing_ReturnsErrors()
    {
        var engine = new Engine
        {
            Type = string.Empty,
            FuelType = string.Empty,
            PowerKw = 120,
            AvgFuelPerHour = 18
        };

        var results = ValidationTestHelper.Validate(engine);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(Engine.Type)));
        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(Engine.FuelType)));
    }

    [Fact]
    public void Validate_WhenNumericValuesAreOutOfRange_ReturnsErrors()
    {
        var engine = new Engine
        {
            Type = "Diesel",
            FuelType = "Diesel",
            PowerKw = 0,
            AvgFuelPerHour = 0
        };

        var results = ValidationTestHelper.Validate(engine);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(Engine.PowerKw)));
        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(Engine.AvgFuelPerHour)));
    }

    [Fact]
    public void Validate_WhenModelIsValid_ReturnsNoErrors()
    {
        var engine = new Engine
        {
            Type = "Diesel 6L",
            FuelType = "Diesel",
            PowerKw = 240,
            AvgFuelPerHour = 24
        };

        var results = ValidationTestHelper.Validate(engine);

        Assert.Empty(results);
    }
}
