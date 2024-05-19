using Contracts.Interfaces;
using Contracts.Models.Dtos;

namespace Api.Repositories;

//TODO: Implement repository
public class BookingRepository : IBookingRepository
{
	public Task PatchBookingAsync(BookingDto bookingDto)
	{
		throw new NotImplementedException();
	}

	public Task<BookingDto?> GetBookingAsync(int id)
	{
		throw new NotImplementedException();
	}
}
