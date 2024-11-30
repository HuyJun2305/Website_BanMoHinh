using View.Handlers;
using View.IServices;
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
builder.Services.AddHttpClient<IBrandServices, BrandServices>();
builder.Services.AddHttpClient<ISizeServices, SizeServices>();
builder.Services.AddHttpClient<IProductServices, ProductServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddHttpClient<IAuthenticationService, AuthenticationService>();
builder.Services.AddHttpClient<IImageServices, ImageServices>();
builder.Services.AddHttpClient<IMaterialServices, MaterialServices>();
builder.Services.AddHttpClient<ICartServices, CartServices>();

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
        pattern: "{controller=HomeCustomer}/{action=ViewProducts}/{id?}");
});
app.Run();
