using Microsoft.AspNetCore.Authentication.Cookies;
using SeniorConnect.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// sets up an HTTP client connect to API
builder.Services.AddHttpClient("SeniorConnectAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5175/");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// register services
builder.Services.AddScoped<AuthService>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddControllersWithViews();

// setup authentication services for cookie-based authentication
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/login";
        option.LogoutPath = "/logout";
        option.ExpireTimeSpan = TimeSpan.FromDays(365);
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
        googleOptions.CallbackPath = new PathString("/signin-google");
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization();

app.MapRazorPages();


//default routing configuration for controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
