using CashierDB.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashierWebApi.BL.Model
{
    public class BillApiDto
    {
        public long Number { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<ItemApiDto> Items { get; set; }
        public BillStatusApiDto Status { get; set; }
        public decimal Cost => Items.Sum(i => i.Cost);

        public BillApiDto() { }

        public BillApiDto(BillDbDto bill) 
        {
            Number = bill.Number;
            Created = bill.Created;
            Status = new BillStatusApiDto(bill.Status);
            Items = bill.Items.Select(i => new ItemApiDto(i));
        }

        public static BillDbDto Create(IEnumerable<ItemApiDto> items, string creatorId)
        {
            return new BillDbDto()
            {
                Created = DateTime.Now,
                StatusId = 1,
                CreatorId = creatorId,
                Items = items.Select(i => i.Create()).ToList(),
            };
        }

        public static void Cancel(BillDbDto bill)
        {
            bill.StatusId = 2;
        }
    }
}
