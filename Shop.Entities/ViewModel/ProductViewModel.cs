using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Entities.ViewModel
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        [Display(Name = "Category")]
        [ValidateNever]
        public IEnumerable<SelectListItem> categorySelect { get; set; }
    }

}
