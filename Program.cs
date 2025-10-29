using Microsoft.EntityFrameworkCore;
using SalesOrderSystem_BackEnd.Data;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// ... your middleware and endpoints

app.Run();
