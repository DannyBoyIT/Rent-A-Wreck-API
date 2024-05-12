using Contracts.Interfaces;
using Contracts.Models.Dtos;

namespace RentAWreckApi.Repositories;

//TODO: Implement repository
public class BookingRepository : IBookingRepository
{
	public Task UpdateBookingAsync(BookingDto bookingDto)
	{
		throw new NotImplementedException();
	}

	public Task<BookingDto?> GetBookingAsync(int id)
	{
		throw new NotImplementedException();
	}
}
