using HouseBroker.Application.DTOs.Property;
using HouseBroker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HouseBroker.Application.Interfaces
{
    public interface IPropertyService
    {
        Task<List<PropertyBrokerDto>> GetForBroker(PropertyFilterDto filter);
        Task<List<PropertyHouseSeekerDto>> GetForHouseSeeker(PropertyFilterDto filter);
        Task<PropertyHouseSeekerDto?> GetByIdForHouseSeeker(Guid id);
        Task<PropertyBrokerDto?> GetByIdForBroker(Guid id);
        Task<object?> GetById(Guid id, ClaimsPrincipal User);
        Task Add(PropertyDto property);
        Task Update(Guid id, PropertyDto property);
        Task Delete(Guid id);
    }
}
