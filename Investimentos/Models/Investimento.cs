using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace ProjetoInvestimentos.Models;

/// <summary>
/// Modelo de dados para investimentos
/// </summary>
[SwaggerSchema("Representa um investimento realizado por um usuário")]
public class Investimento
{
    /// <summary>
    /// Identificador único do investimento
    /// </summary>
    [SwaggerSchema("ID único do investimento (gerado automaticamente)")]
    public Guid Id { get; set; }

    /// <summary>
    /// CPF do usuário proprietário do investimento
    /// </summary>
    [Required(ErrorMessage = "CPF é obrigatório")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "CPF deve ter 11 dígitos")]
    [SwaggerSchema("CPF do usuário (apenas números, 11 dígitos)")]
    public String UserCpf { get; set; } = string.Empty;
    
    /// <summary>
    /// ID do usuário (referência interna)
    /// </summary>
    [SwaggerSchema("ID interno do usuário")]
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Tipo do investimento
    /// </summary>
    [Required(ErrorMessage = "Tipo é obrigatório")]
    [StringLength(50, ErrorMessage = "Tipo deve ter no máximo 50 caracteres")]
    [SwaggerSchema("Tipo de investimento")]
    public string Tipo { get; set; } = string.Empty;
    
    /// <summary>
    /// Código do ativo investido
    /// </summary>
    [Required(ErrorMessage = "Código é obrigatório")]
    [StringLength(20, ErrorMessage = "Código deve ter no máximo 20 caracteres")]
    [SwaggerSchema("Código do ativo")]
    public string Codigo { get; set; } = string.Empty;
    
    /// <summary>
    /// Valor monetário do investimento
    /// </summary>
    [Required(ErrorMessage = "Valor é obrigatório")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
    [SwaggerSchema("Valor do investimento em reais")]
    public decimal Valor { get; set; }
    
    /// <summary>
    /// Tipo de operação realizada
    /// </summary>
    [Required(ErrorMessage = "Operação é obrigatória")]
    [RegularExpression("^(compra|venda)$", ErrorMessage = "Operação deve ser 'compra' ou 'venda'")]
    [SwaggerSchema("Tipo de operação")]
    public string Operacao { get; set; } = string.Empty; // "compra" ou "venda"
    
    /// <summary>
    /// Data e hora de criação do registro
    /// </summary>
    [SwaggerSchema("Data de criação do investimento")]
    public DateTime CriadoEm { get; set; } = DateTime.Now;
    
    /// <summary>
    /// Data e hora da última alteração
    /// </summary>
    [SwaggerSchema("Data da última alteração")]
    public DateTime AlteradoEm { get; set; } = DateTime.Now;
}
