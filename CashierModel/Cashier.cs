using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CashierModel
{
    public class Cashier
    {
        List<Bill> Bills { get; } = new List<Bill>();

        public Bill OpenNewBill()
        {
            return new Bill();
        }

        public Bill FindBill(uint number)
        {
            return Bills.Find(bill => bill.Number == number);
        }

        public IEnumerable<Bill> FindBills(BillStatus status)
        {
            return Bills.Where(bill => bill.Status == status);
        }
    }
}
