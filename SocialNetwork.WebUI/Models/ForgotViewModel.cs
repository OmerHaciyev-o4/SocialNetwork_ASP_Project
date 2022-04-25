using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.WebUI.Models
{
    public class ForgotViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}