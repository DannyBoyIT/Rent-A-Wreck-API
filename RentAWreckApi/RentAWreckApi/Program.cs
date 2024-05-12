using Contracts.Interfaces;
using RentAWreckApi.Services;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddTransient<IRentalPricingService, RentalPricingService>();
builder.Services.AddTransient<IBookingService, BookingService>();
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }