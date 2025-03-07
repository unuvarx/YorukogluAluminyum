using Microsoft.AspNetCore.Mvc;

namespace YorukogluAluminyum.Controllers;

public class projelerimizController : Controller
{
    // GET
    public IActionResult index()
    {
        ViewBag.ActicePage = 3;
        return View();
    }
}