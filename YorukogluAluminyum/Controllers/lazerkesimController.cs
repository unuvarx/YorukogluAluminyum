using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class lazerkesimController : Controller
{
    // GET
    public IActionResult index()
    {
        ViewBag.ActivePage = 4;
        return View();
    }
}