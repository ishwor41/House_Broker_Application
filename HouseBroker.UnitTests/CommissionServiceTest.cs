using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using HouseBroker.Application.Services;
using HouseBroker.Application.Interfaces;
using HouseBroker.Domain.Entities;

namespace HouseBroker.UnitTests
{


    public class CommissionServiceTests
    {
        private readonly Mock<ICommissionSettingRepository> _repoMock;
        private readonly CommissionService _service;

        public CommissionServiceTests()
        {
            _repoMock = new Mock<ICommissionSettingRepository>();
            _service = new CommissionService(_repoMock.Object);
        }

        [Fact]
        public async Task Calculate_ShouldReturnCorrectCommission()
        {
            // Arrange
            decimal price = 1000;

            var rules = new List<CommissionSetting>
        {
            new CommissionSetting
            {
                MinPrice = 0,
                MaxPrice = 5000,
                Percentage = 5
            }
        };

            _repoMock.Setup(r => r.GetAllAsync())
                     .ReturnsAsync(rules);

            // Act
            var result = await _service.Calculate(price);

            // Assert
            Assert.Equal(50, result);
        }
    }
}
