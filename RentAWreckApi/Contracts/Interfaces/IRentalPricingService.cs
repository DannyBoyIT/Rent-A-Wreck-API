using Contracts.Constants;

namespace Contracts.Interfaces;

public interface IRentalPricingService
{
	/// <summary>
	/// Calculates the rental price.
	/// </summary>
	/// <param name="carCategory">The car category.</param>
	/// <param name="daysRented">The total days for the rental.</param>
	/// <param name="kilometersTraveled">The total kilometers traveled for the rental.</param>
	/// <returns>The final rental price for the provided parameters.</returns>
	decimal CalculateRentalPrice(CarCategory carCategory, double daysRented, int kilometersTraveled);
}