using CashierModel;
using System.Xml.Serialization;

namespace ConsoleCashier
{
    [XmlType(TypeName = "Item")]
    public class ItemFileDto
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Amount { get; set; }

        public static ItemFileDto Map(Item item)
        {
            return new ItemFileDto()
            {
                Barcode = item.Barcode,
                Name = item.Name,
                Price = item.Price,
                Amount = item.Amount,
            };
        }

        public static Item Map(ItemFileDto item)
        {
            return new Item(
                new Product(
                    item.Barcode,
                    item.Name,
                    item.Price
                ),
                item.Amount
            );
        }
    }
}
