using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class anasayfa : Controller
{
    // GET
    public IActionResult Index()
    {
        ViewBag.ActivePage = 0;
        return View();
    }
}