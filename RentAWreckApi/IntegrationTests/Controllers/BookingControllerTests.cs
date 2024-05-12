using Contracts.Constants;
using Contracts.Models.Dtos;
using FluentAssertions;
using System.Net;
using Bogus;

namespace IntegrationTests.Controllers;

public class BookingControllerTests : IClassFixture<CustomWebApplicationFactory>
{
	private readonly CustomWebApplicationFactory _factory;
	private readonly HttpClient _client;

	public BookingControllerTests(CustomWebApplicationFactory factory)
	{
		_factory = factory;
		_client = factory.CreateClient();
	}

	[Theory]
	[MemberData(nameof(GenerateValidPickupRegistrationDtos))]
	public async Task RegisterPickup_ReturnsOk_WhenDataIsValid(PickupRegistrationDto pickupRegistrationDto)
	{
		// Arrange
		_factory.ConfigureMockRepository(MockHelpers.SetupBookingRepositoryWithDefaultBooking);

		//Act
		var httpResponse = await _client.PatchAsJsonAsync("booking/pickup", pickupRegistrationDto);

		//Assert
		httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
	}

	[Theory]
	[MemberData(nameof(GenerateInvalidPickupRegistrationDtos))]
	public async Task RegisterPickup_ReturnsBadRequest_WhenDataIsInvalid(PickupRegistrationDto pickupRegistrationDto)
	{
		// Arrange
		_factory.ConfigureMockRepository(MockHelpers.SetupBookingRepositoryWithDefaultBooking);

		//Act
		var httpResponse = await _client.PatchAsJsonAsync("booking/pickup", pickupRegistrationDto);

		//Assert
		httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task RegisterPickup_ReturnsNotFound_WhenBookingDoesNotExist()
	{
		// Arrange
		var faker = new Faker();
		_factory.ConfigureMockRepository(MockHelpers.SetupBookingRepositoryWithNullBooking);
		var pickupRegistrationDto = new PickupRegistrationDto(MockHelpers.DefaultBookingDto.BookingNumber, faker.Random.Replace("???###"), faker.PickRandom<CarCategory>(), faker.Random.Replace("########-####"), faker.Date.RecentOffset(), faker.Random.Int(1, 9999));

		//Act
		var httpResponse = await _client.PatchAsJsonAsync("booking/pickup", pickupRegistrationDto);

		//Assert
		httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	[Theory]
	[MemberData(nameof(GenerateValidReturnRegistrationDtos))]
	public async Task RegisterReturn_ReturnsOk_WhenDataIsValid(BookingDto bookingDto, ReturnRegistrationDto returnRegistrationDto, int expectedFinalPrice)
	{
		// Arrange
		_factory.ConfigureMockRepository(mock => MockHelpers.SetupBookingRepositoryWithCustomBooking(mock, bookingDto));

		//Act
		var httpResponse = await _client.PatchAsJsonAsync("booking/return", returnRegistrationDto);

		//Assert
		httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
		if (httpResponse.IsSuccessStatusCode)
		{
			var actualFinalPrice = await httpResponse.Content.ReadFromJsonAsync<decimal>();
			actualFinalPrice.Should().Be(expectedFinalPrice);
		}
	}

	[Theory]
	[MemberData(nameof(GenerateInvalidReturnRegistrationDtos))]
	public async Task RegisterReturn_ReturnsBadRequest_WhenDataIsInvalid(ReturnRegistrationDto returnRegistrationDto)
	{
		// Arrange
		_factory.ConfigureMockRepository(MockHelpers.SetupBookingRepositoryWithDefaultBooking);

		//Act
		var httpResponse = await _client.PatchAsJsonAsync("booking/return", returnRegistrationDto);

		//Assert
		httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
	}

	[Fact]
	public async Task RegisterReturn_ReturnsNotFound_WhenBookingDoesNotExist()
	{
		// Arrange
		var faker = new Faker();
		_factory.ConfigureMockRepository(MockHelpers.SetupBookingRepositoryWithNullBooking);
		var pickupRegistrationDto = new ReturnRegistrationDto(MockHelpers.DefaultBookingDto.BookingNumber, faker.Date.RecentOffset(), faker.Random.Int(1, 9999));

		//Act
		var httpResponse = await _client.PatchAsJsonAsync("booking/return", pickupRegistrationDto);

		//Assert
		httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
	}

	#region Pickup data generation
	public static IEnumerable<object[]> GenerateValidPickupRegistrationDtos()
	{
		var faker = new Faker();
		yield return new object[] { new PickupRegistrationDto(faker.Random.Int(1, 999999), faker.Random.Replace("???###"), faker.PickRandom<CarCategory>(), faker.Random.Replace("########-####"), faker.Date.RecentOffset(), faker.Random.Int(1, 9999)) };
		yield return new object[] { new PickupRegistrationDto(faker.Random.Int(1, 999999), faker.Random.Replace("???###"), faker.PickRandom<CarCategory>(), faker.Random.Replace("########-####"), faker.Date.RecentOffset(), faker.Random.Int(1, 9999)) };
		yield return new object[] { new PickupRegistrationDto(faker.Random.Int(1, 999999), faker.Random.Replace("???###"), faker.PickRandom<CarCategory>(), faker.Random.Replace("########-####"), faker.Date.RecentOffset(), faker.Random.Int(1, 9999)) };
		yield return new object[] { new PickupRegistrationDto(faker.Random.Int(1, 999999), faker.Random.Replace("???###"), faker.PickRandom<CarCategory>(), faker.Random.Replace("########-####"), faker.Date.RecentOffset(), faker.Random.Int(1, 9999)) };
	}

	public static IEnumerable<object[]> GenerateInvalidPickupRegistrationDtos()
	{
		var faker = new Faker();
		//Invalid registration number
		yield return new object[] { new PickupRegistrationDto(faker.Random.Int(1, 999999), string.Empty, faker.PickRandom<CarCategory>(), faker.Random.Replace("########-####"), faker.Date.RecentOffset(), faker.Random.Int(1, 9999)) };
		//Invalid registration number
		yield return new object[] { new PickupRegistrationDto(faker.Random.Int(1, 999999), faker.Random.Replace("???????###########"), faker.PickRandom<CarCategory>(), faker.Random.Replace("########-####"), DateTimeOffset.Now, faker.Random.Int(1, 9999)) };
		//Invalid social security number
		yield return new object[] { new PickupRegistrationDto(faker.Random.Int(1, 999999), faker.Random.Replace("???###"), faker.PickRandom<CarCategory>(), faker.Random.Replace("################-#######"), DateTimeOffset.Now, faker.Random.Int(1, 9999)) };
		//Invalid odometer
		yield return new object[] { new PickupRegistrationDto(faker.Random.Int(1, 999999), faker.Random.Replace("???###"), faker.PickRandom<CarCategory>(), faker.Random.Replace("########-####"), DateTimeOffset.Now, faker.Random.Int(-9999, -1)) };
	}
	#endregion

	#region Return data generation

	public static IEnumerable<object[]> GenerateValidReturnRegistrationDtos()
	{
		var faker = new Faker();
		var bookingNumber = faker.Random.Int(1, 999999);
		var pickupDate = faker.Date.RecentOffset();
		var pickupOdometer = faker.Random.Int(1, 9999);

		yield return new object[]
		{
				new BookingDto { BookingNumber = bookingNumber, RegistrationNumber = faker.Random.Replace("???###"), Category = CarCategory.Small, SocialSecurityNumber = faker.Random.Replace("########-####"), PickupDate = pickupDate, PickupOdometer = pickupOdometer }, 
				new ReturnRegistrationDto(bookingNumber, pickupDate.AddDays(5), pickupOdometer + 150),
				1750 //expected price
		};
		yield return new object[]
		{
				new BookingDto { BookingNumber = bookingNumber, RegistrationNumber = faker.Random.Replace("???###"), Category = CarCategory.Estate, SocialSecurityNumber = faker.Random.Replace("########-####"), PickupDate = pickupDate, PickupOdometer = pickupOdometer }, 
				new ReturnRegistrationDto(bookingNumber, pickupDate.AddDays(5), pickupOdometer + 150),
				3250 //expected price
		};
		yield return new object[]
		{
				new BookingDto { BookingNumber = bookingNumber, RegistrationNumber = faker.Random.Replace("???###"), Category = CarCategory.Truck, SocialSecurityNumber = faker.Random.Replace("########-####"), PickupDate = pickupDate, PickupOdometer = pickupOdometer }, 
				new ReturnRegistrationDto(bookingNumber, pickupDate.AddDays(5), pickupOdometer + 150),
				3750 //expected price
		};
	}

	public static IEnumerable<object[]> GenerateInvalidReturnRegistrationDtos()
	{
		var faker = new Faker();
		//Invalid odometer
		yield return new object[] { new ReturnRegistrationDto(faker.Random.Int(1, 999999), DateTimeOffset.Now, faker.Random.Int(-9999, -1)) };
	}
	#endregion
}
