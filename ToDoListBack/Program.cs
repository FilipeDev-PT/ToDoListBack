using Microsoft.EntityFrameworkCore;
using ToDoListBack.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection();


builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllers();

builder.Services.AddDbContext<TaskContext>(options =>
{
    options.UseNpgsql("Host=ep-odd-sun-a5kuegem-pooler.us-east-2.aws.neon.tech;Username=neondb_owner;Password=npg_X7TUBQJu1Krm;Database=neondb;SslMode=Require;");
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.UseRouting();
app.MapControllers();

app.Run();
