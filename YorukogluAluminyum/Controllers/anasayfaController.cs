using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class anasayfaController : Controller
{
    // GET
    public IActionResult index()
    {
        ViewBag.ActivePage = 0;
        return View();
    }
}