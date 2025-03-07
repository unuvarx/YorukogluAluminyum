using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class hizmetlerimizController : Controller
{
    // GET
    public IActionResult index()
    {
        ViewBag.ActivePage = 2;
        return View();
    }
}