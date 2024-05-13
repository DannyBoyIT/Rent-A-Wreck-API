using Moq;
using Contracts.Interfaces;
using Contracts.Models.Dtos;
using RentAWreckApi.Services;

namespace UnitTests.Services;

public class BookingServiceTests
{
	private readonly Mock<IBookingRepository> _bookingRepositoryMock;
	private readonly BookingService _bookingService;

	public BookingServiceTests()
	{
		_bookingRepositoryMock = new Mock<IBookingRepository>();
		var rentalPricingServiceMock = new Mock<IRentalPricingService>();
		_bookingService = new BookingService(_bookingRepositoryMock.Object, rentalPricingServiceMock.Object);
	}

	[Fact]
	public async Task RegisterReturn_ThrowsArgumentOutOfRangeException_WhenReturnDateIsBeforePickupDate()
	{
		// Arrange
		var bookingDto = new BookingDto { BookingNumber = 1, PickupOdometer = 1000, PickupDate = DateTime.Now.AddDays(-5) };
		var returnDto = new ReturnRegistrationDto(1, DateTime.Now.AddDays(-10), 1500);

		_bookingRepositoryMock.Setup(repo => repo.GetBookingAsync(It.IsAny<int>())).ReturnsAsync(bookingDto);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _bookingService.RegisterReturn(returnDto));
	}

	[Fact]
	public async Task RegisterReturn_ThrowsArgumentOutOfRangeException_WhenOdometerIsLessThanPickup()
	{
		// Arrange
		var bookingDto = new BookingDto { BookingNumber = 1, PickupOdometer = 1000, PickupDate = DateTime.Now.AddDays(-1) };
		var returnDto = new ReturnRegistrationDto(1, DateTime.Now, 500);

		_bookingRepositoryMock.Setup(repo => repo.GetBookingAsync(It.IsAny<int>())).ReturnsAsync(bookingDto);

		// Act & Assert
		await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _bookingService.RegisterReturn(returnDto));
	}
}