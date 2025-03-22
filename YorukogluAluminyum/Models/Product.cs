using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YorukogluAluminyum.Models;

public class Product
{
    
    [Key]
    public int ProductId { get; set; }
    [ForeignKey("Categories")]
    public int CategoryId { get; set; }
    public string Description { get; set; } 
    public string Category { get; set; }
    public string img1 { get; set; }
    public string img2 { get; set; }
    public string img3{ get; set; }
    public string img4 { get; set; }
}