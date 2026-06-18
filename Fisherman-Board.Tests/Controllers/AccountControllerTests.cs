using System.Security.Claims;
using Fisherman_Board.Controllers;
using Fisherman_Board.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fisherman_Board.Tests.Controllers;

public class AccountControllerTests
{
    [Fact]
    public void LoginGet_WhenUserIsAnonymous_ReturnsViewWithReturnUrl()
    {
        var controller = CreateController(isAuthenticated: false);

        var result = controller.Login("/Registry");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LoginViewModel>(viewResult.Model);

        Assert.Equal("/Registry", model.ReturnUrl);
    }

    [Fact]
    public void LoginGet_WhenUserIsAuthenticated_RedirectsToHome()
    {
        var controller = CreateController(isAuthenticated: true);

        var result = controller.Login("/Registry");

        var redirect = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Home", redirect.ControllerName);
    }

    [Fact]
    public void RegisterGet_WhenUserIsAnonymous_ReturnsViewWithReturnUrl()
    {
        var controller = CreateController(isAuthenticated: false);

        var result = controller.Register("/Reports");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<RegisterViewModel>(viewResult.Model);

        Assert.Equal("/Reports", model.ReturnUrl);
    }

    [Fact]
    public async Task LoginPost_WhenModelStateIsInvalid_ReturnsSameModel()
    {
        var controller = CreateController(isAuthenticated: false);
        var model = new LoginViewModel
        {
            Email = "bad-email",
            Password = string.Empty
        };

        controller.ModelState.AddModelError(nameof(LoginViewModel.Email), "Invalid");

        var result = await controller.Login(model);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(model, viewResult.Model);
    }

    [Fact]
    public async Task RegisterPost_WhenModelStateIsInvalid_ReturnsSameModel()
    {
        var controller = CreateController(isAuthenticated: false);
        var model = new RegisterViewModel
        {
            Email = "operator@fisherman.bg",
            Password = "123",
            ConfirmPassword = "123"
        };

        controller.ModelState.AddModelError(nameof(RegisterViewModel.Password), "Too short");

        var result = await controller.Register(model);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(model, viewResult.Model);
    }

    private static AccountController CreateController(bool isAuthenticated)
    {
        var identity = isAuthenticated
            ? new ClaimsIdentity([new Claim(ClaimTypes.Name, "operator@fisherman.bg")], authenticationType: "Test")
            : new ClaimsIdentity();

        return new AccountController(null!, null!)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(identity)
                }
            }
        };
    }
}
