using Fisherman_Board.Controllers;
using Fisherman_Board.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fisherman_Board.Tests.Controllers;

public class RegistryControllerTests
{
    [Fact]
    public void CreateFisherman_ReturnsViewWithDefaultAdultBirthDate()
    {
        var controller = new RegistryController(null!);

        var result = controller.CreateFisherman();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<Person>(viewResult.Model);

        Assert.Equal(DateTime.Today.AddYears(-18), model.BirthDate);
    }

    [Fact]
    public async Task CreateFisherman_WhenModelStateIsInvalid_ReturnsSameModel()
    {
        var controller = new RegistryController(null!);
        var person = new Person();

        controller.ModelState.AddModelError(nameof(Person.FullName), "Required");

        var result = await controller.CreateFisherman(person);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(person, viewResult.Model);
    }

    [Fact]
    public async Task EditFisherman_WhenRouteIdDoesNotMatchModelId_ReturnsNotFound()
    {
        var controller = new RegistryController(null!);
        var person = new Person { Id = 7, FullName = "Ivan Petrov" };

        var result = await controller.EditFisherman(3, person);

        Assert.IsType<NotFoundResult>(result);
    }
}
