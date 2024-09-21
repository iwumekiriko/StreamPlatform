using System.ComponentModel.DataAnnotations;

namespace StreamPlatform.ViewModels;

public class StreamCreateViewModel
{
    [Required]
    public string StreamName { get; set; }
}
