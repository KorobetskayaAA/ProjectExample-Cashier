using CashierDB.Model;
using CashierDB.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Repositories
{
    public class BillRepository : CashierRepositoryBase
    {
        public BillRepository(CashierContext context) : base(context) {}

        public async Task<IEnumerable<BillDbDto>> GetAllAsync()
        {
            return await context.Bills
                .Include(b => b.Items)
                .Include(b => b.Status)
                .ToListAsync();
        }

        public async Task<BillDbDto> GetAsync(long number)
        {
            return await context.Bills
                .Include(b => b.Items)
                .Include(b => b.Status)
                .FirstOrDefaultAsync(b => b.Number == number);
        }

        public void Create(BillDbDto bill)
        {
            context.Bills.Add(bill);
        }
    }
}
