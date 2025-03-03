using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class projelerimizController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}