using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
       
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [ValidateNever]
        public string? Img { get; set; } 
        public decimal Price { get; set; }
       
        
        // relation 
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
