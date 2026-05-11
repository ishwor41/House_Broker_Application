using HouseBroker.Application.Interfaces;
using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Enums;
using HouseBroker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HouseBroker.Infrastructure.Repositories
{
    public class CommissionSettingRepository: ICommissionSettingRepository
    {
        private readonly AppDbContext _context;

        public CommissionSettingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CommissionSetting>> GetAllAsync()
        {
            return await _context.CommissionSettings
            .Where(x => x.RecordStatus == RecordStatus.Active)
             .ToListAsync();
        }
    }
}