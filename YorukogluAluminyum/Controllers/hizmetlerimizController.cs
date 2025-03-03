using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class hizmetlerimizController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}