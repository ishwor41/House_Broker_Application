using HouseBroker.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseBroker.Application.Services
{
    public class CommissionService: ICommissionService
    {
        private readonly ICommissionSettingRepository _repo;
        public CommissionService(ICommissionSettingRepository repo)
        {
            _repo = repo;
        }
        public async Task<decimal> Calculate(decimal price)
        {
            var rules = await _repo.GetAllAsync();

            var rule = rules.FirstOrDefault(r =>
                price >= r.MinPrice &&
                (r.MaxPrice == null || price < r.MaxPrice));

            if (rule == null)
                return 0;

            return price * rule.Percentage / 100;
        }
    }
}
