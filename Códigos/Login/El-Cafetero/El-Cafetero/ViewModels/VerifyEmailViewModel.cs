using System.ComponentModel.DataAnnotations;

namespace El_Cafetero.ViewModels
{
    public class VerifyEmailViewModel
    {

        [Required(ErrorMessage = "Email is required. ")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
