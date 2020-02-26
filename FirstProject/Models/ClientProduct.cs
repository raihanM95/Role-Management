using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FirstProject.Models
{
    public class ClientProduct
    {
        public int ID { get; set; }

        public int UsersId { get; set; }
        public Users Users { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}