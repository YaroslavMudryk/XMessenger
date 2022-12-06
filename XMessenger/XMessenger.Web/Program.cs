using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using XMessenger.Helpers;
using XMessenger.Infrastructure.IoC;
using XMessenger.Web.Middlewares;

var builder = WebApplication.CreateBuilder(args);

#region Services

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1,0);
    options.ReportApiVersions = true;
});

builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = TokenOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = TokenOptions.Audience,
                    ValidateLifetime = true,
                    IssuerSigningKey = TokenOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                };
            });

builder.Services.AddXMessengerServices(builder.Configuration);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new TrimStringConverter());
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "XMessenger API",
    });


    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    s.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { jwtSecurityScheme, Array.Empty<string>() }
            });
});

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

app.UseStaticFiles();

app.UseRouting();

app.UseSessionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

#endregion