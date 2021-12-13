using CashierDB.Model;
using CashierDB.Model.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Repositories
{
    public class BillStatusRepository : CashierRepositoryBase
    {
        public BillStatusRepository(CashierContext context) : base(context) { }

        public async Task<IEnumerable<BillStatusDbDto>> GetAllAsync()
        {
            return await context.BillStatuses.ToListAsync();
        }

        public async Task<BillStatusDbDto> GetAsync(int id)
        {
            return await context.BillStatuses.FindAsync(id);
        }
    }
}
