using System.ComponentModel.DataAnnotations;

namespace MvcClient.ViewModels
{
    public class EditViewModel
    {

        [Required]
        public string Id { get; set; }

        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "The Password must be at least 4 characters long.")]
        public string Password { get; set; }
        [Compare("Password")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
