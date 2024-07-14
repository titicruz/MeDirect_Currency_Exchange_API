using MeDirect_Currency_Exchange_API.Data;
using MeDirect_Currency_Exchange_API.Interfaces;
using MeDirect_Currency_Exchange_API.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
if(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development") {
    builder.Services.AddHttpClient<IRateProviderClient, MockRateProviderClient>();
    builder.Services.AddScoped<IRateProviderClient, MockRateProviderClient>();
} else {
    builder.Services.AddHttpClient<IRateProviderClient, FixerRateProviderClient>();
    builder.Services.AddScoped<IRateProviderClient, FixerRateProviderClient>();
}
builder.Services.AddSingleton<ICacheService, CacheService>();
builder.Services.AddScoped<IExchangeService, ExchangeService>();

builder.Services.AddDbContext<Currency_Exchange_API_Context>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DBConnection")));

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
