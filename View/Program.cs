using Data.Models;
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

// Cấu hình Session
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian hết hạn session
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

// Đăng ký các services
builder.Services.AddTransient<AuthenticationHandler>();

builder.Services.AddHttpClient("ServerApi")
	.ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7280" ?? ""))
	.AddHttpMessageHandler<AuthenticationHandler>();

builder.Services.AddAuthorizationCore();

// Các dịch vụ cho ứng dụng của bạn
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

// Các dịch vụ liên quan đến order
builder.Services.AddHttpClient<IOrderServices, OrderServices>();
builder.Services.AddHttpClient<IOrderDetailServices, OrderDetailServices>();

// Cấu hình JWT
var secret = builder.Configuration["JWT:Secret"] ?? throw new InvalidOperationException("Khóa bí mật chưa đc tạo");
builder.Services.AddAuthentication(options =>
{
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
		ClockSkew = TimeSpan.Zero
	};
});

// Cấu hình CORS
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll", policy =>
	{
		policy.WithOrigins("https://localhost:7221") // Thay bằng domain Frontend của bạn
			  .AllowAnyHeader()
			  .AllowAnyMethod()
			  .AllowCredentials(); // Cho phép gửi cookie/session
	});
});

var app = builder.Build();

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts(); // HSTS cho môi trường sản xuất
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// Cấu hình CORS
app.UseCors("AllowAll");

app.UseRouting();

// Các middleware bảo mật
app.UseSession(); // Đảm bảo session hoạt động trước authorization
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapControllerRoute(
		name: "default",
		pattern: "{controller=Authentication}/{action=Login}/{id?}");
});

app.Run();
