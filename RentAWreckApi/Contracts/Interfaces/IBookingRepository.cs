using Contracts.Models.Dtos;

namespace Contracts.Interfaces;

public interface IBookingRepository
{
    Task UpdateBookingAsync(BookingDto bookingDto);
    Task<BookingDto?> GetBookingAsync(int id);
}