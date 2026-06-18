using Fisherman_Board.Controllers;
using Fisherman_Board.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;

namespace Fisherman_Board.Tests.Controllers;

public class HomeControllerTests
{
    [Fact]
    public void Privacy_ReturnsViewResult()
    {
        var controller = new HomeController(NullLogger<HomeController>.Instance, null!);

        var result = controller.Privacy();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Error_UsesHttpContextTraceIdentifier()
    {
        var controller = new HomeController(NullLogger<HomeController>.Instance, null!)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    TraceIdentifier = "trace-123"
                }
            }
        };

        var result = controller.Error();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ErrorViewModel>(viewResult.Model);

        Assert.Equal("trace-123", model.RequestId);
    }
}
