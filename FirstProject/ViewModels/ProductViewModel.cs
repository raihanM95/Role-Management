using FirstProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstProject.ViewModels
{
    public class ProductViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Product name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Client name")]
        public int ClientId { get; set; }

        [Display(Name = "Support name")]
        public int SupportId { get; set; }

        public int[] ClientIds { get; set; }

        public int[] SupportIds { get; set; }

        public IEnumerable<SelectListItem> ClientSelectListItems { get; set; }
        public IEnumerable<SelectListItem> SupportSelectListItems { get; set; }

        public IPagedList<Product> Products { get; set; }
    }
}