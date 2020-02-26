using FirstProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstProject.ViewModels
{
    public class UserViewModel
    {
        public string Name { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "User type")]
        [Required]
        public string UserType { get; set; }

        public IPagedList<Users> Users { get; set; }
    }
}