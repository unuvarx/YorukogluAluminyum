using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class projelerimizController : Controller
{
    // GET
    public IActionResult index()
    {
        ViewBag.ActivePage = 3;
        return View();
    }
    public IActionResult detay()
    {
        ViewBag.ActivePage = 3;
        return View();
    }

}