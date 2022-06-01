using System.ComponentModel.DataAnnotations;

namespace RestApi.ViewModels
{
    public class UpdateUserInfoViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}