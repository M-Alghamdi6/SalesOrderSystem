using Microsoft.Data.SqlClient;
using Microsoft.OpenApi.Models;
using SalesOrderSystem_BackEnd.Profiles;
using SalesOrderSystem_BackEnd.Services;
using SalesOrderSystem_BackEnd.Middleware;
using SalesOrderSystem_BackEnd.Repository;

var builder = WebApplication.CreateBuilder(args);

// ------------------- Services -------------------

// Add controllers
builder.Services.AddControllers();

// Swagger documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Sales Order System API",
        Version = "v1",
        Description = "API documentation for Sales Order System."
    });
});

// HttpContext accessor (needed for session)
builder.Services.AddHttpContextAccessor();

// Enable session support
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

// ------------------- Database -------------------

// Register SQL connection
builder.Services.AddTransient<SqlConnection>(_ =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

// ------------------- AutoMapper -------------------
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

// ------------------- Repositories & Services -------------------

// Mapping service
builder.Services.AddScoped<IMappingService, MappingService>();

// Users repository
builder.Services.AddScoped<IUsersRepository, UsersRepository>(sp =>
{
    var sqlConn = sp.GetRequiredService<SqlConnection>();
    var mapping = sp.GetRequiredService<IMappingService>();
    return new UsersRepository(sqlConn, mapping);
});

// SalesRequest service
builder.Services.AddScoped<ISalesRequestService, SalesRequestService>(sp =>
{
    var sqlConn = sp.GetRequiredService<SqlConnection>();
    var mapping = sp.GetRequiredService<IMappingService>();
    return new SalesRequestService(sqlConn, mapping);
});

// SalesRequestLine service
builder.Services.AddScoped<ISalesRequestLineService, SalesRequestLineService>(sp =>
{
    var sqlConn = sp.GetRequiredService<SqlConnection>();
    var mapping = sp.GetRequiredService<IMappingService>();
    return new SalesRequestLineService(sqlConn, mapping);
});

// ------------------- CORS -------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
        builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

// ------------------- Build app -------------------
var app = builder.Build();

// ------------------- Middleware -------------------
app.UseRouting();

app.UseCors("AllowAngularApp");

app.UseSession();

app.UseStaticFiles();
app.UseHttpsRedirection();

// Global exception handling
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
