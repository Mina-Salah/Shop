using Shop.Data.Context;
using Shop.Entities.Interfaces;


namespace shop.Data.Implementation
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public ICategoryRepository Categories { get; private set; }
        public IProductRepository Products { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Categories = new CategoryRepository(context);
            Products = new ProductRepository(context);

        }

        public void Dispose()
        {
             _context.Dispose();
        }

        public int Complet()
        {
            return _context.SaveChanges();
        }
    }
}
