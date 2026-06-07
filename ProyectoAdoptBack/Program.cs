using ProyectoAdoptBack.Services;
using ProyectoAdoptBack.Repositories;
using Dapper;
using System.Data;

SqlMapper.AddTypeHandler(new DateOnlyToDateTimeHandler());
SqlMapper.AddTypeHandler(new NullableDateOnlyToDateTimeHandler());

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();  
builder.Services.AddOpenApi();

//Administrador
builder.Services.AddScoped<IAdministradorRepository, AdministradorRepository>();
builder.Services.AddScoped<IAdministradorService, AdministradorService>();
//Especie de la chida
builder.Services.AddScoped<IEspecieRepository, EspecieRepository>();
builder.Services.AddScoped<IEspecieService, EspecieService>();
//Perfil adoptante (Informacion del mismo)
builder.Services.AddScoped<IPerfilAdoptanteRepository, PerfilAdoptanteRepository>();
builder.Services.AddScoped<IPerfilAdoptanteService, PerfilAdoptanteService>();
//Refugio Administradores 
builder.Services.AddScoped<IRefugioAdministradoresRepository, RefugioAdministradoresRepository>();
builder.Services.AddScoped<IRefugioAdministradoresService, RefugioAdministradoresService>();
//Refugio
builder.Services.AddScoped<IRefugioRepository, RefugioRepository>();
builder.Services.AddScoped<IRefugioService, RefugioService>();
//Usuario
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
//Animales
builder.Services.AddScoped<IAnimalesRepository, AnimalesRepository>();
builder.Services.AddScoped<IAnimalesService, AnimalesService>();
//solicitudAdopcion
builder.Services.AddScoped<ISolicitudAdopcionRepository, SolicitudAdopcionRepository>();
builder.Services.AddScoped<ISolicitudAdopcionService, SolicitudAdopcionService>();
//solicitudanimales
builder.Services.AddScoped<ISolicitudAnimalesRepository, SolicitudAnimalesRepository>();
builder.Services.AddScoped<ISolicitudAnimalesService, SolicitudAnimalesService>();
//imagen
builder.Services.AddScoped<IImagenRepository, ImagenRepository>();
builder.Services.AddScoped<IImagenService, ImagenService>();
//raza
builder.Services.AddScoped<IRazaRepository, RazaRepository>();
builder.Services.AddScoped<IRazaService, RazaService>();
//citas 
builder.Services.AddScoped<ICitasRepository, CitasRepository>();
builder.Services.AddScoped<ICitasService, CitasService>();
//adoptante
builder.Services.AddScoped<IAdoptanteRepository, AdoptanteRepository>();
builder.Services.AddScoped<IAdoptanteService, AdoptanteService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();

public class DateOnlyToDateTimeHandler : SqlMapper.TypeHandler<DateTime>
{
    public override DateTime Parse(object value)
    {
        if (value is DateOnly d) return d.ToDateTime(TimeOnly.MinValue);
        return (DateTime)value;
    }

    public override void SetValue(IDbDataParameter parameter, DateTime value)
        => parameter.Value = value;
}

public class NullableDateOnlyToDateTimeHandler : SqlMapper.TypeHandler<DateTime?>
{
    public override DateTime? Parse(object value)
    {
        if (value is DateOnly d) return d.ToDateTime(TimeOnly.MinValue);
        return (DateTime?)value;
    }

    public override void SetValue(IDbDataParameter parameter, DateTime? value)
        => parameter.Value = value;
}