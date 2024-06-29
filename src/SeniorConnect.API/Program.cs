using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SeniorConnect.API.Data;
using SeniorConnect.API.Service.UserService;
using SeniorConnect.API.Services.ActivityService;
using SeniorConnect.API.Services.ActivityService.Interface;
using SeniorConnect.API.Services.UserService;
using SeniorConnect.API.Services.UserService.Interface;
using SeniorConnect.API.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Register Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddControllers();
builder.Services.AddScoped<IActivityService, ActivityService>();

// Configure DbContext with connection string from configuration
var sqlConnection = builder.Configuration.GetConnectionString("SeniorConnectSqlDb");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(sqlConnection, sqlOptions =>
        sqlOptions.EnableRetryOnFailure()));

/*//allows for uploading files to storage
var storageConnection = builder.Configuration.GetConnectionString("SeniorConnectStorage");
builder.Services.AddAzureClients(azureBuilder =>
{
    azureBuilder.AddBlobServiceClient(storageConnection);
});
*/

var tokenKey = builder.Configuration["AppSettings:Token"]; // Retrieve token key from AppSettings
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer( options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenKey)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"], 
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
        };
    }
);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // Serve the Swagger UI at the app's root
    });

    // Enable detailed error messages in production
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();