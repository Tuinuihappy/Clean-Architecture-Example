using CleanArchitectureDemo.API.Middleware;
using CleanArchitectureDemo.Infrastructure;
using CleanArchitectureDemo.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== Register Services =====
// เรียก AddInfrastructure() จาก Infrastructure layer
// API layer ไม่ต้องรู้ว่า register service อะไรบ้าง — มี extension method ห่อไว้ให้
builder.Services.AddInfrastructure();

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
// InMemory DB จะถูกสร้างใหม่ทุกครั้งที่ restart — ต้อง EnsureCreated เพื่อให้ seed data ทำงาน
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
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
