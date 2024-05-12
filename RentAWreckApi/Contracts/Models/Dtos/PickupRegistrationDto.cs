using System.ComponentModel.DataAnnotations;
using Contracts.Constants;

namespace Contracts.Models.Dtos;

public record PickupRegistrationDto
(
	[Required]
	int BookingNumber,
	[Required]
	[StringLength(10, MinimumLength = 1, ErrorMessage = "The registration number is out of bounds. Min is 1 and max is 10 characters.")]
	string RegistrationNumber,
	CarCategory Category,
	[Required]
	[StringLength(20, ErrorMessage = "The social security number is out of bounds. Max is 20 characters.")]
	string SocialSecurityNumber,
	DateTimeOffset PickupDate,
	[Range(0, int.MaxValue, ErrorMessage = "The odometer value must be positive.")]
	int PickupOdometer
);