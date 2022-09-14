using Microsoft.EntityFrameworkCore;
using RocketElevators.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddDbContext<RocketElevatorsContext>(options => {
    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

    Console.WriteLine("----------------------");
    Console.WriteLine("connectionString:" + connectionString);
    Console.WriteLine("----------------------");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});
//builder.Services.AddSwaggerGen(c =>
//{
//    c.SwaggerDoc("v1", new() { Title = "TodoApi", Version = "v1" });
//});
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
