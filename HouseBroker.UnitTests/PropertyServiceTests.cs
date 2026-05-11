using Xunit;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using HouseBroker.Application.Services;
using HouseBroker.Application.Interfaces;
using HouseBroker.Domain.Entities;
using HouseBroker.Application.DTOs.Property;

namespace HouseBroker.Tests.Services
{
    public class PropertyServiceTests
    {
        private readonly Mock<IPropertyRepository> _repoMock = new();
        private readonly Mock<ICommissionService> _commissionMock = new();
        private readonly Mock<ICurrentUserService> _userMock = new();
        private readonly Mock<IMemoryCache> _cacheMock = new();
        private readonly Mock<IFileService> _fileMock = new();

        private readonly PropertyService _service;

        public PropertyServiceTests()
        {
            _service = new PropertyService(
                _repoMock.Object,
                _commissionMock.Object,
                _userMock.Object,
                _cacheMock.Object,
                _fileMock.Object
            );
        }

        [Fact]
        public async Task Add_ShouldCreatePropertySuccessfully()
        {
        
            var dto = new PropertyDto
            {
                Title = "Test House",
                PropertyType = "House",
                Location = "Kathmandu",
                Price = 1000,
                Features = "Nice view"
            };

            _commissionMock.Setup(x => x.Calculate(It.IsAny<decimal>()))
                           .ReturnsAsync(50);

            _fileMock.Setup(x => x.SaveImageAsync(It.IsAny<IFormFile>(), It.IsAny<string>()))
                     .ReturnsAsync("/images/test.jpg");

            _repoMock.Setup(x => x.AddAsync(It.IsAny<Property>()))
                     .Returns(Task.CompletedTask);

            _userMock.Setup(x => x.UserId)
                     .Returns(Guid.NewGuid());


            await _service.Add(dto);

            _repoMock.Verify(x => x.AddAsync(It.IsAny<Property>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldModifyPropertySuccessfully()
        {
 
            var id = Guid.NewGuid();

            var existing = new Property
            {
                Id = id,
                Title = "Old Title"
            };

            var dto = new PropertyDto
            {
                Title = "New Title",
                PropertyType = "House",
                Location = "Kathmandu",
                Price = 2000,
                Features = "Updated features"
            };

            _repoMock.Setup(x => x.GetByIdAsync(id,null))
                     .ReturnsAsync(existing);

            _commissionMock.Setup(x => x.Calculate(It.IsAny<decimal>()))
                           .ReturnsAsync(100);

            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Property>()))
                     .Returns(Task.CompletedTask);

            _userMock.Setup(x => x.UserId)
                     .Returns(Guid.NewGuid());


            await _service.Update(id, dto);

 
            _repoMock.Verify(x => x.UpdateAsync(It.Is<Property>(p =>
                p.Title == "New Title" &&
                p.Price == 2000
            )), Times.Once);
        }
    }
}
