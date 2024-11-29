using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using View.Handlers;
using View.IServices;
using View.Servicecs;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
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
builder.Services.AddHttpClient<IBrandServices, BrandServices>();
builder.Services.AddHttpClient<ISizeServices, SizeServices>();
builder.Services.AddHttpClient<IProductServices, ProductServices>();
builder.Services.AddHttpClient<IUserServices, UserServices>();
builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient<IImageServices, ImageServices>();
builder.Services.AddHttpClient<IMaterialServices, MaterialServices>();
builder.Services.AddHttpContextAccessor();
//




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
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
