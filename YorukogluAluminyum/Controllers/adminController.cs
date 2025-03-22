using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using YorukogluAluminyum.Models;

namespace YorukogluAluminyum.Controllers;

public class adminController : Controller
{
    private readonly Contexts _contexts;
    private readonly IConfiguration _configuration;
    private const string AuthCookieName = "_u";
    private readonly IWebHostEnvironment _webHostEnvironment;

    public adminController(Contexts contexts, IConfiguration configuration , IWebHostEnvironment webHostEnvironment)
    {
        
        _contexts = contexts;
        
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }
    
    
    private Users ValidateToken(string token)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        var tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secretKey,
            ValidateIssuer = false,
            ValidateAudience = false,
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken validatedToken);

        var jwtToken = (JwtSecurityToken)validatedToken;
        var username = jwtToken.Subject;

        var user = _contexts.Users.AsNoTracking().FirstOrDefault(u => u.UserName == username);
        return user;
    }
    private bool UserIsAuthenticated()
    {
        try
        {
            string token = Request.Cookies[AuthCookieName];
            Console.WriteLine("ÇEREEEZZZ; : " + token);

            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            var user = ValidateToken(token);

            if (user == null)
            {
                return false;
            }


            return true;
        }
        catch (Exception ex)
        {

            return false;
        }
    }
    private void AddJwtCookie(string token)
    {
        Response.Cookies.Append(AuthCookieName, token, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.Now.AddHours(1)
        });
    }

    
    
    private string CreateToken(Users user)
    {
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        Console.WriteLine("TOKENEEEENNENE : " + secretKey);
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    
    
    public IActionResult login()
    {
        return View();
    }
    
    // ymurat : yorukoglualuminyum68!
    [HttpPost]
    public ActionResult LoginReq(string username, string password)
    {
        try
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.ErrorMessage = "Kullanıcı adı veya şifre alanı boş";
                return View("login");
            }

            var user = _contexts.Users.AsNoTracking().FirstOrDefault(u => u.UserName == username);
        
            if (user == null)
            {
                ViewBag.ErrorMessage = "Hatalı şifre veya kullanıcı adı!";
                return View("login");
            }

            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                ViewBag.ErrorMessage = "Kullanıcı bilgileri hatalı!";
                return View("login");
            }

            if (BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                AddJwtCookie(CreateToken(user));
                return RedirectToAction("home", "admin");
            }
            else
            {
                ViewBag.ErrorMessage = "Hatalı şifre veya kullanıcı adı!";
                return View("login");
            }
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Hatalı işlem!";
            Console.WriteLine(ex.Message);
            Console.WriteLine(username);
            Console.WriteLine(password);
            return View("login");
        }
    }

    public IActionResult home()
    {
        if (!UserIsAuthenticated())
        {
            return RedirectToAction("login", "admin");
        }
        var products = _contexts.Products.ToList();
        ViewBag.MailNumber = _contexts.Mails.Count();
        ViewBag.Products = products;
        return View();
    }


    public IActionResult addcategory()
    {
        if (!UserIsAuthenticated())
        {
            return RedirectToAction("login", "admin");
        }
        ViewBag.MailNumber = _contexts.Mails.Count();
        return View();
    }
    public IActionResult addproduct()
    {
        if (!UserIsAuthenticated())
        {
            return RedirectToAction("login", "admin");
        }
        var categories = _contexts.Categories.ToList();
        ViewBag.MailNumber = _contexts.Mails.Count();
        ViewBag.Categories = categories;
        return View();
    }
    public IActionResult listofcategory()
    {
        if (!UserIsAuthenticated())
        {
            return RedirectToAction("login", "admin");
        }
        var categories = _contexts.Categories.ToList();
        ViewBag.MailNumber = _contexts.Mails.Count();
        ViewBag.Categories = categories;
        return View();
    }
    
    [HttpPost]
    public IActionResult AddProduct(IFormFile image1, IFormFile image2, IFormFile image3, IFormFile image4, int categoryId, string description)
    {
        try
        {
            if (!UserIsAuthenticated())
            {
                return RedirectToAction("login", "admin");
            }
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads/products");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            string GenerateFileName(string extension) => $"{new Random().Next(10000000, 99999999)}{extension}";

            string SaveImage(IFormFile file)
            {
                if (file != null && file.Length > 0)
                {
                    string extension = Path.GetExtension(file.FileName);
                    string fileName = GenerateFileName(extension);
                    string filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    return $"/uploads/products/{fileName}";
                }
                return null;
            }

            var category = _contexts.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == categoryId);
            var product = new Product
            {
                Category = category.Category,
                CategoryId = categoryId,
               Description = description,
                img1 = SaveImage(image1),
                img2 = SaveImage(image2),
                img3 = SaveImage(image3),
                img4 = SaveImage(image4)
            };

            _contexts.Products.Add(product);
            _contexts.SaveChanges();

            ViewBag.SuccessMessage = "Ürün başarıyla eklendi!";
            var categories = _contexts.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Ürün ekleme sırasında hata oluştu! " + ex.Message;
            var categories = _contexts.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
    }

    [HttpPost]
    public IActionResult AddCategory(string category)
    {
        try
        {
            if (!UserIsAuthenticated())
            {
                return RedirectToAction("login", "admin");
            }

            var categoryObj = new Categories
            {
                Category = category
            };
            _contexts.Categories.Add(categoryObj);
            _contexts.SaveChanges();
            ViewBag.SuccessMessage = "Kategori başarıyla eklendi!";
            return View();
        }
        catch (Exception e)
        {
            ViewBag.ErrorMessage = "Ürün Eklenirken hata oluştu!" + e.Message;
            return View();
        }
    }
    [HttpPost]
    public IActionResult DeleteProductReq(int productId)
    {
        try
        {
            if (!UserIsAuthenticated())
            {
                return RedirectToAction("login", "admin");
            }
            
            var product = _contexts.Products.AsNoTracking().FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                ViewBag.ErrorMessage = "Ürün bulunamadı!";
                return RedirectToAction("home", "admin");
            }

            
            string[] imagePaths = { product.img1, product.img2, product.img3, product.img4 };

            foreach (var imagePath in imagePaths)
            {
                if (!string.IsNullOrEmpty(imagePath)) // Eğer resim yolu varsa
                {
                    string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));

                    if (System.IO.File.Exists(fullPath)) // Dosya var mı kontrol et
                    {
                        System.IO.File.Delete(fullPath); // Dosyayı sil
                    }
                }
            }
            _contexts.Products.Remove(product);
            _contexts.SaveChanges();

            ViewBag.SuccessMessage = "Ürün başarıyla silindi!";
            return RedirectToAction("home", "admin");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = "Ürün silinirken hata oluştu! " + ex.Message;
            return RedirectToAction("home", "admin");
        }
    }

    [HttpPost]
    public IActionResult DeleteCategoryReq(int categoryId)
    {
        try
        {
            if (!UserIsAuthenticated())
            {
                return RedirectToAction("login", "admin");
            }

            // Kategorideki ürünleri bul
            var products = _contexts.Products.Where(p => p.CategoryId == categoryId).ToList();

            foreach (var product in products)
            {
                // Resimleri sil
                DeleteImage(product.img1);
                DeleteImage(product.img2);
                DeleteImage(product.img3);
                DeleteImage(product.img4);

                // Ürünü veritabanından kaldır
                _contexts.Products.Remove(product);
            }

            // Kategoriyi bul
            var category = _contexts.Categories.AsNoTracking().FirstOrDefault(c => c.CategoryId == categoryId);
            if (category != null)
            {
                _contexts.Categories.Remove(category);
            }

            // Değişiklikleri kaydet
            _contexts.SaveChanges();

            ViewBag.SuccessMessage = "Kategori ve ilgili ürünler başarıyla silindi!";
        }
        catch (Exception e)
        {
            ViewBag.ErrorMessage = "Silme işlemi sırasında hata oluştu: " + e.Message;
        }

        return RedirectToAction("listofcategory", "admin");
    }

// Resim silme fonksiyonu
    private void DeleteImage(string imagePath)
    {
        if (!string.IsNullOrEmpty(imagePath))
        {
            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, imagePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
    }

    public IActionResult contacts()
    {
        if (!UserIsAuthenticated())
        {
            return RedirectToAction("login", "admin");
        }
        ViewBag.MailNumber = _contexts.Mails.Count();
        ViewBag.Mails = _contexts.Mails.ToList();
        return View();
    }

    public async Task<IActionResult> DeleteMailReq(int mailId)
    {
        try
        {
            if (!UserIsAuthenticated())
            {
                return RedirectToAction("login", "admin");
            }

            var mail = _contexts.Mails.AsNoTracking().FirstOrDefault(m => m.MailId == mailId);
            if (mail == null)
            {
                return RedirectToAction("contacts", "admin"); // Mail bulunamazsa, doğru sayfaya yönlendir
            }

            _contexts.Mails.Remove(mail);
            await _contexts.SaveChangesAsync();

            return RedirectToAction("contacts", "admin"); // Silme işlemi başarılıysa, doğru sayfaya yönlendir
        }
        catch (Exception e)
        {
            // Hata durumunda da doğru sayfaya yönlendir
            return RedirectToAction("contacts", "admin");
        }
    }

}