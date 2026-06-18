using Fisherman_Board.Tests.TestUtilities;
using Fisherman_Board.ViewModels;

namespace Fisherman_Board.Tests.ViewModels;

public class LoginViewModelTests
{
    [Fact]
    public void Validate_WhenEmailIsMissing_ReturnsError()
    {
        var model = new LoginViewModel
        {
            Email = string.Empty,
            Password = "secret123"
        };

        var results = ValidationTestHelper.Validate(model);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(LoginViewModel.Email)));
    }

    [Fact]
    public void Validate_WhenEmailIsInvalid_ReturnsError()
    {
        var model = new LoginViewModel
        {
            Email = "not-an-email",
            Password = "secret123"
        };

        var results = ValidationTestHelper.Validate(model);

        Assert.True(ValidationTestHelper.HasErrorFor(results, nameof(LoginViewModel.Email)));
    }

    [Fact]
    public void Validate_WhenModelIsValid_ReturnsNoErrors()
    {
        var model = new LoginViewModel
        {
            Email = "operator@fisherman.bg",
            Password = "secret123",
            RememberMe = true
        };

        var results = ValidationTestHelper.Validate(model);

        Assert.Empty(results);
    }
}
