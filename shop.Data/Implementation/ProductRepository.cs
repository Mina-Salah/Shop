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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        // Custom implementation of Update for Product
        public void Update(Product product)
        {
            var productInDb = _context.products.FirstOrDefault(c => c.Id == product.Id);
            if (productInDb != null)
            {
                productInDb.Name = product.Name;
                productInDb.Description = product.Description;
                productInDb.Price = product.Price;
                productInDb.Img = product.Img;
                productInDb.CategoryId = product.CategoryId;  
            }
        }
    }
}
