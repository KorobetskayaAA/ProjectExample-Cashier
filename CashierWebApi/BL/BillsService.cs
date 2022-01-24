using CashierDB.Model.DTO;
using CashierDB.Repositories;
using CashierWebApi.BL.Exceptions;
using CashierWebApi.BL.Model;
using Microsoft.AspNetCore.Identity;
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
        UserManager<UserDbDto> userManager;

        public BillsService(BillRepository billRepository, BillStatusRepository statusRepository,
            UserManager<UserDbDto> userManager)
        {
            this.billRepository = billRepository;
            this.statusRepository = statusRepository;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<BillApiDto>> GetAllAsync(int? statusId, string orderBy, bool orderAsc)
        {
            var bills = await billRepository.GetAllAsync(statusId, orderBy, orderAsc);
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

        public async Task<Exception> CreateAsync(IEnumerable<ItemApiDto> items, string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            var bill = BillApiDto.Create(items, user.Id);
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
