using CashierDB.Model.DTO;
using CashierModel;

namespace ConsoleCashier.DB.Mapping
{
    static class ProductMapper
    {
        public static Product Map(ProductDbDto product)
        {
            if (product == null)
                return null;
            return new Product(product.Barcode, product.Name, product.Price);
        }

        public static ProductDbDto Map(Product product)
        {
            if (product == null)
                return null;
            return new ProductDbDto() 
            {
                Barcode = product.Barcode,
                Name = product.Name,
                Price = product.Price,
            };
        }
    }
}
