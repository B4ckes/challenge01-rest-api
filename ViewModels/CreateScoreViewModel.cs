using System.ComponentModel.DataAnnotations;
using RestApi.Models;

namespace RestApi.ViewModels
{
    public class CreateScoreViewModel
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ScoreAmount { get; set; }
    }
}