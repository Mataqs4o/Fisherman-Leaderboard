using Fisherman_Board.Tests.TestUtilities;
using Fisherman_Board.ViewModels;

namespace Fisherman_Board.Tests.ViewModels;

public class RegisterViewModelTests
{
    [Fact]
    public void Validate_WhenPasswordIsTooShort_ReturnsError()
    {
        var model = new RegisterViewModel
        {
            Email = "operator@fisherman.bg",
            Password = "123",
            ConfirmPassword = "123"
        };

        var results = ValidationTestHelper.Validate(model);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(RegisterViewModel.Password)));
    }

    [Fact]
    public void Validate_WhenPasswordsDoNotMatch_ReturnsError()
    {
        var model = new RegisterViewModel
        {
            Email = "operator@fisherman.bg",
            Password = "secret123",
            ConfirmPassword = "different123"
        };

        var results = ValidationTestHelper.Validate(model);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(RegisterViewModel.ConfirmPassword)));
    }

    [Fact]
    public void Validate_WhenModelIsValid_ReturnsNoErrors()
    {
        var model = new RegisterViewModel
        {
            Email = "operator@fisherman.bg",
            Password = "secret123",
            ConfirmPassword = "secret123",
            ReturnUrl = "/Registry"
        };

        var results = ValidationTestHelper.Validate(model);

        Assert.Empty(results);
    }
}
