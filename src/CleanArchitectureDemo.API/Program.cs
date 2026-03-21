using CleanArchitectureDemo.API.Middleware;
using CleanArchitectureDemo.Modules.Catalog.Infrastructure;
using CleanArchitectureDemo.Modules.Ordering.Infrastructure;
using CleanArchitectureDemo.Modules.Catalog.Infrastructure.Data;
using CleanArchitectureDemo.Modules.Ordering.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== Register Services =====
// เรียก AddCatalogModule() ซึ่งจะครอบคลุมทั้ง Application & Infrastructure ของตระกูล Catalog
builder.Services.AddCatalogModule(builder.Configuration);

// เรียก AddOrderingModule() เตรียมพร้อมสำหรับระบบสั่งซื้อที่แยกอิสระจาก Catalog
builder.Services.AddOrderingModule(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Clean Architecture Demo API",
        Version = "v1",
        Description = "ASP.NET Core API with Clean Architecture — Product Management"
    });
});

var app = builder.Build();

// ===== Seed Database =====
// จะถูกสร้างใหม่ด้วย EnsureCreated ถ้ายังไม่มีฐานข้อมูล (ทดแทน Migration ชั่วคราว)
using (var scope = app.Services.CreateScope())
{
    var catalogContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    catalogContext.Database.EnsureCreated();
    
    var orderingContext = scope.ServiceProvider.GetRequiredService<OrderingDbContext>();
    orderingContext.Database.EnsureCreated();
}

// ===== Middleware Pipeline =====
app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Clean Architecture Demo v1");
        options.RoutePrefix = string.Empty; // Swagger UI ที่ root URL
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
