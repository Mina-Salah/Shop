using Shop.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Entities.ViewModel
{
    public class ShopCardViewModel
    {
        public Product product {  get; set; }
        [Range(1,100,ErrorMessage ="you must enter number bettween 1 to 100 ")]
        public int count { get; set; }  
    }
}
