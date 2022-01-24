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
        public string Creator { get; set; }
        public IEnumerable<ItemApiDto> Items { get; set; }
        public BillStatusApiDto Status { get; set; }
        public decimal Cost => Items.Sum(i => i.Cost);

        public BillApiDto() { }

        public BillApiDto(BillDbDto bill) 
        {
            Number = bill.Number;
            Created = bill.Created;
            if (bill.Creator != null) {
                if (string.IsNullOrWhiteSpace(bill.Creator.LastName))
                {
                    Creator = bill.Creator.UserName;
                }
                else
                {
                    Creator = bill.Creator.LastName + " ";
                    if (!string.IsNullOrEmpty(bill.Creator.FirstName))
                    {
                        Creator += bill.Creator.FirstName.Substring(0, 1) + '.';
                    }
                    if (!string.IsNullOrEmpty(bill.Creator.MiddleName))
                    {
                        Creator += bill.Creator.MiddleName.Substring(0, 1) + '.';
                    }
                }
            }
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
