using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class adminController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}