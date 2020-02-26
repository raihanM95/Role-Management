using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FirstProject.Models
{
    public class Users
    {
        public int ID { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required(AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(200)]
        public string Password { get; set; }

        [Required]
        public string UserType { get; set; }
    }
}