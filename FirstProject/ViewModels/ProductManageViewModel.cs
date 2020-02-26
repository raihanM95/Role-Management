using FirstProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstProject.ViewModels
{
    public class ProductManageViewModel
    {
        [Display(Name = "Client name")]
        public int ClientId { get; set; }

        [Display(Name = "Support name")]
        public int SupportId { get; set; }

        [Display(Name = "Product name")]
        public int ProductId { get; set; }

        public ICollection<ClientProduct> ClientProducts { get; set; }
        public ICollection<SupportProduct> SupportProducts { get; set; }
    }
}