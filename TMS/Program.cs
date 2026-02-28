using Microsoft.EntityFrameworkCore;
using Repository.Models;
using Service;
using Service.Services;
using TMS.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TMSContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContextService, UserContextService>();
// Add services to the container.
//builder.Services.AddScoped<typeof(IGenericRepository),typeof(GenericRepository<>) > ();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//builder.Services.AddScoped(IAuthService, AuthService);
builder.Services.AddScoped<IAuthService, AuthService>();
//builder.Services.AddScoped(IAuthservice)
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
