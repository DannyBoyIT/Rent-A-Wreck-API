using Microsoft.AspNetCore.Mvc;
using System.Net;
using Contracts.Interfaces;
using Contracts.Models.Dtos;

namespace RentAWreckApi.Controllers;
[ApiController]
[Route("[controller]")]
public class BookingController : ControllerBase
{
	private readonly ILogger<BookingController> _logger;
	private readonly IBookingService _bookingService;

	public BookingController(ILogger<BookingController> logger, IBookingService bookingService)
	{
		_logger = logger;
		_bookingService = bookingService;
	}

	/// <summary>
	/// Registers a car pickup.
	/// </summary>
	/// <param name="pickupRegistrationDto">The pickup registration data.</param>
	[HttpPut("pickup")]
	[ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> RegisterPickup(PickupRegistrationDto pickupRegistrationDto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		try
		{
			var registrationSuccessful = await _bookingService.RegisterPickup(pickupRegistrationDto);

			if(registrationSuccessful == false)
				return NotFound();

			return Ok();
		}
		catch (Exception exception)
		{
			_logger.Log(LogLevel.Error, exception.Message, exception);
			return StatusCode((int)HttpStatusCode.InternalServerError);
		}
	}

	/// <summary>
	/// Registers a car return.
	/// </summary>
	/// <param name="returnRegistrationDto">The return registration data.</param>
	[HttpPut("return")]
	[ProducesResponseType(typeof(ReturnRegistrationDto), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> RegisterReturn([FromBody] ReturnRegistrationDto returnRegistrationDto)
	{
		if (!ModelState.IsValid)
			return BadRequest(ModelState);

		try
		{
			var rentalPrice = await _bookingService.RegisterReturn(returnRegistrationDto);

			if (rentalPrice == null)
				return NotFound();

			return Ok(rentalPrice);
		}
		catch (Exception exception)
		{
			_logger.Log(LogLevel.Error, exception, exception.Message);
			return StatusCode((int)HttpStatusCode.InternalServerError);
		}
	}
}
