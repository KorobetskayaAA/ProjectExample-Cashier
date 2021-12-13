using CashierDB.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashierWebApi.BL.Model
{
    public class ItemApiDto
    {
        public string Barcode { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public double Amount { get; set; }
        public decimal Cost => Price * (decimal)Amount;

        public ItemApiDto() { }

        public ItemApiDto(ItemDbDto item)
        {
            Barcode = item.Barcode;
            Name = item.Name;
            Price = item.Price;
            Amount = item.Amount;
        }

        public ItemDbDto Create()
        {
            return new ItemDbDto()
            {
                Barcode = Barcode,
                Name = Name,
                Price = Price,
                Amount = Amount,
            };
        }
    }
}
