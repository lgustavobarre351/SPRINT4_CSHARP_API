using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoInvestimentos.Data;
using ProjetoInvestimentos.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Npgsql;

namespace ProjetoInvestimentos.Controllers;

/// <summary>
/// Gerenciamento de usuários
/// </summary>
[ApiController]
[Route("api/[controller]")]
[SwaggerTag("1️⃣ USUÁRIOS - Cadastro e gestão de perfis")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly string _connectionString;

    public UsuariosController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string not found");
    }

    /// <summary>
    /// Lista todos os usuários cadastrados
    /// </summary>
    /// <returns>Lista de usuários</returns>
    /// <response code="200">Lista de usuários retornada com sucesso</response>
    [HttpGet]
    [SwaggerOperation(
        Summary = "Lista todos os usuários",
        Description = "Retorna uma lista completa de todos os usuários cadastrados no sistema"
    )]
    [SwaggerResponse(200, "Lista de usuários retornada com sucesso", typeof(IEnumerable<UserProfile>))]
    public async Task<ActionResult<IEnumerable<UserProfile>>> GetAll()
    {
        var usuarios = await _context.UserProfiles
            .OrderBy(u => u.CriadoEm)
            .ToListAsync();
        return Ok(usuarios);
    }

    /// <summary>
    /// Busca um usuário pelo CPF
    /// </summary>
    /// <param name="cpf">CPF do usuário (apenas números, 11 dígitos)</param>
    /// <returns>Dados do usuário</returns>
    /// <response code="200">Usuário encontrado</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpGet("{cpf}")]
    [SwaggerOperation(
        Summary = "Busca usuário por CPF",
        Description = "Retorna os dados de um usuário específico baseado no seu CPF. Digite apenas os números do CPF, sem pontos ou traços."
    )]
    [SwaggerResponse(200, "Usuário encontrado", typeof(UserProfile))]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<ActionResult<UserProfile>> GetByCpf(
        [FromRoute, SwaggerParameter("CPF do usuário (apenas números, exemplo: 12345678901)", Required = true)] string cpf)
    {
        var usuario = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.Cpf == cpf);
        
        if (usuario == null)
            return NotFound($"Usuário com CPF {cpf} não encontrado");
        
        return Ok(usuario);
    }

    /// <summary>
    /// Cria um novo usuário
    /// </summary>
    /// <param name="request">Dados do usuário a ser criado</param>
    /// <returns>Usuário criado</returns>
    /// <response code="201">Usuário criado com sucesso</response>
    /// <response code="400">Dados inválidos ou CPF já existe</response>
    /// <response code="408">Timeout na operação</response>
    [HttpPost]
    [SwaggerOperation(
        Summary = "Cria um novo usuário",
        Description = "Cadastra um novo usuário no sistema. Use CPF apenas com números (11 dígitos). Email é opcional."
    )]
    [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UserProfile))]
    [SwaggerResponse(400, "Dados inválidos ou CPF já existe")]
    [SwaggerResponse(408, "Timeout na operação")]
    public async Task<ActionResult<UserProfile>> Create(
        [FromBody, SwaggerRequestBody("Dados do usuário", Required = true)] CreateUserRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Como a tabela user_profiles tem foreign key para auth.users,
            // vamos usar o endpoint CreateSimple que já funciona
            return await CreateSimple(request);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    /// <summary>
    /// Atualiza os dados de um usuário
    /// </summary>
    /// <param name="cpf">CPF do usuário</param>
    /// <param name="request">Novos dados do usuário</param>
    /// <returns>Usuário atualizado</returns>
    /// <response code="200">Usuário atualizado com sucesso</response>
    /// <response code="400">Dados inválidos</response>
    /// <response code="404">Usuário não encontrado</response>
    [HttpPut("{cpf}")]
    [SwaggerOperation(
        Summary = "Atualiza um usuário",
        Description = "Modifica os dados de um usuário existente"
    )]
    [SwaggerResponse(200, "Usuário atualizado com sucesso", typeof(UserProfile))]
    [SwaggerResponse(400, "Dados inválidos")]
    [SwaggerResponse(404, "Usuário não encontrado")]
    public async Task<ActionResult<UserProfile>> Update(
        [FromRoute, SwaggerParameter("CPF do usuário", Required = true)] string cpf,
        [FromBody, SwaggerRequestBody("Dados atualizados do usuário", Required = true)] UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var usuario = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.Cpf == cpf);
        
        if (usuario == null)
            return NotFound($"Usuário com CPF {cpf} não encontrado");

        usuario.Nome = request.Nome;
        
        await _context.SaveChangesAsync();
        
        return Ok(usuario);
    }

    /// <summary>
    /// Remove um usuário
    /// </summary>
    /// <param name="cpf">CPF do usuário a ser removido</param>
    /// <returns>Confirmação de remoção</returns>
    /// <response code="204">Usuário removido com sucesso</response>
    /// <response code="404">Usuário não encontrado</response>
    /// <response code="400">Usuário possui investimentos e não pode ser removido</response>
    [HttpDelete("{cpf}")]
    [SwaggerOperation(
        Summary = "Remove um usuário",
        Description = "Exclui um usuário do sistema. Só é possível remover usuários que não possuem investimentos."
    )]
    [SwaggerResponse(204, "Usuário removido com sucesso")]
    [SwaggerResponse(404, "Usuário não encontrado")]
    [SwaggerResponse(400, "Usuário possui investimentos e não pode ser removido")]
    public async Task<ActionResult> Delete(
        [FromRoute, SwaggerParameter("CPF do usuário a ser removido", Required = true)] string cpf)
    {
        var usuario = await _context.UserProfiles
            .FirstOrDefaultAsync(u => u.Cpf == cpf);
        
        if (usuario == null)
            return NotFound($"Usuário com CPF {cpf} não encontrado");

        // Verificar se o usuário possui investimentos
        var hasInvestments = await _context.Investimentos
            .AnyAsync(i => i.UserId == usuario.Id);
        
        if (hasInvestments)
            return BadRequest("Não é possível remover um usuário que possui investimentos. Remova os investimentos primeiro.");

        _context.UserProfiles.Remove(usuario);
        await _context.SaveChangesAsync();
        
        return NoContent();
    }

    /// <summary>
    /// Cria um usuário de forma simplificada
    /// </summary>
    /// <param name="request">Dados do usuário a ser criado</param>
    /// <returns>Usuário criado</returns>
    /// <response code="201">Usuário criado com sucesso</response>
    /// <response code="400">Dados inválidos ou CPF já existe</response>
    [HttpPost("simple")]
    [SwaggerOperation(
        Summary = "Cria um usuário de forma simplificada",
        Description = "Cadastra um novo usuário sem restrições de foreign key. Use este endpoint se o método normal apresentar timeout. CPF: apenas números, 11 dígitos. Email: opcional mas recomendado."
    )]
    [SwaggerResponse(201, "Usuário criado com sucesso", typeof(UserProfile))]
    [SwaggerResponse(400, "Dados inválidos ou CPF já existe")]
    public async Task<ActionResult<UserProfile>> CreateSimple(
        [FromBody, SwaggerRequestBody("Dados do usuário", Required = true)] CreateUserRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Usar SQL direto com a nova estrutura de banco (sem foreign key para auth.users)
            var agora = DateTime.UtcNow;
            var dados = !string.IsNullOrEmpty(request.Nome) ? 
                       System.Text.Json.JsonSerializer.Serialize(new { nome = request.Nome }) : 
                       null;

            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            // Primeiro verificar se o CPF já existe
            using var checkCommand = new NpgsqlCommand(
                "SELECT COUNT(1) FROM public.user_profiles WHERE cpf = @cpf", 
                connection);
            checkCommand.Parameters.AddWithValue("@cpf", request.Cpf);
            
            var result = await checkCommand.ExecuteScalarAsync();
            var exists = result != null && (long)result > 0;
            if (exists)
                return BadRequest($"Usuário com CPF {request.Cpf} já existe");

            // Inserir o novo usuário (ID será gerado automaticamente)
            using var insertCommand = new NpgsqlCommand(
                "INSERT INTO public.user_profiles (cpf, email, dados, criado_em, alterado_em) VALUES (@cpf, @email, @dados::jsonb, @criado_em, @alterado_em) RETURNING id", 
                connection);
            
            insertCommand.Parameters.AddWithValue("@cpf", request.Cpf);
            insertCommand.Parameters.AddWithValue("@email", string.IsNullOrEmpty(request.Email) ? DBNull.Value : request.Email);
            insertCommand.Parameters.AddWithValue("@dados", string.IsNullOrEmpty(dados) ? DBNull.Value : dados);
            insertCommand.Parameters.AddWithValue("@criado_em", agora);
            insertCommand.Parameters.AddWithValue("@alterado_em", agora);

            var insertedId = await insertCommand.ExecuteScalarAsync();
            var userId = insertedId != null ? (Guid)insertedId : Guid.NewGuid();

            var usuario = new UserProfile
            {
                Id = userId,
                Cpf = request.Cpf,
                Email = request.Email,
                Nome = request.Nome,
                Dados = dados,
                CriadoEm = agora,
                AlteradoEm = agora
            };

            return CreatedAtAction(nameof(GetByCpf), new { cpf = usuario.Cpf }, usuario);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}

/// <summary>
/// Dados para criação de usuário
/// </summary>
[SwaggerSchema("Dados necessários para criar um novo usuário")]
public class CreateUserRequest
{
    /// <summary>
    /// CPF do usuário (apenas números, sem pontos ou traços)
    /// </summary>
    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter exatamente 11 dígitos")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "CPF deve conter apenas números")]
    [SwaggerSchema("CPF do usuário (apenas números, 11 dígitos). Exemplo: 12345678901")]
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// Nome completo do usuário
    /// </summary>
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    [SwaggerSchema("Nome completo do usuário. Exemplo: João Silva Santos")]
    public string? Nome { get; set; }

    /// <summary>
    /// Email do usuário (opcional, mas recomendado)
    /// </summary>
    [EmailAddress(ErrorMessage = "Email deve ter um formato válido")]
    [SwaggerSchema("Email do usuário (opcional). Exemplo: joao.silva@email.com")]
    public string? Email { get; set; }
}

/// <summary>
/// Dados para atualização de usuário
/// </summary>
[SwaggerSchema("Dados para atualizar um usuário existente")]
public class UpdateUserRequest
{
    /// <summary>
    /// Nome do usuário
    /// </summary>
    [StringLength(200, ErrorMessage = "Nome deve ter no máximo 200 caracteres")]
    [SwaggerSchema("Nome completo do usuário")]
    public string? Nome { get; set; }
}