using Microsoft.EntityFrameworkCore;
using MovieApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Контролери Web API.
builder.Services.AddControllers();

// Swagger / OpenAPI (Swashbuckle) для перегляду та тестування ендпоінтів.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// EF Core з провайдером PostgreSQL (Npgsql). Рядок підключення — з appsettings.json.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? "Host=localhost;Port=5432;Database=cinema;Username=postgres;Password=postgres";
builder.Services.AddDbContext<MovieContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

// Застосування міграцій та наповнення БД демонстраційними даними.
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MovieContext>();
    context.Database.Migrate();
    DbSeeder.Seed(context);
}

// Swagger UI доступний за адресою /swagger (увімкнено завжди — зручно для захисту).
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Статичні файли (index.html + app.js) для фронтенду на JavaScript.
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
