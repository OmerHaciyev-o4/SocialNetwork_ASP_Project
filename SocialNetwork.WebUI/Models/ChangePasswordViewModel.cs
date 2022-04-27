using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.WebUI.Models
{
    public class ChangePasswordViewModel
    {
        public string Usrname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string CurrentPassowrd { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ChangePassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmChangePassword { get; set; }
    }
}