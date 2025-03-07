using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class iletisimController : Controller
{
    // GET
    public IActionResult index()
    {
        ViewBag.ActivePage = 5;
        return View();
    }
}