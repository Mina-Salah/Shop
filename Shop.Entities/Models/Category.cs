using System.ComponentModel.DataAnnotations;

namespace Shop.Entities.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;   
    }
}
