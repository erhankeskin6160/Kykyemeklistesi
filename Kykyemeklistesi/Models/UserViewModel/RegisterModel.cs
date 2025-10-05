using System.ComponentModel.DataAnnotations;

namespace Kykyemeklistesi.Models.UserViewModel
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Lütfen E posta adresini doğru bir şekilde giriniz")]
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }



        [Required(ErrorMessage = "Lütfen şifrenizi tekrar giriniz")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="Lütfen Okuduğunuz Şehri Giriniz")]
        public string City { get; set; }


        [Required]
        public string RecaptchaToken { get; set; }

       
         



         
       
    }
}
