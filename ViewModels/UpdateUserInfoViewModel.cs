using System.ComponentModel.DataAnnotations;

namespace RestApi.ViewModels
{
    public class UpdateUserInfoViewModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
    }
}