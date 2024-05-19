using Contracts.Constants;
using Api.Services;
using Contracts.Interfaces;

namespace UnitTests.Services;

public class RentalCostCalculatorServiceTests
{
    private readonly IRentalPricingService _rentalCostCalculatorService;

    public RentalCostCalculatorServiceTests()
    {
        _rentalCostCalculatorService = new RentalPricingService();
    }

    [Theory]
    [InlineData(CarCategory.Small, -1, 10)]
    [InlineData(CarCategory.Small, -50, 10)]
    [InlineData(CarCategory.Small, 0, 10)]
    [InlineData(CarCategory.Estate, -1, 10)]
    [InlineData(CarCategory.Estate, -50, 10)]
    [InlineData(CarCategory.Estate, 0, 10)]
    [InlineData(CarCategory.Truck, -1, 10)]
    [InlineData(CarCategory.Truck, -50, 10)]
    [InlineData(CarCategory.Truck, 0, 10)]
    public void CalculateRentalPrice_NegativeOrZeroDaysRented_ThrowsArgumentOutOfRangeException(CarCategory carCategory, double daysRented, int kilometersTraveled)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _rentalCostCalculatorService.CalculateRentalPrice(carCategory, daysRented, kilometersTraveled));
        Assert.Equal("daysRented", exception.ParamName);
    }

    [Fact]
    public void CalculateRentalPrice_NegativeKilometersTraveled_ThrowsArgumentOutOfRangeException()
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => _rentalCostCalculatorService.CalculateRentalPrice(CarCategory.Small, 1, -1));
        Assert.Equal("kilometersTraveled", exception.ParamName);
    }

    [Theory]
    [InlineData(CarCategory.Small, 1, 0, 350)]
    [InlineData(CarCategory.Small, 5, 0, 1750)]
    [InlineData(CarCategory.Small, 1, 150, 350)]
    [InlineData(CarCategory.Small, 5, 150, 1750)]
    [InlineData(CarCategory.Estate, 1, 0, 455)]
    [InlineData(CarCategory.Estate, 5, 0, 2275)]
    [InlineData(CarCategory.Estate, 1, 150, 1430)]
    [InlineData(CarCategory.Estate, 5, 150, 3250)]
    [InlineData(CarCategory.Truck, 1, 0, 525)]
    [InlineData(CarCategory.Truck, 5, 0, 2625)]
    [InlineData(CarCategory.Truck, 1, 150, 1650)]
    [InlineData(CarCategory.Truck, 5, 150, 3750)]
    public void CalculateRentalPrice_ValidInputs_ReturnsExpectedPrice(CarCategory carCategory, double daysRented, int kilometersTraveled, decimal expectedPrice)
    {
        // Act
        var result = _rentalCostCalculatorService.CalculateRentalPrice(carCategory, daysRented, kilometersTraveled);

        // Assert
        Assert.Equal(expectedPrice, result);
    }

    [Fact]
    public void CalculateRentalPrice_InvalidCarCategory_ThrowsArgumentException()
    {
        // Arrange
        CarCategory invalidCategory = (CarCategory)999;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _rentalCostCalculatorService.CalculateRentalPrice(invalidCategory, 1, 10));
        Assert.Equal("carCategory", exception.ParamName);
    }
}