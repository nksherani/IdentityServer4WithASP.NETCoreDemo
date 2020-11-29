using System.ComponentModel.DataAnnotations;

namespace MvcClient.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string Id { get; set; }
       
        [Required]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "The Password must be at least 4 characters long.")]
        public string Password { get; set; }
        [Compare("Password")]
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
