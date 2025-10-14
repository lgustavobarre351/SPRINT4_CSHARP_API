using Microsoft.AspNetCore.Mvc;
using ProjetoInvestimentos.Models;
using ProjetoInvestimentos.Repositories;
using ProjetoInvestimentos.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ProjetoInvestimentos.Controllers;

/// <summary>
/// Controller de investimentos
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("2️⃣ INVESTIMENTOS DA BASE - CRUD completo com consultas LINQ avançadas")]
public class InvestimentosController : ControllerBase
{
    private readonly IInvestimentoRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly IB3ValidationService _b3ValidationService;

    public InvestimentosController(IInvestimentoRepository repository, HttpClient httpClient, IB3ValidationService b3ValidationService)
    {
        _repository = repository;
        _httpClient = httpClient;
        _b3ValidationService = b3ValidationService;
    }

    // CRUD completo (35%)
    
    /// <summary>
    /// Obtém todos os investimentos
    /// </summary>
    /// <returns>Lista de todos os investimentos ordenados por data de criação</returns>
    /// <response code="200">Retorna a lista de investimentos</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista todos os investimentos",
        Description = "Retorna uma lista completa de todos os investimentos cadastrados, ordenados por data de criação (mais recentes primeiro)"
    )]
    [SwaggerResponse(200, "Lista de investimentos retornada com sucesso", typeof(IEnumerable<Investimento>))]
    public async Task<ActionResult<IEnumerable<Investimento>>> GetAll()
    {
        var investimentos = await _repository.GetAllAsync();
        return Ok(investimentos);
    }

    /// <summary>
    /// Obtém um investimento específico pelo ID
    /// </summary>
    /// <param name="id">ID único do investimento</param>
    /// <returns>Dados do investimento</returns>
    /// <response code="200">Investimento encontrado</response>
    /// <response code="404">Investimento não encontrado</response>
    [HttpGet("{id:guid}")]
    [SwaggerOperation(
        Summary = "Busca investimento por ID",
        Description = "Retorna os dados de um investimento específico baseado no seu ID único"
    )]
    [SwaggerResponse(200, "Investimento encontrado", typeof(Investimento))]
    [SwaggerResponse(404, "Investimento não encontrado")]
    public async Task<ActionResult<Investimento>> GetById(
        [FromRoute, SwaggerParameter("ID único do investimento", Required = true)] Guid id)
    {
        var investimento = await _repository.GetByIdAsync(id);
        if (investimento == null)
            return NotFound();
        
        return Ok(investimento);
    }

    /// <summary>
    /// Obtém todos os investimentos de um usuário específico
    /// </summary>
    /// <param name="userCpf">CPF do usuário (apenas números, 11 dígitos)</param>
    /// <returns>Lista de investimentos do usuário</returns>
    /// <response code="200">Lista de investimentos do usuário</response>
    [HttpGet("usuario/{userCpf}")]
    [SwaggerOperation(
        Summary = "Lista investimentos por CPF do usuário",
        Description = "Retorna todos os investimentos de um usuário. Digite apenas os números do CPF, sem pontos ou traços. Exemplo: 12345678901"
    )]
    [SwaggerResponse(200, "Lista de investimentos do usuário", typeof(IEnumerable<Investimento>))]
    public async Task<ActionResult<IEnumerable<Investimento>>> GetByUserCpf(
        [FromRoute, SwaggerParameter("CPF do usuário (apenas números, exemplo: 12345678901)", Required = true)] string userCpf)
    {
        var investimentos = await _repository.GetByUserCpfAsync(userCpf);
        return Ok(investimentos);
    }

    /// <summary>
    /// Cria um novo investimento
    /// </summary>
    /// <param name="investimento">Dados do investimento a ser criado</param>
    /// <returns>Investimento criado com ID gerado</returns>
    /// <response code="201">Investimento criado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Cria um novo investimento",
        Description = "Cadastra um novo investimento. Campo obrigatórios: userCpf (11 números), tipo, codigo, valor, operacao (compra/venda). O ID será gerado automaticamente."
    )]
    [SwaggerResponse(201, "Investimento criado com sucesso", typeof(Investimento))]
    [SwaggerResponse(400, "Dados de entrada inválidos")]
    public async Task<ActionResult<Investimento>> Create(
        [FromBody, SwaggerRequestBody("Dados do investimento", Required = true)] Investimento investimento)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var created = await _repository.CreateAsync(investimento);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>
    /// Atualiza um investimento existente
    /// </summary>
    /// <param name="id">ID do investimento a ser atualizado</param>
    /// <param name="investimento">Novos dados do investimento</param>
    /// <returns>Investimento atualizado</returns>
    /// <response code="200">Investimento atualizado com sucesso</response>
    /// <response code="400">ID não confere ou dados inválidos</response>
    /// <response code="404">Investimento não encontrado</response>
    [HttpPut("{id:guid}")]
    [SwaggerOperation(
        Summary = "Atualiza um investimento",
        Description = "Modifica os dados de um investimento existente. O ID deve ser fornecido na URL e no corpo da requisição."
    )]
    [SwaggerResponse(200, "Investimento atualizado com sucesso", typeof(Investimento))]
    [SwaggerResponse(400, "ID não confere ou dados inválidos")]
    [SwaggerResponse(404, "Investimento não encontrado")]
    public async Task<ActionResult<Investimento>> Update(
        [FromRoute, SwaggerParameter("ID do investimento", Required = true)] Guid id, 
        [FromBody, SwaggerRequestBody("Dados atualizados do investimento", Required = true)] Investimento investimento)
    {
        if (id != investimento.Id)
            return BadRequest("ID mismatch");

        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        var updated = await _repository.UpdateAsync(investimento);
        return Ok(updated);
    }

    /// <summary>
    /// Remove um investimento
    /// </summary>
    /// <param name="id">ID do investimento a ser removido</param>
    /// <returns>Confirmação de remoção</returns>
    /// <response code="204">Investimento removido com sucesso</response>
    /// <response code="404">Investimento não encontrado</response>
    [HttpDelete("{id:guid}")]
    [SwaggerOperation(
        Summary = "Remove um investimento",
        Description = "Exclui permanentemente um investimento do sistema"
    )]
    [SwaggerResponse(204, "Investimento removido com sucesso")]
    [SwaggerResponse(404, "Investimento não encontrado")]
    public async Task<ActionResult> Delete(
        [FromRoute, SwaggerParameter("ID do investimento a ser removido", Required = true)] Guid id)
    {
        var existing = await _repository.GetByIdAsync(id);
        if (existing == null)
            return NotFound();

        await _repository.DeleteAsync(id);
        return NoContent();
    }

    // Pesquisas com LINQ (10%)
    
    /// <summary>
    /// Filtra investimentos por tipo
    /// </summary>
    /// <param name="tipo">Tipo de investimento (ex: Ação, CDB, Tesouro)</param>
    /// <returns>Lista de investimentos do tipo especificado</returns>
    /// <response code="200">Lista de investimentos filtrada por tipo</response>
    [HttpGet("tipo/{tipo}")]
    [SwaggerOperation(
        Summary = "Filtra investimentos por tipo",
        Description = "Retorna todos os investimentos de um tipo específico usando consulta LINQ otimizada"
    )]
    [SwaggerResponse(200, "Investimentos filtrados por tipo", typeof(IEnumerable<Investimento>))]
    public async Task<ActionResult<IEnumerable<Investimento>>> GetByTipo(
        [FromRoute, SwaggerParameter("Tipo de investimento (Ação, CDB, Tesouro, etc.)", Required = true)] string tipo)
    {
        var investimentos = await _repository.GetByTipoAsync(tipo);
        return Ok(investimentos);
    }

    /// <summary>
    /// Filtra investimentos por operação
    /// </summary>
    /// <param name="operacao">Tipo de operação (compra ou venda)</param>
    /// <returns>Lista de investimentos da operação especificada</returns>
    /// <response code="200">Lista de investimentos filtrada por operação</response>
    [HttpGet("operacao/{operacao}")]
    [SwaggerOperation(
        Summary = "Filtra investimentos por operação",
        Description = "Retorna todos os investimentos de uma operação específica (compra/venda) usando LINQ"
    )]
    [SwaggerResponse(200, "Investimentos filtrados por operação", typeof(IEnumerable<Investimento>))]
    public async Task<ActionResult<IEnumerable<Investimento>>> GetByOperacao(
        [FromRoute, SwaggerParameter("Tipo de operação: 'compra' ou 'venda'", Required = true)] string operacao)
    {
        var investimentos = await _repository.GetByOperacaoAsync(operacao);
        return Ok(investimentos);
    }

    /// <summary>
    /// Calcula o valor total investido por um usuário
    /// </summary>
    /// <param name="userCpf">CPF do usuário</param>
    /// <returns>Valor total considerando compras (+) e vendas (-)</returns>
    /// <response code="200">Valor total calculado</response>
    [HttpGet("total-valor/{userCpf}")]
    [SwaggerOperation(
        Summary = "Calcula valor total por usuário",
        Description = "Usa LINQ para somar valores de compras e subtrair vendas, retornando o valor líquido investido"
    )]
    [SwaggerResponse(200, "Valor total calculado", typeof(object))]
    public async Task<ActionResult<decimal>> GetTotalValueByUser(
        [FromRoute, SwaggerParameter("CPF do usuário", Required = true)] string userCpf)
    {
        var total = await _repository.GetTotalValueByUserAsync(userCpf);
        return Ok(new { UserCpf = userCpf, ValorTotal = total });
    }

    /// <summary>
    /// Obtém investimentos recentes
    /// </summary>
    /// <param name="days">Número de dias para considerar como recente (padrão: 30)</param>
    /// <returns>Lista de investimentos dos últimos N dias</returns>
    /// <response code="200">Investimentos recentes</response>
    [HttpGet("recentes")]
    [SwaggerOperation(
        Summary = "Lista investimentos recentes",
        Description = "Retorna investimentos criados nos últimos N dias usando consulta LINQ com filtro de data"
    )]
    [SwaggerResponse(200, "Lista de investimentos recentes", typeof(IEnumerable<Investimento>))]
    public async Task<ActionResult<IEnumerable<Investimento>>> GetRecentInvestments(
        [FromQuery, SwaggerParameter("Número de dias (padrão: 30)")] int days = 30)
    {
        var investimentos = await _repository.GetRecentInvestmentsAsync(days);
        return Ok(investimentos);
    }

    /// <summary>
    /// Obtém resumo de investimentos agrupados por tipo
    /// </summary>
    /// <returns>Estatísticas agrupadas por tipo de investimento</returns>
    /// <response code="200">Resumo estatístico por tipo</response>
    [HttpGet("resumo-por-tipo")]
    [SwaggerOperation(
        Summary = "Resumo estatístico por tipo",
        Description = "Usa LINQ GroupBy para agregar dados: total de investimentos, valor total e valor médio por tipo"
    )]
    [SwaggerResponse(200, "Resumo estatístico agrupado", typeof(IEnumerable<object>))]
    public async Task<ActionResult<IEnumerable<object>>> GetInvestmentSummaryByType()
    {
        var summary = await _repository.GetInvestmentSummaryByTypeAsync();
        return Ok(summary);
    }

    /// <summary>
    /// Lista todos os CPFs únicos presentes na base de dados
    /// </summary>
    /// <returns>Lista de CPFs únicos ordenados</returns>
    /// <response code="200">Lista de CPFs únicos</response>
    [HttpGet("usuarios/cpfs")]
    [SwaggerOperation(
        Summary = "Lista todos os CPFs cadastrados",
        Description = "Usa LINQ Distinct para retornar todos os CPFs únicos presentes na base de dados, ordenados alfabeticamente"
    )]
    [SwaggerResponse(200, "Lista de CPFs únicos", typeof(IEnumerable<string>))]
    public async Task<ActionResult<IEnumerable<string>>> GetAllUserCpfs()
    {
        var cpfs = await _repository.GetAllUserCpfsAsync();
        return Ok(cpfs);
    }

    // APIs para ações brasileiras e outros serviços
    
    /// <summary>
    /// Lista todos os códigos de ações da B3 carregados do CSV
    /// </summary>
    /// <returns>Lista de todos os códigos válidos da B3</returns>
    /// <response code="200">Lista de códigos da B3</response>
    [HttpGet("codigos-b3")]
    [SwaggerOperation(
        Summary = "Lista códigos de ações da B3",
        Description = "Retorna todos os códigos de ações da B3 carregados do arquivo CSV, útil para validação e consulta"
    )]
    [SwaggerResponse(200, "Lista de códigos da B3", typeof(object))]
    public async Task<ActionResult<object>> GetB3Codes()
    {
        try
        {
            var codes = await _b3ValidationService.GetAllB3CodesAsync();
            return Ok(new
            {
                TotalCodigos = codes.Count,
                Codigos = codes.OrderBy(c => c).ToList(),
                CarregadoEm = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Erro = "Erro ao obter códigos B3",
                Mensagem = ex.Message
            });
        }
    }

    /// <summary>
    /// Recarrega a lista de códigos da B3 do arquivo CSV
    /// </summary>
    /// <returns>Confirmação de recarregamento</returns>
    /// <response code="200">Lista recarregada com sucesso</response>
    [HttpPost("recarregar-b3")]
    [SwaggerOperation(
        Summary = "Recarrega códigos da B3",
        Description = "Força o recarregamento da lista de códigos da B3 a partir do arquivo CSV"
    )]
    [SwaggerResponse(200, "Lista recarregada com sucesso", typeof(object))]
    public async Task<ActionResult<object>> ReloadB3Codes()
    {
        try
        {
            await _b3ValidationService.ReloadAsync();
            var codes = await _b3ValidationService.GetAllB3CodesAsync();
            
            return Ok(new
            {
                Mensagem = "Lista de códigos B3 recarregada com sucesso",
                TotalCodigos = codes.Count,
                RecarregadoEm = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Erro = "Erro ao recarregar códigos B3",
                Mensagem = ex.Message
            });
        }
    }

    /// <summary>
    /// Consulta dados de endereço por CEP
    /// </summary>
    /// <param name="cep">CEP no formato 12345678 ou 12345-678</param>
    /// <returns>Dados completos do endereço</returns>
    /// <response code="200">Endereço encontrado</response>
    /// <response code="500">Erro ao consultar CEP</response>
    [HttpGet("cep/{cep}")]
    [SwaggerOperation(
        Summary = "Consulta endereço por CEP",
        Description = "Integra com a API pública ViaCEP para obter dados completos de endereço a partir do CEP"
    )]
    [SwaggerResponse(200, "Endereço encontrado", typeof(object))]
    [SwaggerResponse(500, "Erro ao consultar CEP")]
    public async Task<ActionResult<object>> GetAddressByCep(
        [FromRoute, SwaggerParameter("CEP (formato: 12345678 ou 12345-678)", Required = true)] string cep)
    {
        try
        {
            // API pública do ViaCEP
            var url = $"https://viacep.com.br/ws/{cep}/json/";
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erro ao consultar CEP");
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);

            return Ok(new
            {
                Cep = cep,
                Endereco = data,
                ConsultadoEm = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    /// <summary>
    /// Obtém cotação de ação brasileira (B3)
    /// </summary>
    /// <param name="codigo">Código da ação brasileira (ex: PETR4, VALE3, ITUB4)</param>
    /// <returns>Dados de cotação da ação brasileira</returns>
    /// <response code="200">Cotação obtida com sucesso</response>
    /// <response code="404">Ação não encontrada</response>
    /// <response code="500">Erro ao consultar API</response>
    [HttpGet("cotacao-br/{codigo}")]
    [SwaggerOperation(
        Summary = "Consulta cotação de ação brasileira",
        Description = "Conecta com APIs brasileiras confiáveis (Brapi.dev e Yahoo Finance) para obter cotação em tempo real de ações da B3"
    )]
    [SwaggerResponse(200, "Cotação obtida com sucesso", typeof(object))]
    [SwaggerResponse(404, "Ação não encontrada")]
    [SwaggerResponse(500, "Erro ao consultar API")]
    public async Task<ActionResult<object>> GetBrazilianStockPrice(
        [FromRoute, SwaggerParameter("Código da ação brasileira (PETR4, VALE3, ITUB4, etc.)", Required = true)] string codigo)
    {
        try
        {
            // Normalizar código (remover .SA se existir, converter para maiúsculo)
            codigo = codigo.Replace(".SA", "").ToUpper();
            
            // Primeira tentativa: Brapi.dev (API brasileira confiável)
            try
            {
                var brapiUrl = $"https://brapi.dev/api/quote/{codigo}";
                var brapiResponse = await _httpClient.GetAsync(brapiUrl);
                
                if (brapiResponse.IsSuccessStatusCode)
                {
                    var brapiContent = await brapiResponse.Content.ReadAsStringAsync();
                    var brapiData = JsonSerializer.Deserialize<JsonElement>(brapiContent);
                    
                    if (brapiData.TryGetProperty("results", out var results) && 
                        results.GetArrayLength() > 0)
                    {
                        return Ok(new
                        {
                            Codigo = codigo,
                            Fonte = "Brapi.dev",
                            Dados = results[0],
                            ConsultadoEm = DateTime.UtcNow
                        });
                    }
                }
            }
            catch
            {
                // Continua para Yahoo Finance se Brapi falhar
            }
            
            // Segunda tentativa: Yahoo Finance com sufixo .SA
            try
            {
                var yahooSymbol = $"{codigo}.SA";
                var yahooUrl = $"https://query1.finance.yahoo.com/v8/finance/chart/{yahooSymbol}";
                
                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36");
                
                var yahooResponse = await _httpClient.GetAsync(yahooUrl);
                
                if (yahooResponse.IsSuccessStatusCode)
                {
                    var yahooContent = await yahooResponse.Content.ReadAsStringAsync();
                    var yahooData = JsonSerializer.Deserialize<JsonElement>(yahooContent);
                    
                    if (yahooData.TryGetProperty("chart", out var chart) &&
                        chart.TryGetProperty("result", out var result) &&
                        result.GetArrayLength() > 0)
                    {
                        var stockInfo = result[0];
                        return Ok(new
                        {
                            Codigo = codigo,
                            Fonte = "Yahoo Finance",
                            Dados = stockInfo,
                            ConsultadoEm = DateTime.UtcNow
                        });
                    }
                }
            }
            catch
            {
                // Se ambas as APIs falharem
            }
            
            return NotFound(new
            {
                Erro = "Ação não encontrada",
                Codigo = codigo,
                Mensagem = "Não foi possível encontrar dados para este código de ação. Verifique se o código está correto e se a ação está listada na B3.",
                APIsConsultadas = new[] { "Brapi.dev", "Yahoo Finance" },
                Sugestao = "Tente códigos como: PETR4, VALE3, ITUB4, BBAS3, BBDC4, MGLU3"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                Erro = "Erro interno",
                Mensagem = ex.Message,
                Codigo = codigo
            });
        }
    }

    /// <summary>
    /// Obtém preço atual do Bitcoin
    /// </summary>
    /// <returns>Preço do Bitcoin em BRL e USD</returns>
    /// <response code="200">Preço obtido com sucesso</response>
    /// <response code="500">Erro ao consultar preço</response>
    [HttpGet("bitcoin-price")]
    [SwaggerOperation(
        Summary = "Consulta preço do Bitcoin",
        Description = "Conecta com a API CoinGecko para obter o preço atual do Bitcoin em Real Brasileiro e Dólar Americano"
    )]
    [SwaggerResponse(200, "Preço do Bitcoin obtido", typeof(object))]
    [SwaggerResponse(500, "Erro ao consultar preço do Bitcoin")]
    public async Task<ActionResult<object>> GetBitcoinPrice()
    {
        try
        {
            // API pública do CoinGecko
            var url = "https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=brl,usd";
            
            var response = await _httpClient.GetAsync(url);
            
            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, "Erro ao consultar preço do Bitcoin");
            }

            var content = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(content);

            return Ok(new
            {
                Bitcoin = data,
                ConsultadoEm = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    /// <summary>
    /// Inicializa o banco com dados de exemplo para testes
    /// </summary>
    /// <returns>Confirmação da inicialização</returns>
    /// <response code="200">Dados de exemplo criados com sucesso</response>
    [HttpPost("inicializar-dados")]
    [SwaggerOperation(
        Summary = "Inicializa dados de exemplo",
        Description = "Cria usuários e investimentos de exemplo para facilitar os testes da API. Use apenas em ambiente de desenvolvimento."
    )]
    [SwaggerResponse(200, "Dados de exemplo criados com sucesso")]
    public async Task<ActionResult> InicializarDados()
    {
        try
        {
            // Verificar se já existem dados
            var existingInvestments = await _repository.GetAllAsync();
            if (existingInvestments.Any())
            {
                return Ok(new { message = "Dados já existem no banco. Use apenas quando o banco estiver vazio." });
            }

            // Criar investimentos de exemplo (que criarão usuários automaticamente)
            var investimentosExemplo = new List<Investimento>
            {
                new Investimento
                {
                    UserCpf = "52604928238",
                    Tipo = "Ação",
                    Codigo = "PETR4",
                    Valor = 1000.50m,
                    Operacao = "compra"
                },
                new Investimento
                {
                    UserCpf = "52604928238",
                    Tipo = "Ação",
                    Codigo = "VALE3",
                    Valor = 2500.00m,
                    Operacao = "compra"
                },
                new Investimento
                {
                    UserCpf = "52604928238",
                    Tipo = "CDB",
                    Codigo = "CDB001",
                    Valor = 5000.00m,
                    Operacao = "compra"
                },
                new Investimento
                {
                    UserCpf = "11122233344",
                    Tipo = "Tesouro",
                    Codigo = "TESOURO_SELIC",
                    Valor = 3000.00m,
                    Operacao = "compra"
                },
                new Investimento
                {
                    UserCpf = "11122233344",
                    Tipo = "Fundo",
                    Codigo = "FUNDO_XP",
                    Valor = 1500.00m,
                    Operacao = "compra"
                }
            };

            foreach (var investimento in investimentosExemplo)
            {
                await _repository.CreateAsync(investimento);
            }

            return Ok(new 
            { 
                message = "Dados de exemplo criados com sucesso!",
                usuariosCriados = new[] { "52604928238", "11122233344" },
                investimentosCriados = investimentosExemplo.Count,
                instrucoes = "Agora você pode testar todos os endpoints da API usando os CPFs: 52604928238 e 11122233344"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao inicializar dados: {ex.Message}");
        }
    }
}