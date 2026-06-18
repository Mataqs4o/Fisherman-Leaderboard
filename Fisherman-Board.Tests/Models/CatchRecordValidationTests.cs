using Fisherman_Board.Models;
using Fisherman_Board.Tests.TestUtilities;

namespace Fisherman_Board.Tests.Models;

public class CatchRecordValidationTests
{
    [Fact]
    public void Validate_WhenSpeciesIsMissing_ReturnsError()
    {
        var catchRecord = new CatchRecord
        {
            Species = string.Empty,
            QuantityKg = 8.5
        };

        var results = ValidationTestHelper.Validate(catchRecord);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(CatchRecord.Species)));
    }

    [Fact]
    public void Validate_WhenQuantityIsOutOfRange_ReturnsError()
    {
        var catchRecord = new CatchRecord
        {
            Species = "Сафрид",
            QuantityKg = 0
        };

        var results = ValidationTestHelper.Validate(catchRecord);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(CatchRecord.QuantityKg)));
    }

    [Fact]
    public void Validate_WhenModelIsValid_ReturnsNoErrors()
    {
        var catchRecord = new CatchRecord
        {
            FishingTripId = 5,
            Species = "Цаца",
            QuantityKg = 12.3
        };

        var results = ValidationTestHelper.Validate(catchRecord);

        Assert.Empty(results);
    }
}
