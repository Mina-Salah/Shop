using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Entities.Models
{
    public class Product
    {
       
        public int id { get; set; }
        [Required]
        public string name { get; set; }
        public string description { get; set; }
        public string Img { get; set; } 
        public decimal price { get; set; }
       
        
        // relation 
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
