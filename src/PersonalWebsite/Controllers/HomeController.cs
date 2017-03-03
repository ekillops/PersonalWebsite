using Microsoft.AspNetCore.Mvc;
using PersonalWebsite.Models;

namespace PersonalWebsite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Projects()
        {
            var repos = Project.GetTopThreeStarred();
            return View(repos);
        }
    }
}
