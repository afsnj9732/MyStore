using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Repository.Implements;
using MyStore.Server.Models.Repository.Interfaces;
using MyStore.Server.Models.Service.Implements;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DbStoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IMemberRepository, MemberRepository>()
                .AddScoped<IMemberService, MemberService>()
                .AddScoped<ICartRepository, CartRepository>()
                .AddScoped<ICartService, CartService>()
                .AddScoped<IOrderRepository, OrderRepository>()
                .AddScoped<IOrderService, OrderService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IStripeService, StripeService>()
                .AddHttpClient<IRecaptchaService, RecaptchaService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
