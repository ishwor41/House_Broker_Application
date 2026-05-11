using HouseBroker.Application.DTOs.Property;
using HouseBroker.Application.Interfaces;
using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Enums;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace HouseBroker.Application.Services
{
    /// <summary>
    /// PropertyService contains business logic for Property management.
    /// This belongs to Application layer in Clean Architecture.
    /// </summary>
    public class PropertyService: IPropertyService
    {
        private readonly ICommissionService _commissionService;
        private readonly IPropertyRepository _repo;
        private readonly ICurrentUserService _currentUser;
        private readonly IMemoryCache _cache;
        private readonly IFileService _fileService;

        public PropertyService(IPropertyRepository repo, ICommissionService commissionService, ICurrentUserService currentUser, IMemoryCache cache, IFileService fileService)
        {
            _repo = repo;
            _commissionService = commissionService;
            _currentUser = currentUser;
            _cache = cache;
            _fileService = fileService;
        }

        #region GET - House Seeker
        /// <summary>
        /// Returns simplified property list for House Seeker users.
        /// Uses caching for performance when no filters are applied.
        /// </summary>
        public async Task<List<PropertyHouseSeekerDto>> GetForHouseSeeker(PropertyFilterDto filter)
        {
 
            bool hasFilter =
                !string.IsNullOrEmpty(filter.Title) ||
                !string.IsNullOrEmpty(filter.PropertyType) ||
                !string.IsNullOrEmpty(filter.Location) ||
                filter.MinPrice.HasValue ||
                filter.MaxPrice.HasValue;

            if (hasFilter)
            {
                var filtered = await _repo.GetAllAsync(filter, null);

                return filtered.Select(p => new PropertyHouseSeekerDto
                {
                    Title = p.Title,
                    Features = p.Features,
                    Location = p.Location,
                    Price = p.Price,
                    ImagePath = p.PropertyImageUrl
                }).ToList();
            }

   
            string cacheKey = "properties_house_seeker";

            if (_cache.TryGetValue(cacheKey, out List<PropertyHouseSeekerDto> cached))
                return cached;

            var properties = await _repo.GetAllAsync(filter, null);

            var result = properties.Select(p => new PropertyHouseSeekerDto
            {
                Title = p.Title,
                Features = p.Features,
                Location = p.Location,
                Price = p.Price,
                ImagePath = p.PropertyImageUrl
            }).ToList();

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        

        }
        #endregion


        #region GET - Broker
        /// <summary>
        /// Returns full property details for Broker including commission.
        /// Supports filtering and caching.
        /// </summary>
        public async Task<List<PropertyBrokerDto>> GetForBroker(PropertyFilterDto filter)
        {
            bool hasFilter =
                !string.IsNullOrEmpty(filter.Title) ||
                !string.IsNullOrEmpty(filter.PropertyType) ||
                !string.IsNullOrEmpty(filter.Location) ||
                filter.MinPrice.HasValue ||
                filter.MaxPrice.HasValue;

            var userId = _currentUser.UserId;


            if (hasFilter)
            {
                var filteredProperties = await _repo.GetAllAsync(filter, userId);

                return filteredProperties.Select(p => new PropertyBrokerDto
                {
                    Title = p.Title,
                    Features = p.Features,
                    Location = p.Location,
                    Price = p.Price,
                    Commission = p.CommissionAmount,
                    ImagePath = p.PropertyImageUrl
                }).ToList();
            }

            string cacheKey = "properties_broker";

            if (_cache.TryGetValue(cacheKey, out List<PropertyBrokerDto> cached))
                return cached;

            var properties = await _repo.GetAllAsync(filter, userId);

            var result = properties.Select(p => new PropertyBrokerDto
            {
                Title = p.Title,
                Features = p.Features,
                Location = p.Location,
                Price = p.Price,
                Commission = p.CommissionAmount,
                ImagePath = p.PropertyImageUrl
            }).ToList();

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }
        #endregion

        public async Task<object?> GetById(Guid id,ClaimsPrincipal user)
        {
            if (user.IsInRole("Broker"))
                return await GetByIdForBroker(id);

            return await GetByIdForHouseSeeker(id);
        }
        #region GET BY ID
        /// <summary>
        /// Get single property by Id
        /// </summary>
        public async Task<PropertyHouseSeekerDto?> GetByIdForHouseSeeker(Guid id)
        {
            var property = await _repo.GetByIdAsync(id,null);

            if (property == null)
                return null;

            return new PropertyHouseSeekerDto
            {
                Title = property.Title,
                Price = property.Price,
                Location = property.Location,
                Features = property.Features,
                ImagePath = property.PropertyImageUrl
            };


        }

        #endregion

        #region GET BY ID
        /// <summary>
        /// Get single property by Id
        /// </summary>
        public async Task<PropertyBrokerDto?> GetByIdForBroker(Guid id)
        {
            var userId = _currentUser.UserId;
            var property = await _repo.GetByIdAsync(id, userId);

            if (property == null)
                return null;

            return new PropertyBrokerDto
            {
                Title = property.Title,
                Price = property.Price,
                Location = property.Location,
                Features = property.Features,
                ImagePath = property.PropertyImageUrl,
                Commission = property.CommissionAmount
            };
        }

        #endregion



        #region ADD PROPERTY
        /// <summary>
        /// Creates a new property listing.
        /// Calculates commission and assigns metadata.
        /// </summary>
        public async Task Add(PropertyDto propertyDto)
        {
            if (propertyDto == null)
                throw new ArgumentNullException(nameof(propertyDto));

            var imagePath = await _fileService.SaveImageAsync(propertyDto.PropertyImage, "property-images");
            var commission = await _commissionService.Calculate(propertyDto.Price);
            var property = new Property
            {
                Id = Guid.NewGuid(),
                Title = propertyDto.Title,
                PropertyType = propertyDto.PropertyType.ToString(),
                Location = propertyDto.Location,
                Price = propertyDto.Price,
                Features = propertyDto.Features,
                CommissionAmount = commission,
                PropertyImageUrl = imagePath,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = _currentUser.UserId,
                RecordStatus = RecordStatus.Active
            };

            _cache.Remove("properties_house_seeker");
            _cache.Remove("properties_broker");
            await _repo.AddAsync(property);
        }
        #endregion

        #region UPDATE PROPERTY
        /// <summary>
        /// Updates existing property details.
        /// </summary>
        public async Task Update(Guid id,PropertyDto propertyDto)
        {
            if (propertyDto == null)
                throw new ArgumentNullException(nameof(propertyDto));

            var existing = await _repo.GetByIdAsync(id,null);

            if (existing == null)
                throw new Exception("Property not found");

            var commission = await _commissionService.Calculate(propertyDto.Price);
            var property = new Property
            {
                Id = id,
                Title = propertyDto.Title,
                PropertyType = propertyDto.PropertyType.ToString(),
                Location = propertyDto.Location,
                Price = propertyDto.Price,
                Features = propertyDto.Features,
                CommissionAmount = commission,
                UpdatedDate = DateTime.UtcNow,
                UpdatedBy = _currentUser.UserId,
                RecordStatus = RecordStatus.Active
            };

            if (propertyDto.PropertyImage != null)
            {
                var imagePath = await _fileService.SaveImageAsync(propertyDto.PropertyImage, "property-images");
                property.PropertyImageUrl = imagePath;
            }

            _cache.Remove("properties_house_seeker");
            _cache.Remove("properties_broker");
            await _repo.UpdateAsync(property);
        }
        #endregion

        #region DELETE PROPERTY
        /// <summary>
        /// Deletes property by Id.
        /// </summary>
        public async Task Delete(Guid id)
        {
            var existing = await _repo.GetByIdAsync(id,null);
            if (existing == null)
                throw new Exception("Property not found");

            _cache.Remove("properties_house_seeker");
            _cache.Remove("properties_broker");
            await _repo.DeleteAsync(id);
        }

        #endregion
    }
}
