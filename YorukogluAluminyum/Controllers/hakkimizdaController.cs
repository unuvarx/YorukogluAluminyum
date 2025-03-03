using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class hakkimizdaController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}