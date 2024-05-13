using Contracts.Constants;
using Contracts.Interfaces;

namespace RentAWreckApi.Services;

public class RentalPricingService : IRentalPricingService
{
	public decimal CalculateRentalPrice(CarCategory carCategory, double daysRented, int kilometersTraveled)
	{
		if (daysRented < 1)
			throw new ArgumentOutOfRangeException(nameof(daysRented), daysRented, "Days rented must be at least 1 day.");

		if (kilometersTraveled < 0)
			throw new ArgumentOutOfRangeException(nameof(kilometersTraveled), kilometersTraveled, "Kilometers traveled cannot be negative.");

		switch (carCategory)
		{
			case CarCategory.Small:
				return PricingConstants.BaseDayPrice * (decimal)daysRented;
			case CarCategory.Estate:
				return PricingConstants.BaseDayPrice * PricingConstants.EstateAddition * (decimal)daysRented + PricingConstants.BaseKmPrice * PricingConstants.EstateAddition * kilometersTraveled;
			case CarCategory.Truck:
				return PricingConstants.BaseDayPrice * PricingConstants.TruckAddition * (decimal)daysRented + PricingConstants.BaseKmPrice * PricingConstants.TruckAddition * kilometersTraveled;
			default:
				throw new ArgumentException("Invalid car category provided.", nameof(carCategory));
		}
	}
}