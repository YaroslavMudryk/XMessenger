using XMessenger.Helpers;
using XMessenger.Infrastructure.IoC;
using XMessenger.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
    options.ReportApiVersions = true;
});
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddXMessengerServices(builder.Configuration);
builder.Services.AddSwaggerGen();

#endregion

var app = builder.Build();

#region Config

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalErrorHandler();

app.UseApiVersioning();

app.UseHttpsRedirection();

app.UseSessionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion