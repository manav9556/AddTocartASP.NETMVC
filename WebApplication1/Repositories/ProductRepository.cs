using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class ProductRepository
    {
        private readonly ProductDbContext _dbContext;

        public ProductRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _dbContext.Products.ToList();
        }

        public void AddProduct(Product product)
        {
            _dbContext.Products.Add(product);
            _dbContext.SaveChanges();
        }
    }
}
