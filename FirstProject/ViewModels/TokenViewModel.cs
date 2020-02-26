using FirstProject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FirstProject.ViewModels
{
    public class TokenViewModel
    {
        public int ID { get; set; }

        [Display(Name = "Complain description")]
        [Required]
        public string ComplainDescription { get; set; }

        [Display(Name = "Solved description")]
        public string SolvedDescription { get; set; }

        [Display(Name = "Product name")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public Token Token { get; set; }

        public IPagedList<Token> Tokens { get; set; }
    }
}