using Fisherman_Board.Models;
using Fisherman_Board.Tests.TestUtilities;

namespace Fisherman_Board.Tests.Models;

public class PersonValidationTests
{
    [Fact]
    public void Validate_WhenFullNameIsMissing_ReturnsError()
    {
        var person = new Person
        {
            FullName = string.Empty
        };

        var results = ValidationTestHelper.Validate(person);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(Person.FullName)));
    }

    [Fact]
    public void Validate_WhenRequiredFieldsArePresent_ReturnsNoErrors()
    {
        var person = new Person
        {
            FullName = "Ivan Petrov",
            BirthDate = new DateTime(1990, 5, 10),
            IsDisabled = false,
            IsPensioner = false
        };

        var results = ValidationTestHelper.Validate(person);

        Assert.Empty(results);
    }
}
