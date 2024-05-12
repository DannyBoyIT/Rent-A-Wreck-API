using Contracts.Constants;
using Contracts.Interfaces;
using Contracts.Models.Dtos;
using Moq;

namespace IntegrationTests;

public class MockHelpers
{
	public static BookingDto DefaultBookingDto => new()
	{
		BookingNumber = 12345
	};

	public static void SetupBookingRepositoryWithDefaultBooking(Mock<IBookingRepository> mock)
	{
		mock.Setup(repo => repo.GetBookingAsync(It.IsAny<int>()))
			.ReturnsAsync(DefaultBookingDto);
	}
	
	public static void SetupBookingRepositoryWithNullBooking(Mock<IBookingRepository> mock)
	{
		mock.Setup(repo => repo.GetBookingAsync(It.IsAny<int>()))
			.ReturnsAsync(null as BookingDto);
	}

	public static void SetupBookingRepositoryWithCustomBooking(Mock<IBookingRepository> mock, BookingDto bookingDto)
	{
		mock.Setup(repo => repo.GetBookingAsync(It.IsAny<int>()))
			.ReturnsAsync(bookingDto);
	}
}