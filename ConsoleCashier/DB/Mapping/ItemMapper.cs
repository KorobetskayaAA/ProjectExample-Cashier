using CashierDB.Model.DTO;
using CashierModel;

namespace ConsoleCashier.DB.Mapping
{
    static class ItemMapper
    {
        public static Item Map(ItemDbDto item)
        {
            if (item == null)
                return null;
            return new Item(new Product(item.Barcode, item.Name, item.Price), item.Amount);
        }

        public static ItemDbDto Map(Item item)
        {
            if (item == null)
                return null;
            return new ItemDbDto()
            {
                Barcode = item.Barcode,
                Name = item.Name,
                Price = item.Price,
                Amount = item.Amount,
            };
        }
    }
}
