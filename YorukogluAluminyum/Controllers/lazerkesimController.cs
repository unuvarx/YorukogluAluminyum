using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class lazerkesimController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}