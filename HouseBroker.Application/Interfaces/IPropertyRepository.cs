using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HouseBroker.Domain.Entities;
using HouseBroker.Application.DTOs.Property;

namespace HouseBroker.Application.Interfaces
{
    public interface IPropertyRepository
    {
        Task<List<Property>> GetAllAsync(PropertyFilterDto filter, Guid? UserId);
        Task<Property?> GetByIdAsync(Guid id, Guid? UserId);
        Task AddAsync(Property property);
        Task UpdateAsync(Property property);
        Task DeleteAsync(Guid id);
    }
}
