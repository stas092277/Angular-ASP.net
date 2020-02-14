using InGo.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InGo.Models.Identity
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords must match")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
