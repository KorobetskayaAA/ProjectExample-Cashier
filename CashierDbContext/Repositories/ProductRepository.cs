using CashierDB.Model;
using CashierDB.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Repositories
{
    public class ProductRepository : CashierRepositoryBase
    {
        public ProductRepository(CashierContext context) : base(context) { }

        public async Task<IEnumerable<ProductDbDto>> GetAllAsync(string search, bool? sortPriceAsc)
        {
            var products = context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                products = products
                    .Where(p => EF.Functions.Like(p.Name, $"%{search}%") 
                            || EF.Functions.Like(p.Barcode, $"%{search}%"));
            }
            if (sortPriceAsc == true)
            {
                products = products.OrderBy(p => p.Price);
            }
            else if (sortPriceAsc == false)
            {
                products = products.OrderByDescending(p => p.Price);
            }
            return await products.ToListAsync();
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
