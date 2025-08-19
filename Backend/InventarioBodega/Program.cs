using InventarioBackend.Data;
using InventarioBackend.Models; // Asegúrate de incluir este using si usas modelos en Program.cs, aunque no es común
using InventarioBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuración de CORS (Cross-Origin Resource Sharing)
// Esto permite que tu frontend (ej. Angular en localhost:5000 o 4200)
// pueda hacer peticiones a tu backend (ej. en localhost:5244)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            // Permite solicitudes desde localhost:5000 (donde podría estar tu Angular)
            // y localhost:5244 (donde se ejecuta tu backend con Swagger)
            // Puedes añadir más orígenes si tu frontend se ejecuta en otros puertos o dominios
            builder.WithOrigins("http://localhost:5000", "http://localhost:4200", "http://localhost:5244")
                   .AllowAnyHeader() // Permite cualquier encabezado en la solicitud
                   .AllowAnyMethod() // Permite cualquier método HTTP (GET, POST, PUT, DELETE)
                   .AllowCredentials(); // Permite el envío de cookies o encabezados de autorización (si se usan)
        });
});

// Añade los servicios para los controladores de la API
builder.Services.AddControllers();

// Configuración de Swagger/OpenAPI para la documentación y prueba de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext con SQL Server
// Obtiene la cadena de conexión "DefaultConnection" del archivo appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<InventarioService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
// Habilita Swagger UI solo en el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Comentado para deshabilitar la redirección HTTPS en desarrollo
// Esto evita el error "Failed to determine the https port for redirect"
// app.UseHttpsRedirection();

// Usar la política CORS definida. Debe ir antes de UseAuthorization() y MapControllers()
app.UseCors("AllowAngularApp");

// Comentado para deshabilitar la autorización temporalmente en desarrollo
// Esto resuelve el error 401 (Unauthorized) que estabas experimentando
// app.UseAuthorization();

// Mapea los controladores de la API a las rutas URL
app.MapControllers();

// Inicia la aplicación
app.Run();
