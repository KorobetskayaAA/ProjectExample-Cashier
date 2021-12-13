using CashierDB.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CashierDB.Repositories
{
    public class CashierRepositoryBase
    {
        protected CashierContext context;

        public CashierRepositoryBase(CashierContext context)
        {
            this.context = context;
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
