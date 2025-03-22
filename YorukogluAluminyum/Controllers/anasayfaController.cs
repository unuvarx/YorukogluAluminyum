using Microsoft.AspNetCore.Mvc;
using YorukogluAluminyum.Models;

namespace YorukogluAluminyum.Controllers;

public class anasayfaController : Controller
{
    private readonly Contexts _context;

    public anasayfaController(Contexts context)
    {
        _context = context;
        
    }
    public IActionResult index()
    {
        var products = _context.Products
            .OrderByDescending(p => p.ProductId) // En son eklenenler en üste gelsin
            .Take(3)  // Son 3 ürünü seç
            .ToList(); // Listeye çevir

// ID sırasına göre düzeltmek istiyorsan burada sıralayabilirsin
        products = products.OrderBy(p => p.ProductId).ToList();
Console.WriteLine("PRODUCTSSSS--------- : " + products.Count);
        ViewBag.Products  = products;
        ViewBag.ActivePage = 0;
        return View();
    }
}