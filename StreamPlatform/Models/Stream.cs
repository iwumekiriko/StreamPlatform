using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamPlatform.Models;

public class Stream
{
    [Key]
    public string UserId { get; set; }
    [ForeignKey("UserId")]
    public ApplicationUser User { get; set; }

    public string StreamName { get; set; }
    public string StreamUrl { get; set; }
    public string StreamKey { get; set; }
    public bool IsActive { get; set; }

    public string? PreviewImageUrl { get; set; }
}
