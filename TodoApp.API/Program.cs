using Microsoft.EntityFrameworkCore;
using TodoApp.Services;
using TodoApp.Core.Interfaces;
using TodoApp.Repositories;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure DbContext
//builder.Services.AddDbContext<AppDbContext>(options =>
  //  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

      builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
  

// Register repositoriesFconfi
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

// Register services
builder.Services.AddScoped<IUserService, UserService>();

// Configure authentication (JWT Bearer)
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.ASCII.GetBytes("your_secret_key_here")) // Use the same key as in JwtHelper.
        };
    });

var app = builder.Build();

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

app.Run();