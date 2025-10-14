using Microsoft.EntityFrameworkCore;
using ProjetoInvestimentos.Data;
using ProjetoInvestimentos.Models;

namespace ProjetoInvestimentos.Repositories;

public interface IInvestimentoRepository
{
    Task<IEnumerable<Investimento>> GetAllAsync();
    Task<Investimento?> GetByIdAsync(Guid id);
    Task<IEnumerable<Investimento>> GetByUserCpfAsync(string userCpf);
    Task<Investimento> CreateAsync(Investimento investimento);
    Task<Investimento> UpdateAsync(Investimento investimento);
    Task DeleteAsync(Guid id);
    
    // LINQ queries (peso 10%)
    Task<IEnumerable<Investimento>> GetByTipoAsync(string tipo);
    Task<IEnumerable<Investimento>> GetByOperacaoAsync(string operacao);
    Task<decimal> GetTotalValueByUserAsync(string userCpf);
    Task<IEnumerable<Investimento>> GetRecentInvestmentsAsync(int days = 30);
    Task<IEnumerable<object>> GetInvestmentSummaryByTypeAsync();
    Task<IEnumerable<string>> GetAllUserCpfsAsync();
}

public class EfInvestimentoRepository : IInvestimentoRepository
{
    private readonly AppDbContext _context;

    public EfInvestimentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Investimento>> GetAllAsync()
    {
        var investimentos = await (from i in _context.Investimentos
                                   join u in _context.UserProfiles on i.UserId equals u.Id
                                   orderby i.CriadoEm descending
                                   select new { Investimento = i, UserCpf = u.Cpf })
                                  .ToListAsync();

        // Preencher UserCpf para cada investimento
        foreach (var item in investimentos)
        {
            item.Investimento.UserCpf = item.UserCpf;
        }

        return investimentos.Select(x => x.Investimento);
    }

    public async Task<Investimento?> GetByIdAsync(Guid id)
    {
        return await _context.Investimentos
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Investimento>> GetByUserCpfAsync(string userCpf)
    {
        var investimentos = await (from i in _context.Investimentos
                                   join u in _context.UserProfiles on i.UserId equals u.Id
                                   where u.Cpf == userCpf
                                   orderby i.CriadoEm descending
                                   select i).ToListAsync();

        // Preencher UserCpf para cada investimento
        foreach (var inv in investimentos)
        {
            inv.UserCpf = userCpf;
        }

        return investimentos;
    }

    public async Task<Investimento> CreateAsync(Investimento investimento)
    {
        investimento.Id = Guid.NewGuid();
        investimento.CriadoEm = DateTime.UtcNow;
        investimento.AlteradoEm = DateTime.UtcNow;

        // Se UserCpf foi fornecido, buscar ou criar o usuário
        if (!string.IsNullOrEmpty(investimento.UserCpf))
        {
            var user = await _context.UserProfiles
                .FirstOrDefaultAsync(u => u.Cpf == investimento.UserCpf);
            
            if (user == null)
            {
                // Criar usuário automaticamente se não existir
                user = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    Cpf = investimento.UserCpf,
                    Nome = null, // Nome pode ser definido posteriormente
                    CriadoEm = DateTime.UtcNow,
                    AlteradoEm = DateTime.UtcNow
                };
                
                _context.UserProfiles.Add(user);
                await _context.SaveChangesAsync();
            }
            
            investimento.UserId = user.Id;
        }

        _context.Investimentos.Add(investimento);
        await _context.SaveChangesAsync();
        return investimento;
    }

    public async Task<Investimento> UpdateAsync(Investimento investimento)
    {
        investimento.AlteradoEm = DateTime.UtcNow;
        _context.Investimentos.Update(investimento);
        await _context.SaveChangesAsync();
        return investimento;
    }

    public async Task DeleteAsync(Guid id)
    {
        var investimento = await GetByIdAsync(id);
        if (investimento != null)
        {
            _context.Investimentos.Remove(investimento);
            await _context.SaveChangesAsync();
        }
    }

    // LINQ Queries (peso 10%)
    public async Task<IEnumerable<Investimento>> GetByTipoAsync(string tipo)
    {
        return await _context.Investimentos
            .Where(i => i.Tipo.ToLower() == tipo.ToLower())
            .OrderByDescending(i => i.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<Investimento>> GetByOperacaoAsync(string operacao)
    {
        return await _context.Investimentos
            .Where(i => i.Operacao.ToLower() == operacao.ToLower())
            .OrderByDescending(i => i.CriadoEm)
            .ToListAsync();
    }

    public async Task<decimal> GetTotalValueByUserAsync(string userCpf)
    {
        return await (from i in _context.Investimentos
                      join u in _context.UserProfiles on i.UserId equals u.Id
                      where u.Cpf == userCpf
                      select i.Operacao.ToLower() == "compra" ? i.Valor : -i.Valor)
                     .SumAsync();
    }

    public async Task<IEnumerable<Investimento>> GetRecentInvestmentsAsync(int days = 30)
    {
        var cutoffDate = DateTime.UtcNow.AddDays(-days);
        return await _context.Investimentos
            .Where(i => i.CriadoEm >= cutoffDate)
            .OrderByDescending(i => i.CriadoEm)
            .ToListAsync();
    }

    public async Task<IEnumerable<object>> GetInvestmentSummaryByTypeAsync()
    {
        return await _context.Investimentos
            .GroupBy(i => i.Tipo)
            .Select(g => new
            {
                Tipo = g.Key,
                TotalInvestimentos = g.Count(),
                ValorTotal = g.Sum(i => i.Operacao.ToLower() == "compra" ? i.Valor : -i.Valor),
                ValorMedio = g.Average(i => i.Valor)
            })
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetAllUserCpfsAsync()
    {
        return await (from i in _context.Investimentos
                      join u in _context.UserProfiles on i.UserId equals u.Id
                      select u.Cpf)
                     .Distinct()
                     .OrderBy(cpf => cpf)
                     .ToListAsync();
    }
}