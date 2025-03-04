using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyStore.Server.Models.DbEntity;
using MyStore.Server.Models.Factories.Implements;
using MyStore.Server.Models.Factories.Interfaces;
using MyStore.Server.Models.Repository.Implements;
using MyStore.Server.Models.Repository.Interfaces;
using MyStore.Server.Models.Service.Implements;
using MyStore.Server.Models.Service.Interfaces;
using MyStore.Server.Models.UnitOfWork;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//設置Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // 設定最低層級
    .WriteTo.Console()
    .WriteTo.File("logs/mystore.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

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
                .AddTransient<StripeService>()
                .AddTransient<StripeEmbeddedService>()
                .AddTransient<IPaymentFactory,PaymentFactory>()
                //.AddScoped<IStripeService, StripeService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddHttpClient<IRecaptchaService, RecaptchaService>();
                


//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//                .AddCookie();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = "https://localhost:7266/",
        //ValidAudience = "https://localhost:5173/",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySuperSecretKeyMySuperSecretKeyMySuperSecretKey"))
    };
});



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});


builder.Host.UseSerilog();
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

app.UseCors("AllowAll");

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
