using Microsoft.EntityFrameworkCore;

namespace YorukogluAluminyum.Models;

public class Contexts : DbContext

{
    public Contexts(DbContextOptions<Contexts> options) : base(options)
    {
        
    }
    
    public DbSet<Users> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Categories> Categories { get; set; }
    public DbSet<Mails> Mails { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.Entity<Users>()
            .HasKey(x => x.UserId);
        
        modelBuilder.Entity<Product>()
            .HasKey(x => x.ProductId);
        modelBuilder.Entity<Categories>()
            .HasKey(x => x.CategoryId);
        modelBuilder.Entity<Product>()
            .HasOne<Categories>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Mails>()
            .HasKey(x => x.MailId);
            
    }    
}