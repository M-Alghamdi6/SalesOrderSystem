using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using SalesOrderSystem_BackEnd.Profiles;
using SalesOrderSystem_BackEnd.Services;
using SalesOrderSystem_BackEnd.Middleware;
using SalesOrderSystem_BackEnd.Repository;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Services -------------------
builder.Services.AddControllers();

// ✅ Add Swagger documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sales Order System API",
        Version = "v1",
        Description = "API documentation for Sales Order System."
    });
});

// Add HttpContext accessor (needed for session)
builder.Services.AddHttpContextAccessor();

//  Enable session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Needed for SameSite rules
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

// Database connection
builder.Services.AddTransient<SqlConnection>(_ =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// Repositories and Services
builder.Services.AddScoped<IMappingService, MappingService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>(sp =>
{
    var sqlConn = sp.GetRequiredService<SqlConnection>();
    var mapping = sp.GetRequiredService<IMappingService>();
    return new UsersRepository(sqlConn, mapping);
});

builder.Services.AddScoped<ISalesRequestService, SalesRequestService>(sp =>
{
    var sqlConn = sp.GetRequiredService<SqlConnection>();
    var mapping = sp.GetRequiredService<IMappingService>();
    return new SalesRequestService(sqlConn, mapping);
});


// CORS for Angular frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
        builder
            .WithOrigins("http://localhost:4200") // Angular dev server
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// ------------------- Build app -------------------
var app = builder.Build();

// ------------------- Middleware -------------------
app.UseRouting();

//  Use CORS before session/auth
app.UseCors("AllowAngularApp");

//  Session before endpoints
app.UseSession();

app.UseStaticFiles();
app.UseHttpsRedirection();

//  Global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// ------------------- Swagger -------------------
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sales Order System API V1");
    c.RoutePrefix = string.Empty;
});

// ------------------- Map Controllers -------------------
app.MapControllers();

app.Run();
