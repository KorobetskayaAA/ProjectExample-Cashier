using CashierModel;
using System.Xml.Serialization;

namespace ConsoleCashier
{
    [XmlType(TypeName = "Product")]
    public class ProductFileDto
    {
        [XmlAttribute("barcode")]
        public string Barcode { get; set; }
        public string Name { get; set; }
        public int PriceKopecs { get; set; }

        public static ProductFileDto Map(Product product)
        {
            return new ProductFileDto()
            {
                Barcode = product.Barcode,
                Name = product.Name,
                PriceKopecs = (int)(product.Price * 100),
            };
        }

        public static Product Map(ProductFileDto product)
        {
            return new Product(
                product.Barcode,
                product.Name,
                product.PriceKopecs / 100m
            );
        }
    }
}
