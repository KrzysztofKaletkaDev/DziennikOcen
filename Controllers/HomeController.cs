using Microsoft.AspNetCore.Mvc;

namespace DziennikOcen.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "User");
                }

                if (User.IsInRole("Lecturer"))
                {
                    return RedirectToAction("Index", "Grade");
                }

                return View("UnassignedRole");
            }

            return RedirectToAction("Login", "Account");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}