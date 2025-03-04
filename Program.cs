using Microsoft.EntityFrameworkCore;
using WiseTruck.API.Data;
using WiseTruck.API.Repositories;
using WiseTruck.API.Services;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "WiseTruck API",
        Version = "v1",
        Description = "API para gerenciamento de frota de caminhões",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipe WiseTruck",
            Email = "contato@wisetruck.com.br"
        }
    });
    
    c.EnableAnnotations();

    // Configuração correta para agrupamento
    c.TagActionsBy(api => 
    {
        if (api.GroupName != null)
        {
            return new[] { api.GroupName };
        }
        
        var controllerName = api.ActionDescriptor.RouteValues["controller"];
        return new[] { controllerName };
    });

    // Ordenação dos grupos
    c.OrderActionsBy(apiDesc => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}");
});

// Configuração do contexto do banco de dados
builder.Services.AddDbContext<WiseTruckContext>(options =>
{
    // Priorizar a string de conexão do ambiente (Render.com)
    var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
                          builder.Configuration.GetConnectionString("DefaultConnection");
    
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null);
    });

    if (builder.Environment.IsDevelopment())
    {
        options.LogTo(Console.WriteLine, LogLevel.Information);
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Registro dos repositórios
builder.Services.AddScoped<ICaminhaoRepository, CaminhaoRepository>();
builder.Services.AddScoped<IMotoristaRepository, MotoristaRepository>();
builder.Services.AddScoped<IViagemRepository, ViagemRepository>();
builder.Services.AddScoped<IPedagioRepository, PedagioRepository>();
builder.Services.AddScoped<IAbastecimentoRepository, AbastecimentoRepository>();

// Registro dos serviços
builder.Services.AddScoped<ICaminhaoService, CaminhaoService>();
builder.Services.AddScoped<IMotoristaService, MotoristaService>();
builder.Services.AddScoped<IViagemService, ViagemService>();
builder.Services.AddScoped<IPedagioService, PedagioService>();
builder.Services.AddScoped<IAbastecimentoService, AbastecimentoService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WiseTruck API V1");
        c.RoutePrefix = string.Empty; // Para acessar o Swagger na raiz
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
