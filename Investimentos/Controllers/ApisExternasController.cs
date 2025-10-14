using Microsoft.AspNetCore.Mvc;
using ProjetoInvestimentos.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ProjetoInvestimentos.Controllers;

/// <summary>
/// APIs externas e validações
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("3️⃣ APIS EXTERNAS - B3, HG Brasil e validações externas")]
public class ApisExternasController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IB3ValidationService _b3ValidationService;

    public ApisExternasController(HttpClient httpClient, IB3ValidationService b3ValidationService)
    {
        _httpClient = httpClient;
        _b3ValidationService = b3ValidationService;
    }

    /// <summary>
    /// Lista códigos de ações da B3
    /// </summary>
    /// <returns>Lista de códigos B3</returns>
    [HttpGet("codigos-b3")]
    [SwaggerOperation(
        Summary = "Lista códigos de ações da B3",
        Description = "Retorna todos os códigos de ações da B3 carregados do arquivo CSV"
    )]
    [SwaggerResponse(200, "Lista de códigos da B3", typeof(object))]
    public async Task<ActionResult<object>> GetB3Codes()
    {
        try
        {
            var codes = await _b3ValidationService.GetAllB3CodesAsync();
            return Ok(new
            {
                Success = true,
                Message = "Códigos B3 carregados com sucesso",
                Data = new
                {
                    TotalCodes = codes.Count(),
                    Codes = codes.Take(50), // Primeiros 50 para exemplo
                    AllCodes = codes
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Erro = "Erro ao obter códigos B3",
                Detalhes = ex.Message
            });
        }
    }

    /// <summary>
    /// Recarrega códigos da B3
    /// </summary>
    /// <returns>Status da operação</returns>
    [HttpPost("recarregar-b3")]
    [SwaggerOperation(
        Summary = "Recarrega códigos da B3",
        Description = "Força o recarregamento da lista de códigos da B3"
    )]
    public async Task<ActionResult<object>> ReloadB3Codes()
    {
        try
        {
            await _b3ValidationService.ReloadAsync();
            var codes = await _b3ValidationService.GetAllB3CodesAsync();
            
            return Ok(new
            {
                Success = true,
                Message = "Códigos B3 recarregados com sucesso",
                TotalCodes = codes.Count()
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Erro = "Erro ao recarregar códigos B3",
                Detalhes = ex.Message
            });
        }
    }

    /// <summary>
    /// Consulta cotação de uma ação
    /// </summary>
    /// <param name="symbol">Código da ação</param>
    /// <returns>Cotação atual</returns>
    [HttpGet("cotacao/{symbol}")]
    [SwaggerOperation(
        Summary = "Cotação atual de ação",
        Description = "Consulta a cotação atual de uma ação específica"
    )]
    [SwaggerResponse(200, "Cotação obtida com sucesso", typeof(object))]
    [SwaggerResponse(400, "Erro na consulta da cotação")]
    [SwaggerResponse(404, "Ação não encontrada")]
    public async Task<ActionResult<object>> GetStockQuote([Required] string symbol)
    {
        try
        {
            // Validar se o código existe na B3
            var isValid = await _b3ValidationService.IsB3StockAsync(symbol.ToUpper());
            if (!isValid)
            {
                return NotFound(new
                {
                    Success = false,
                    Erro = "Código de ação não encontrado na B3",
                    Symbol = symbol.ToUpper(),
                    Message = "Verifique se o código está correto usando /api/ApisExternas/codigos-b3"
                });
            }

            // Consultar cotação na HG Brasil
            var response = await _httpClient.GetAsync($"https://api.hgbrasil.com/finance/stock_price?key=YOUR_API_KEY&symbol={symbol.ToUpper()}");
            
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest(new
                {
                    Success = false,
                    Erro = "Erro ao consultar cotação externa",
                    StatusCode = response.StatusCode
                });
            }

            var content = await response.Content.ReadAsStringAsync();
            var quotationData = JsonSerializer.Deserialize<object>(content);

            return Ok(new
            {
                Success = true,
                Symbol = symbol.ToUpper(),
                Message = "Cotação obtida com sucesso",
                Data = quotationData,
                Timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Erro = "Erro ao obter cotação",
                Symbol = symbol,
                Detalhes = ex.Message
            });
        }
    }

    /// <summary>
    /// Valida código de ação
    /// </summary>
    /// <param name="symbol">Código para validar</param>
    /// <returns>Resultado da validação</returns>
    [HttpGet("validar-codigo/{symbol}")]
    [SwaggerOperation(
        Summary = "Validar código de ação B3",
        Description = "Verifica se um código de ação é válido na B3"
    )]
    public async Task<ActionResult<object>> ValidateB3Code([Required] string symbol)
    {
        try
        {
            var isValid = await _b3ValidationService.IsB3StockAsync(symbol.ToUpper());
            
            return Ok(new
            {
                Success = true,
                Symbol = symbol.ToUpper(),
                IsValid = isValid,
                Message = isValid ? "Código válido na B3" : "Código não encontrado na B3"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Success = false,
                Erro = "Erro ao validar código",
                Symbol = symbol,
                Detalhes = ex.Message
            });
        }
    }
}