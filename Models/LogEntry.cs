using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace aspnet_logger_backend.Models;

public class Logentry
{
    [Required]
    public string? id { get; set; }

    //[Required]
    //public DateTime CreatedAt { get; set; }

    //[AllowNull]
    //public string Message { get; set; }

    [AllowNull]
    public string data { get; set; }
}