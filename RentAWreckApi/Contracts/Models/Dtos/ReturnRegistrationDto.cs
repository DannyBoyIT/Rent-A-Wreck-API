using System.ComponentModel.DataAnnotations;

namespace Contracts.Models.Dtos;

public record ReturnRegistrationDto
(
	[Required]
	int BookingNumber,
	DateTimeOffset ReturnDate,
	[Range(1, int.MaxValue, ErrorMessage = "The odometer value must be positive.")]
	int ReturnOdometer
);