using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Entities.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // Add repository properties here
        ICategoryRepository Categories { get; }

        // Save changes to the database
        int Complet();
    }
}
