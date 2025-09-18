using MongoDB.Driver;
using StudentManagement.Infrastructure;
using StudentManagement.Models;
using StudentManagement.Services;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IStudentStoreDatabaseSettings>(_ =>
    builder.Configuration.GetSection(nameof(StudentStoreDatabaseSettings))
        .Get<StudentStoreDatabaseSettings>() ?? new StudentStoreDatabaseSettings());

builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var cfg = sp.GetRequiredService<IStudentStoreDatabaseSettings>();
    var cs = builder.Configuration.GetConnectionString("myFirstDatabase")
             ?? cfg.ConnectionString
             ?? "mongodb://admin:0WyJzfr7wUtwcpYTpUd2y4@localhost:27017/myFirstDatabase?authSource=admin";
    return new MongoClient(cs);
});
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddHostedService<StudentDataSeeder>();
builder.Services.AddSingleton<StudentSeeder>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<StudentSeeder>();
    await seeder.SeedAsync();
}

app.MapDefaultEndpoints();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    // UI at / swagger
    c.RoutePrefix = "swagger";
    // Point to Swashbuckle’s JSON (always available when UseSwagger() is enabled)
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "StudentManagement API v1");
});
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseAuthorization();
app.MapControllers();
app.UseCors();
app.Run();
