using Contracts.Models.Dtos;

namespace Contracts.Interfaces;

public interface IBookingRepository
{
	/// <summary>
	/// Patches the booking.
	/// </summary>
	/// <param name="bookingDto">The booking object to patch.</param>
	Task PatchBookingAsync(BookingDto bookingDto);

	/// <summary>
	/// Gets a specific booking.
	/// </summary>
	/// <param name="id">The ID for the booking.</param>
	/// <returns>The DTO for the requested booking.</returns>
	Task<BookingDto?> GetBookingAsync(int id);
}