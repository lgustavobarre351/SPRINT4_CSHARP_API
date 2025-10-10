using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using ProjetoInvestimentos.Models;

namespace ProjetoInvestimentos.Swagger;

/// <summary>
/// Provedor de exemplos para documentação Swagger
/// </summary>
public class SwaggerExamplesFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(Investimento))
        {
            schema.Example = new Microsoft.OpenApi.Any.OpenApiObject
            {
                ["id"] = new Microsoft.OpenApi.Any.OpenApiString("00000000-0000-0000-0000-000000000000"),
                ["userCpf"] = new Microsoft.OpenApi.Any.OpenApiString("12345678901"),
                ["userId"] = new Microsoft.OpenApi.Any.OpenApiString("00000000-0000-0000-0000-000000000000"),
                ["tipo"] = new Microsoft.OpenApi.Any.OpenApiString("Ação"),
                ["codigo"] = new Microsoft.OpenApi.Any.OpenApiString("PETR4"),
                ["valor"] = new Microsoft.OpenApi.Any.OpenApiDouble(1000.50),
                ["operacao"] = new Microsoft.OpenApi.Any.OpenApiString("compra"),
                ["criadoEm"] = new Microsoft.OpenApi.Any.OpenApiString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss")),
                ["alteradoEm"] = new Microsoft.OpenApi.Any.OpenApiString(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"))
            };
        }
    }
}