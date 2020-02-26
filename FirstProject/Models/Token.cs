using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirstProject.Models
{
    public class Token
    {
        public int ID { get; set; }

        [Required]
        public string ComplainDescription { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? ComplainDate { get; set; }

        public string SolvedDescription { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? SolvedDate { get; set; }

        public bool Type { get; set; }

        public bool Status { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}