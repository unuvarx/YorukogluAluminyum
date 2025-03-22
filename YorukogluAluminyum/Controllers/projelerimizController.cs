using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YorukogluAluminyum.Models;

namespace YorukogluAluminyum.Controllers;

public class projelerimizController : Controller
{
    private readonly Contexts _contexts;

    public projelerimizController(Contexts contexts)
    {
        _contexts = contexts;
    }
    public IActionResult index()
    {
        ViewBag.ActivePage = 3;
        ViewBag.Products = _contexts.Products.ToList();
        return View();
    }
    public IActionResult detay(int proje)
    {
        var project = _contexts.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == proje);
        if (project == null)
        {
            return RedirectToAction("index", "projelerimiz");
        }
        ViewBag.Product = project;
        ViewBag.ActivePage = 3;
        return View();
    }

}