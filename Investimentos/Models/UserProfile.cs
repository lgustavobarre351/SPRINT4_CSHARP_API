using System.ComponentModel.DataAnnotations;

namespace ProjetoInvestimentos.Models;

/// <summary>
/// Modelo para perfis de usuário
/// </summary>
public class UserProfile
{
    /// <summary>
    /// ID único do usuário
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// CPF do usuário
    /// </summary>
    [Required]
    [StringLength(11)]
    public string Cpf { get; set; } = string.Empty;

    /// <summary>
    /// Nome do usuário
    /// </summary>
    public string? Nome { get; set; }

    /// <summary>
    /// Data de criação
    /// </summary>
    public DateTime CreatedAt { get; set; }
}