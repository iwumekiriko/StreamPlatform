using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace StreamPlatform.ViewModels
{
    public class EditProfileViewModel
    {
        [Required]
        [Display(Name = "New username")]
        public string UserName { get; set; } 
    }
}