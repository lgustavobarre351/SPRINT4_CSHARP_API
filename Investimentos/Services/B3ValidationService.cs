using System.Collections.Concurrent;

namespace ProjetoInvestimentos.Services;

/// <summary>
/// Serviço para validar códigos de ações da B3 usando CSV
/// </summary>
public interface IB3ValidationService
{
    Task<bool> IsB3StockAsync(string codigo);
    Task<List<string>> GetAllB3CodesAsync();
    Task ReloadAsync();
}

public class B3ValidationService : IB3ValidationService
{
    private readonly ILogger<B3ValidationService> _logger;
    private readonly ConcurrentDictionary<string, bool> _b3Codes = new();
    private DateTime _lastLoad = DateTime.MinValue;
    private readonly SemaphoreSlim _loadLock = new(1, 1);

    public B3ValidationService(ILogger<B3ValidationService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> IsB3StockAsync(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo)) return false;
        
        await EnsureLoadedAsync();
        codigo = codigo.Replace(".SA", "").Trim().ToUpper();
        return _b3Codes.ContainsKey(codigo);
    }

    public async Task<List<string>> GetAllB3CodesAsync()
    {
        await EnsureLoadedAsync();
        return _b3Codes.Keys.ToList();
    }

    public async Task ReloadAsync()
    {
        await _loadLock.WaitAsync();
        try
        {
            await LoadCsvAsync();
        }
        finally
        {
            _loadLock.Release();
        }
    }

    private async Task EnsureLoadedAsync()
    {
        if (_b3Codes.IsEmpty || DateTime.UtcNow.Subtract(_lastLoad).TotalHours > 6)
        {
            await _loadLock.WaitAsync();
            try
            {
                if (_b3Codes.IsEmpty || DateTime.UtcNow.Subtract(_lastLoad).TotalHours > 6)
                {
                    await LoadCsvAsync();
                }
            }
            finally
            {
                _loadLock.Release();
            }
        }
    }

    private async Task LoadCsvAsync()
    {
        try
        {
            // Primeiro, tentar carregar o CSV do usuário
            var userCsvPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "acoes-listadas-b3.csv");
            
            if (File.Exists(userCsvPath))
            {
                await LoadFromCsvFile(userCsvPath);
                _logger.LogInformation($"Carregados {_b3Codes.Count} códigos B3 do arquivo: {userCsvPath}");
                return;
            }

            // Se não encontrar, procurar em outros locais
            var possiblePaths = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "acoes-listadas-b3.csv"),
                Path.Combine(AppContext.BaseDirectory, "Data", "acoes-listadas-b3.csv"),
                "acoes-listadas-b3.csv"
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    await LoadFromCsvFile(path);
                    _logger.LogInformation($"Carregados {_b3Codes.Count} códigos B3 do arquivo: {path}");
                    return;
                }
            }

            // Se não encontrar nenhum CSV, usar lista básica
            LoadBasicList();
            _logger.LogWarning($"CSV não encontrado. Usando lista básica com {_b3Codes.Count} códigos");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao carregar códigos B3: {ex.Message}");
            LoadBasicList();
        }
    }

    private async Task LoadFromCsvFile(string filePath)
    {
        _b3Codes.Clear();
        var lines = await File.ReadAllLinesAsync(filePath);
        
        foreach (var line in lines.Skip(1)) // Pular cabeçalho
        {
            if (string.IsNullOrWhiteSpace(line)) continue;
            
            try
            {
                // Extrair primeiro campo (ticker) - pode estar entre aspas
                var ticker = ExtractFirstField(line);
                if (!string.IsNullOrWhiteSpace(ticker))
                {
                    ticker = ticker.Replace(".SA", "").Trim().ToUpper();
                    _b3Codes.TryAdd(ticker, true);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning($"Erro ao processar linha CSV: {line}. Erro: {ex.Message}");
            }
        }
        
        _lastLoad = DateTime.UtcNow;
    }

    private string ExtractFirstField(string csvLine)
    {
        // Remover aspas se existirem
        if (csvLine.StartsWith("\""))
        {
            var endQuote = csvLine.IndexOf("\"", 1);
            if (endQuote > 0)
            {
                return csvLine.Substring(1, endQuote - 1);
            }
        }
        
        // Se não tem aspas, pegar até a primeira vírgula
        var commaIndex = csvLine.IndexOf(',');
        if (commaIndex > 0)
        {
            return csvLine.Substring(0, commaIndex);
        }
        
        return csvLine;
    }

    private void LoadBasicList()
    {
        _b3Codes.Clear();
        
        // Lista básica das principais ações da B3 (caso o CSV não seja encontrado)
        var basicCodes = new[]
        {
            "PETR3", "PETR4", "VALE3", "ITUB4", "BBDC3", "BBDC4", "ABEV3", "B3SA3", 
            "BBAS3", "MGLU3", "WEGE3", "RAIL3", "RENT3", "CSAN3", "ASAI3", "COGN3",
            "AZUL4", "ITSA4", "CSNA3", "VAMO3", "CVCB3", "PCAR3", "BRAV3", "LREN3",
            "DIRR3", "CPLE6", "MRVE3", "MBRF3", "HAPV3", "VBBR3", "USIM5", "MOTV3",
            "PRIO3", "GGBR4", "TIMS3", "UGPA3", "BEEF3", "POMO4", "BPAC11", "NATU3",
            "CMIN3", "CMIG4", "BRKM5", "ALPA4", "ENEV3", "AURE3", "SUZB3", "ELET3",
            "ELET6", "EMBR3", "KLBN11", "GOAU4", "SBSP3", "TAEE11", "VIVT3", "RADL3",
            "NTCO3", "MRFG3", "JBSS3", "BRAP4", "CPFE3", "ELET4", "MULT3", "QUAL3",
            "IRBR3", "SANB11", "TOTS3", "LWSA3", "MDIA3", "EVEN3", "CYRE3", "JHSF3"
        };

        foreach (var code in basicCodes)
        {
            _b3Codes.TryAdd(code, true);
        }
        
        _lastLoad = DateTime.UtcNow;
    }
}