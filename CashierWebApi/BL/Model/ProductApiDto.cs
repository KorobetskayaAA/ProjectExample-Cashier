using CashierDB.Model.DTO;
using System.Text.Json.Serialization;

namespace CashierWebApi.BL.Model
{
    public class ProductApiDto
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ProductApiDto() { }

        public ProductDbDto Create()
        {
            return new ProductDbDto()
            {
                Barcode = Barcode,
                Name = Name,
                Price = Price
            };
        }

        public void Update(ProductDbDto product)
        {
            product.Barcode = Barcode;
            product.Name = Name;
            product.Price = Price;
        }

        public ProductApiDto(ProductDbDto product)
        {
            Barcode = product.Barcode;
            Name = product.Name;
            Price = product.Price;
        }
    }
}
