using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using HouseBroker.API.Controllers;
using HouseBroker.Application.Interfaces;

using HouseBroker.Application.DTOs.Property;

namespace HouseBroker.Tests.Controllers
{
    public class PropertyControllerTests
    {
        private readonly Mock<IPropertyService> _serviceMock;
        private readonly PropertyController _controller;

        public PropertyControllerTests()
        {
            _serviceMock = new Mock<IPropertyService>();
            _controller = new PropertyController(_serviceMock.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnOkResult()
        {
            // Arrange
            var dto = new PropertyDto
            {
                Title = "Test Property",
                PropertyType = "House",
                Location = "Kathmandu",
                Price = 1000,
                Features = "Nice view"
            };

            _serviceMock.Setup(x => x.Add(dto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Create_ShouldCallServiceOnce()
        {
            // Arrange
            var dto = new PropertyDto
            {
                Title = "Another Property",
                Price = 2000
            };

            _serviceMock.Setup(x => x.Add(It.IsAny<PropertyDto>()))
                        .Returns(Task.CompletedTask);

            // Act
            await _controller.Create(dto);

            // Assert
            _serviceMock.Verify(x => x.Add(It.IsAny<PropertyDto>()), Times.Once);
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenValidRequest()
        {
            // Arrange
            var dto = new PropertyDto
            {
                Title = "Valid Property",
                Price = 3000
            };

            _serviceMock.Setup(x => x.Add(dto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var okResult = Assert.IsType<OkResult>(result);
            Assert.NotNull(okResult);
        }
    }
}