using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class iletisimController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}