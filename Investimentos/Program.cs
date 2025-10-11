using Microsoft.EntityFrameworkCore;
using ProjetoInvestimentos.Data;
using ProjetoInvestimentos.Repositories;
using ProjetoInvestimentos.Services;
using ProjetoInvestimentos.Swagger;

var builder = WebApplication.CreateBuilder(args);

// --- Services Configuration ---
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Investimentos API",
        Version = "v1",
        Description = "API completa para gerenciamento de investimentos com CRUD, consultas LINQ e integração com APIs externas",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Equipe Challenge XP",
            Email = "contato@challengexp.com"
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        }
    });

    // Incluir comentários XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }

    // Configurar exemplos
    c.EnableAnnotations();
    c.SchemaFilter<SwaggerExamplesFilter>();
    
    // Configurar tags
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
});

// --- Entity Framework ---
string connectionString = "User Id=postgres.oziwendirtmqquvqkree;Password=vqUfvs3b3RgDrEpX;Server=aws-0-us-east-2.pooler.supabase.com;Port=6543;Database=postgres";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

// --- Repository ---
builder.Services.AddScoped<IInvestimentoRepository, EfInvestimentoRepository>();

// --- Serviços ---
builder.Services.AddSingleton<IB3ValidationService, B3ValidationService>();

// --- HttpClient para APIs externas ---
builder.Services.AddHttpClient();

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// --- Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investimentos API v1");
        c.RoutePrefix = "swagger";
        c.DocumentTitle = "Investimentos API - Documentação";
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        c.DefaultModelsExpandDepth(-1);
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
        c.EnableValidator();
    });
}

app.UseCors();
app.UseRouting();
app.MapControllers();

// --- Redirecionamento para Swagger ---
app.MapGet("/", () => Results.Redirect("/swagger"));

// Configuração de porta para produção
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Run($"http://0.0.0.0:{port}");