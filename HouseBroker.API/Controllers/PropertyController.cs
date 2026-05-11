using Microsoft.AspNetCore.Mvc;
using HouseBroker.Application.Interfaces;
using HouseBroker.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using HouseBroker.Application.DTOs.Property;

namespace HouseBroker.API.Controllers
{
    /// <summary>
    /// PropertyController handles all HTTP requests related to Property management.
    /// This is the API layer in Clean Architecture.
    /// It communicates only with the Application layer (IPropertyService).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _service;

        /// <summary>
        /// Constructor with dependency injection of application service.
        /// API layer depends only on abstraction (interface), not implementation.
        /// </summary>
        public PropertyController(IPropertyService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get all properties based on user role (HouseSeeker or Broker).
        /// Broker sees full listings, HouseSeeker sees filtered listings.
        /// </summary>
        /// 
        [Authorize(Roles = "HouseSeeker,Broker")]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PropertyFilterDto filter)
        {
            if (User.IsInRole("Broker"))
                return Ok(await _service.GetForBroker(filter));

            return Ok(await _service.GetForHouseSeeker(filter));
        }


        /// <summary>
        /// Create a new property listing.
        /// Only Broker is authorized to create properties.
        /// </summary>
        [Authorize(Roles = "Broker")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] PropertyDto property)
        {
            await _service.Add(property);
            return Ok();
        }

        /// <summary>
        /// Get property details by unique identifier.
        /// </summary>
        [Authorize(Roles = "HouseSeeker,Broker")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var property = await _service.GetById(id,User);

            if (property == null)
                return NotFound("Property not found");

            return Ok(property);
        }

        /// <summary>
        /// Update existing property details.
        /// Only Broker can update property data.
        /// </summary>
        [Authorize(Roles = "Broker")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] PropertyDto property)
        {

            await _service.Update(id,property);
            return Ok("Property updated successfully");
        }


        /// <summary>
        /// Delete a property by ID.
        /// </summary>
        [Authorize(Roles = "Broker")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            return Ok("Property deleted successfully");
        }
    }
}
