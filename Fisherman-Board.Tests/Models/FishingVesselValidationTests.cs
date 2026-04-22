using Fisherman_Board.Models;
using Fisherman_Board.Tests.TestUtilities;

namespace Fisherman_Board.Tests.Models;

public class FishingVesselValidationTests
{
    [Fact]
    public void Validate_WhenRequiredIdentifiersAreMissing_ReturnsErrors()
    {
        var vessel = new FishingVessel
        {
            InternationalNumber = string.Empty,
            CallSign = string.Empty,
            Marking = string.Empty,
            Length = 10,
            Width = 3,
            Tonnage = 12,
            Draft = 1.2
        };

        var results = ValidationTestHelper.Validate(vessel);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(FishingVessel.InternationalNumber)));
        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(FishingVessel.CallSign)));
        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(FishingVessel.Marking)));
    }

    [Fact]
    public void Validate_WhenMeasurementsAreOutOfRange_ReturnsErrors()
    {
        var vessel = new FishingVessel
        {
            InternationalNumber = "BG-1001",
            CallSign = "LZAA",
            Marking = "VAR-001",
            Length = 0,
            Width = 0,
            Tonnage = 0,
            Draft = 0
        };

        var results = ValidationTestHelper.Validate(vessel);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(FishingVessel.Length)));
        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(FishingVessel.Width)));
        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(FishingVessel.Tonnage)));
        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(FishingVessel.Draft)));
    }

    [Fact]
    public void Validate_WhenModelIsValid_ReturnsNoErrors()
    {
        var vessel = new FishingVessel
        {
            InternationalNumber = "IMO-1234567",
            CallSign = "LZAB",
            Marking = "VAR-314",
            Length = 14.8,
            Width = 4.6,
            Tonnage = 28,
            Draft = 1.9,
            EngineId = 1
        };

        var results = ValidationTestHelper.Validate(vessel);

        Assert.Empty(results);
    }
}
