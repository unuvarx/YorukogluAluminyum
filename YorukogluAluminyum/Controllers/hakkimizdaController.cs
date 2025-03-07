using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class hakkimizdaController : Controller
{
    // GET
    public IActionResult index()
    {
        ViewBag.ActivePage = 1;
        return View();
    }
}