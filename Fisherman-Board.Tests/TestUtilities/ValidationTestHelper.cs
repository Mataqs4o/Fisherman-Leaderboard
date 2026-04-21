using System.ComponentModel.DataAnnotations;

namespace Fisherman_Board.Tests.TestUtilities;

internal static class ValidationTestHelper
{
    public static IReadOnlyList<ValidationResult> Validate(object instance)
    {
        var results = new List<ValidationResult>();
        var context = new ValidationContext(instance);

        Validator.TryValidateObject(instance, context, results, validateAllProperties: true);

        return results;
    }

    public static bool HasErrorFor(IEnumerable<ValidationResult> results, string memberName)
    {
        return results.Any(result => result.MemberNames.Contains(memberName));
    }
}
