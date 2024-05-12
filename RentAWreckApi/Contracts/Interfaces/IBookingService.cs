using Contracts.Models.Dtos;

namespace Contracts.Interfaces;

public interface IBookingService
{
	/// <summary>
	/// Registers the car pickup for an existing booking.
	/// </summary>
	/// <param name="pickupRegistrationDto">The pickup information to be registered.</param>
	/// <returns>True if the registration has succeeded or false if the booking was not found.</returns>
	Task<bool> RegisterPickup(PickupRegistrationDto pickupRegistrationDto);

	/// <summary>
	/// Registers the car return for an existing booking.
	/// </summary>
	/// <param name="returnRegistrationDto">The return information to be registered.</param>
	/// <returns>The final rental price for the booking or null if the booking was not found.</returns>
	Task<decimal?> RegisterReturn(ReturnRegistrationDto returnRegistrationDto);
}