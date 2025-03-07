using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace aspnet_logger_backend.Models;

public class Logentry
{
    [Required]
    public int id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [AllowNull]
    public string Message { get; set; }
}