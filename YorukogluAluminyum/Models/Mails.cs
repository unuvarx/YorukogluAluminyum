using System.ComponentModel.DataAnnotations;

namespace YorukogluAluminyum.Models;

public class Mails
{
    
    [Key]
    public int MailId { get; set; }
    public string NameSurname { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }
    public string Message { get; set; }
}