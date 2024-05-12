using Microsoft.AspNetCore.Mvc.Testing;
using Contracts.Interfaces;
using Moq;

namespace IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
	public Mock<IBookingRepository>? BookingRepositoryMock { get; private set; }

	protected override void ConfigureWebHost(IWebHostBuilder builder)
	{
		builder.ConfigureServices(services =>
		{
			// Remove the existing repository registration
			var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IBookingRepository));
			if (descriptor != null)
			{
				services.Remove(descriptor);
			}

			// Initialize and add the mock repository
			BookingRepositoryMock = new Mock<IBookingRepository>();
			services.AddScoped<IBookingRepository>(_ => BookingRepositoryMock.Object);
		});
	}

	public void ConfigureMockRepository(Action<Mock<IBookingRepository>> configure)
	{
		if (BookingRepositoryMock != null) configure(BookingRepositoryMock);
	}
}