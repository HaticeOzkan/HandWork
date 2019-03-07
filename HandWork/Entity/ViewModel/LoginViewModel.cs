using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModel
{
    class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name ="Email Adres")]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        [Required]
        [Display(Name ="Sifre")]
        public string Password { get; set; }
        [Display(Name ="Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
