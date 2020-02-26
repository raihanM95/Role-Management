using FirstProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstProject.ViewModels
{
    public class RegistrationViewModel
    {
        public int ID { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Username")]
        [StringLength(15, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        [StringLength(200)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [MinLength(6, ErrorMessage = "Password minimum 6 character required")]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        [StringLength(200)]
        public string ConfirmPassword { get; set; }

        [Display(Name = "User type")]
        [Required]
        public string UserType { get; set; }
        //public ClientInfo UrserType { get; set; }
    }
}