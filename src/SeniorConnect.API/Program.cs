global using Microsoft.EntityFrameworkCore;
using SeniorConnect.API.Data;
using SeniorConnect.API.Service.UserService;
using SeniorConnect.API.Services.ActivityService;
using SeniorConnect.API.Services.ActivityService.Interface;
using SeniorConnect.API.Services.UserService;
using SeniorConnect.API.Services.UserService.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Service
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IAuthenticationService,AuthenticationService>();
builder.Services.AddScoped<ITokenService,TokenService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddControllers();
builder.Services.AddScoped<IActivityService, ActivityService>();

// Configure DbContext with connection string from appsettings.json
builder.Services.AddDbContext<DataContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddControllers()
    .AddJsonOptions( options => { 
            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve; 
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
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
