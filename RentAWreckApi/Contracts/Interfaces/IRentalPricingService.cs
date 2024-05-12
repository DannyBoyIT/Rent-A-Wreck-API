using Contracts.Constants;

namespace Contracts.Interfaces;

public interface IRentalPricingService
{
    decimal? CalculateRentalPrice(CarCategory carCategory, double daysRented, int kilometersTraveled);
}