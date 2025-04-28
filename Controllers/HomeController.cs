using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ToDoUygulaması.Models;

namespace ToDoUygulaması.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // Todo controller'a yönlendir
            return RedirectToAction("Index", "Todo");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
