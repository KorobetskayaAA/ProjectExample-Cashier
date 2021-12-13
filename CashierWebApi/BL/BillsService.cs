using CashierDB.Repositories;
using CashierWebApi.BL.Exceptions;
using CashierWebApi.BL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashierWebApi.BL
{
    public class BillsService
    {
        BillRepository billRepository;
        BillStatusRepository statusRepository;

        public BillsService(BillRepository billRepository, BillStatusRepository statusRepository)
        {
            this.billRepository = billRepository;
            this.statusRepository = statusRepository;
        }

        public async Task<IEnumerable<BillApiDto>> GetAllAsync()
        {
            var bills = await billRepository.GetAllAsync();
            return bills.Select(b => new BillApiDto(b));
        }

        public async Task<IEnumerable<BillStatusApiDto>> GetAllStatusesAsync()
        {
            var statuses = await statusRepository.GetAllAsync();
            return statuses.Select(bs => new BillStatusApiDto(bs));
        }

        public async Task<BillApiDto> GetAsync(uint number)
        {
            var bill = await billRepository.GetAsync(number);
            if (bill == null)
            {
                return null;
            }
            return new BillApiDto(bill);
        }

        public async Task<Exception> CreateAsync(IEnumerable<ItemApiDto> items)
        {
            var bill = BillApiDto.Create(items);
            billRepository.Create(bill);
            try
            {
                await billRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                return new SaveChangesException(ex);
            }
            return null;
        }

        public async Task<(BillApiDto, Exception)> CancelAsync(uint number)
        {
            var bill = await billRepository.GetAsync(number);
            if (bill == null)
            {
                return (null, new KeyNotFoundException($"Чек с номером {number} не найден"));
            }

            BillApiDto.Cancel(bill);
            try
            {
                await billRepository.SaveAsync();
            }
            catch (Exception ex)
            {
                return (null, new SaveChangesException(ex));
            }
            return (new BillApiDto(bill), null);
        }
    }
}
