using CashierDB.Model.DTO;
using CashierModel;
using System.Linq;

namespace ConsoleCashier.DB.Mapping
{
    static class BillMapper
    {
        public static Bill Map(BillDbDto bill)
        {
            if (bill == null)
                return null;
            return new Bill(
                (uint)bill.Number,
                bill.Created,
                (BillStatus)bill.StatusId,
                bill.Items.Select(i => ItemMapper.Map(i)).ToList()
            );
        }

        public static BillDbDto Map(Bill bill)
        {
            if (bill == null)
                return null;
            return new BillDbDto() 
            {
                Number = bill.Number,
                Created = bill.Created,
                StatusId = (int)bill.Status,
                Items = bill.Items.Select(i => ItemMapper.Map(i)).ToList()
            };
        }
    }
}
