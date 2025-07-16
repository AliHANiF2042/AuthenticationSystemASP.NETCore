using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using User_Registration_System;
using User_Registration_System.Context;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<context>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));
});
builder.Services.AddScoped<iUserServices, UserServices>();

#region Cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/UserPanel/Home/Logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

#region Endpoints
app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
           name: "UserPanel",
           areaName: "UserPanel",
           pattern: "UserPanel/{controller=Home}/{action=Profile}/{id?}");

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Register}/{id?}");
});
#endregion

void ConfigurationServices(IServiceCollection services)
{
    services.AddDbContext<context>(options =>
    {
        options.UseSqlServer("ConnectionString");
    });
}

app.Run();
