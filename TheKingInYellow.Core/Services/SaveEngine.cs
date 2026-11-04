using System.Text.Json;
using TheKingInYellow.Core.Domain.Primitives;

namespace TheKingInYellow.Core.Services;

/// <summary>
/// Serviço estático responsável por carregar e salvar arquivos JSON estáticos e dados de progresso/save do jogador.
/// Atua como uma fronteira de segurança, convertendo exceções de I/O e serialização no padrão <see cref="Result"/>.
/// </summary>
public static class SaveEngine
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };
    
    /// <summary>
    /// Carrega e desserializa o conteúdo de um arquivo JSON para o tipo especificado, falhando graciosamente em caso de erro.
    /// </summary>
    /// <typeparam name="T">O tipo do objeto para o qual o JSON será desserializado.</typeparam>
    /// <param name="filePath">O caminho relativo ou absoluto para o arquivo JSON a ser lido.</param>
    /// <returns>Um <see cref="Result{T}"/> com os dados desserializados em caso de sucesso ou a mensagem de falha correspondente.</returns>
    public static Result<T> LoadFromFile<T>(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Result<T>.Failure("O caminho do arquivo não pode ser nulo ou vazio.");
        
        if (!File.Exists(filePath))
            return Result<T>.Failure($"O arquivo '{filePath}' não existe.");

        try
        {
            var json = File.ReadAllText(filePath);
            var data = JsonSerializer.Deserialize<T>(json, JsonOptions);

            return data is null 
                ? Result<T>.Failure("A desserialização retornou um objeto nulo.") 
                : Result<T>.Success(data);
        }
        catch (JsonException e)
        {
            return Result<T>.Failure($"Erro ao desserializar o arquivo '{filePath}': {e.Message}");
        }
        catch (IOException e)
        {
            return Result<T>.Failure($"Erro ao ler o arquivo '{filePath}': {e.Message}");
        }
    }

    /// <summary>
    /// Serializa um objeto e salva seu conteúdo em formato JSON no caminho especificado.
    /// </summary>
    /// <typeparam name="T">O tipo do objeto a ser serializado e salvo.</typeparam>
    /// <param name="filePath">O caminho do arquivo de destino onde os dados serão salvos.</param>
    /// <param name="data">O objeto contendo os dados a serem serializados e persistidos.</param>
    /// <returns>Um <see cref="Result"/> indicando sucesso na operação de salvamento ou falha com a respectiva mensagem de erro.</returns>
    public static Result SaveToFile<T>(string filePath, T data)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return Result.Failure("O caminho do arquivo não pode ser nulo ou vazio.");

        if (data is null)
            return Result.Failure("Nenhum dado fornecido para salvamento.");

        try
        {
            var json = JsonSerializer.Serialize(data, JsonOptions);

            File.WriteAllText(filePath, json);
            
            return Result.Success();
        }
        catch (Exception e) when (e is JsonException or
                                      IOException or
                                      UnauthorizedAccessException or
                                      DirectoryNotFoundException)
        {
            return Result.Failure($"Falha ao salvar o arquivo '{filePath}': {e.Message}");
        }
    }
}