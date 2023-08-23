using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageFileName { get; set; }
        public decimal Price { get; set; }
       
        public string Image { get; internal set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
