using Microsoft.EntityFrameworkCore;
using Shop.Data.Context;
using Shop.Entities.Interfaces;
using Shop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shop.Data.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Custom implementation of Update for Category
        public void Update(Category category)
        {
            var categoryInDb = _context.Category.FirstOrDefault(c => c.Id == category.Id);
            if (categoryInDb != null)
            {
                categoryInDb.Name = category.Name;
                categoryInDb.Description = category.Description;
                categoryInDb.DateTime = DateTime.Now;  
            }
        }
    }
}
