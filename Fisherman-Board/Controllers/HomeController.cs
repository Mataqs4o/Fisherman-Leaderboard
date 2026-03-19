using System.Diagnostics; // Използва се за работа с процеси и диагностика
using Fisherman_Board.Models; // Импорт на моделите от проекта
using Microsoft.AspNetCore.Mvc; // Основни класове за MVC

namespace Fisherman_Board.Controllers
{
    public class HomeController : Controller // Главен контролер за приложението
    {
        private readonly ILogger<HomeController> _logger; // Логър за запис на събития и грешки

        public HomeController(ILogger<HomeController> logger) // Конструктор с dependency injection
        {
            _logger = logger; // Присвояване на подадения логър
        }

        public IActionResult Index() // Метод за началната страница
        {
            return View(); // Връща изгледа Index
        }

        public IActionResult Privacy() // Метод за страницата Privacy
        {
            return View(); // Връща изгледа Privacy
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] // Забранява кеширането
        public IActionResult Error() // Метод за обработка на грешки
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // Взима текущото ID на заявката
            });
        }
    }
}
