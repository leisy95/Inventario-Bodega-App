using InventarioBackend.Data;
using InventarioBackend.Models; // Aseg�rate de incluir este using si usas modelos en Program.cs, aunque no es com�n
using InventarioBackend.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configuraci�n de CORS (Cross-Origin Resource Sharing)
// Esto permite que tu frontend (ej. Angular en localhost:5000 o 4200)
// pueda hacer peticiones a tu backend (ej. en localhost:5244)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            // Permite solicitudes desde localhost:5000 (donde podr�a estar tu Angular)
            // y localhost:5244 (donde se ejecuta tu backend con Swagger)
            // Puedes a�adir m�s or�genes si tu frontend se ejecuta en otros puertos o dominios
            builder.WithOrigins("http://localhost:5000", "http://localhost:4200", "http://localhost:5244")
                   .AllowAnyHeader() // Permite cualquier encabezado en la solicitud
                   .AllowAnyMethod() // Permite cualquier m�todo HTTP (GET, POST, PUT, DELETE)
                   .AllowCredentials(); // Permite el env�o de cookies o encabezados de autorizaci�n (si se usan)
        });
});

// A�ade los servicios para los controladores de la API
builder.Services.AddControllers();

// Configuraci�n de Swagger/OpenAPI para la documentaci�n y prueba de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar DbContext con SQL Server
// Obtiene la cadena de conexi�n "DefaultConnection" del archivo appsettings.json
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

// Comentado para deshabilitar la redirecci�n HTTPS en desarrollo
// Esto evita el error "Failed to determine the https port for redirect"
// app.UseHttpsRedirection();

// Usar la pol�tica CORS definida. Debe ir antes de UseAuthorization() y MapControllers()
app.UseCors("AllowAngularApp");

// Comentado para deshabilitar la autorizaci�n temporalmente en desarrollo
// Esto resuelve el error 401 (Unauthorized) que estabas experimentando
// app.UseAuthorization();

// Mapea los controladores de la API a las rutas URL
app.MapControllers();

// Inicia la aplicaci�n
app.Run();
