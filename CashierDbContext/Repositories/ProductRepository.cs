using CashierDB.Model;
using CashierDB.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Repositories
{
    public class ProductRepository : CashierRepositoryBase
    {
        public ProductRepository(CashierContext context) : base(context) { }

        public async Task<IEnumerable<ProductDbDto>> GetAllAsync()
        {
            var products = await context.Products.ToListAsync();
            return products;
        }

        public async Task<ProductDbDto> GetAsync(string barcode)
        {
            var product = await context.Products.FindAsync(barcode);
            return product;
        }

        public void Create(ProductDbDto product)
        {
            context.Products.Add(product);
        }

        public void Update(ProductDbDto product)
        {
            context.Products.Update(product);
        }

        public void Delete(ProductDbDto product)
        {
            context.Products.Remove(product);
        }

        public async Task<ProductDbDto> DeleteAsync(string barcode)
        {
            var product = await context.Products.FindAsync(barcode);
            if (product != null)
            {
                context.Products.Remove(product);
            }
            return product;
        }

        public async Task<bool> Exists(string barcode)
        {
            return await context.Products.AnyAsync(p => p.Barcode == barcode);
        }
    }
}
