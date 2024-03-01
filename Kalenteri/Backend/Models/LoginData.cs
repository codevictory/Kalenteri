using System.ComponentModel.DataAnnotations;

namespace Kalenteri.Backend.Models;

public class LoginData
{
    [Required]
    [StringLength(16, ErrorMessage = "Identifier too long (16 character limit).")]
    public string? Id { get; set; }

}