using ThirdPartyIntegrationApp.Interfaces;
using ThirdPartyIntegrationApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IApiCallerService, ApiCallerService>();
builder.Services.AddScoped<IDemoIntegrationService, DemoIntegrationService>();
builder.Services.AddHttpClient<IDemoApiCallerService, DemoApiCallerService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DemoApiConfiguration:BaseApiUrl"]!);
});

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
