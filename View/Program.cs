﻿using Data.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using View.Handlers;
using View.IServices;
using View.Servicecs;
using View.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddTransient<AuthenticationHandler>();

builder.Services.AddHttpClient("ServerApi")
                .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7280" ?? ""))
                .AddHttpMessageHandler<AuthenticationHandler>();

builder.Services.AddAuthorizationCore();
//
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient<IAddresServices, AddresServices>();
builder.Services.AddHttpClient<IBrandServices, BrandServices>();
builder.Services.AddHttpClient<ISizeServices, SizeServices>();
builder.Services.AddHttpClient<IProductServices, ProductServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient<IImageServices, ImageServices>();
builder.Services.AddHttpClient<IMaterialServices, MaterialServices>();
builder.Services.AddHttpClient<ICartServices, CartServices>();
builder.Services.AddHttpClient<ICategoryServices, CategoryServices>();

//order
builder.Services.AddHttpClient<IOrderServices, OrderServices>();
//orderDetail
builder.Services.AddHttpClient<IOrderDetailServices, OrderDetailServices>();




var secret = builder.Configuration["JWT:Secret"] ?? throw new InvalidOperationException("Khóa bí mật chưa đc tạo ");
builder.Services.AddAuthentication(options =>
{
	// Đặt cookie cho giao diện web
	options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Mặc định cho MVC
	options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
	options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
	options.LoginPath = "/Authentication/Login"; // Trang đăng nhập nếu người dùng chưa đăng nhập
	options.LogoutPath = "/Authentication/Logout"; // Trang đăng xuất
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Thời gian tồn tại của cookie
})
.AddJwtBearer(options =>
{
	options.SaveToken = true;
	options.TokenValidationParameters = new TokenValidationParameters
	{
		ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],
		ValidAudience = builder.Configuration["Jwt:ValidAudience"],
		IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
		ClockSkew = new TimeSpan(0, 0, 5)
	};

});



//
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
    builder =>
    {
        builder.WithOrigins("https://localhost:7075")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

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
app.UseCors("AllowMVC");
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.UseAuthentication();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Authentication}/{action=Login}/{id?}");
});
app.Run();
