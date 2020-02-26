using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstProject.Models
{
    public class ClientInfo
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        public string Contact { get; set; }

        public int UsersId { get; set; }
        public Users Users { get; set; }
    }
}