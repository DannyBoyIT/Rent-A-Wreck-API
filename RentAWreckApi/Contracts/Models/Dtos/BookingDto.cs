using Contracts.Constants;

namespace Contracts.Models.Dtos;

public record BookingDto
{
	public int BookingNumber { get; set; }
	public string? RegistrationNumber { get; set; }
	public CarCategory Category { get; set; }
	public string? SocialSecurityNumber { get; set; }
	public DateTimeOffset PickupDate { get; set; }
	public int PickupOdometer { get; set; }
	public DateTimeOffset ReturnDate { get; set; }
	public int ReturnOdometer { get; set; }
	public decimal RentalPrice { get; set; }
}
