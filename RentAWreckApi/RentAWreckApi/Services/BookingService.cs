using Contracts.Interfaces;
using Contracts.Models.Dtos;

namespace RentAWreckApi.Services;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRentalPricingService _rentalCostCalculatorService;

    public BookingService(IBookingRepository bookingRepository, IRentalPricingService rentalCostCalculatorService)
    {
        _bookingRepository = bookingRepository;
        _rentalCostCalculatorService = rentalCostCalculatorService;
    }

    /// <inheritdoc />
	public async Task<bool> RegisterPickup(PickupRegistrationDto pickupRegistrationDto)
    {
	    var booking = await _bookingRepository.GetBookingAsync(pickupRegistrationDto.BookingNumber);

	    if (booking is null)
		    return false;
	    
        booking.RegistrationNumber = pickupRegistrationDto.RegistrationNumber;
        booking.Category = pickupRegistrationDto.Category;
        booking.SocialSecurityNumber = pickupRegistrationDto.SocialSecurityNumber;
        booking.PickupDate = pickupRegistrationDto.PickupDate;
        booking.PickupOdometer = pickupRegistrationDto.PickupOdometer;

        await _bookingRepository.PatchBookingAsync(booking);

        return true;
    }

    /// <inheritdoc />
	public async Task<decimal?> RegisterReturn(ReturnRegistrationDto returnRegistrationDto)
    {
	    var booking = await _bookingRepository.GetBookingAsync(returnRegistrationDto.BookingNumber);

	    if (booking is null)
		    return null;
	    
	    if (returnRegistrationDto.ReturnDate < booking.PickupDate)
		    throw new ArgumentOutOfRangeException(nameof(returnRegistrationDto.ReturnOdometer), returnRegistrationDto.ReturnOdometer, "The return date cannot be earlier than the pick up date.");

		if (returnRegistrationDto.ReturnOdometer < booking.PickupOdometer)
		    throw new ArgumentOutOfRangeException(nameof(returnRegistrationDto.ReturnOdometer), returnRegistrationDto.ReturnOdometer, "The odometer cannot be less when the car was returned than when the car was picked up.");
		
		var daysRented = Math.Ceiling((returnRegistrationDto.ReturnDate - booking.PickupDate).TotalDays);
	    var kilometersTraveled = returnRegistrationDto.ReturnOdometer - booking.PickupOdometer;
	    var rentalPrice = _rentalCostCalculatorService.CalculateRentalPrice(booking.Category, daysRented, kilometersTraveled);

	    booking.ReturnDate = returnRegistrationDto.ReturnDate;
	    booking.ReturnOdometer = returnRegistrationDto.ReturnOdometer;
	    booking.RentalPrice = rentalPrice;

	    await _bookingRepository.PatchBookingAsync(booking);

	    return rentalPrice;
    }
}