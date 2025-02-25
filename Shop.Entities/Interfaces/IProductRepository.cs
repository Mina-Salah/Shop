using Shop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Entities.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
