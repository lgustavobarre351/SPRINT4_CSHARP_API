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

    public InvestimentosController(IInvestimentoRepository repository)
    {
        _repository = repository;
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
}