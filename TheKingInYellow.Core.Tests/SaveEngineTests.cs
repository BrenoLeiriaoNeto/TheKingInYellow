using System.Text.Json;
using TheKingInYellow.Core.Domain.Primitives;
using TheKingInYellow.Core.Services;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class SaveEngineTests : IDisposable
{
    private readonly string _tempDirectory;

    public SaveEngineTests()
    {
        _tempDirectory = Path.Combine(Path.GetTempPath(), "SaveEngineTests_" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(_tempDirectory);
    }

    public void Dispose()
    {
        if (Directory.Exists(_tempDirectory))
        {
            try
            {
                Directory.Delete(_tempDirectory, recursive: true);
            }
            catch
            {
                // Ignora falhas de limpeza no descarte temporário
            }
        }
    }

    private record TestPayload(string Name, int Level, List<string> Tags);

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void LoadFromFile_NullOrWhiteSpacePath_ReturnsFailure(string? filePath)
    {
        var result = SaveEngine.LoadFromFile<TestPayload>(filePath!);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("O caminho do arquivo não pode ser nulo ou vazio.", result.Error);
    }

    [Fact]
    public void LoadFromFile_NonExistentFile_ReturnsFailure()
    {
        var filePath = Path.Combine(_tempDirectory, "arquivo_inexistente.json");

        var result = SaveEngine.LoadFromFile<TestPayload>(filePath);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Contains("não existe", result.Error);
    }

    [Fact]
    public void LoadFromFile_ValidJson_ReturnsSuccessWithDeserializedData()
    {
        var filePath = Path.Combine(_tempDirectory, "valid_payload.json");
        var original = new TestPayload("Hastur", 10, ["King", "Yellow", "Carcosa"]);
        File.WriteAllText(filePath, JsonSerializer.Serialize(original));

        var result = SaveEngine.LoadFromFile<TestPayload>(filePath);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.NotNull(result.Value);
        Assert.Equal(original.Name, result.Value.Name);
        Assert.Equal(original.Level, result.Value.Level);
        Assert.Equal(original.Tags, result.Value.Tags);
    }

    [Fact]
    public void LoadFromFile_CaseInsensitivePropertyNames_ReturnsSuccess()
    {
        var filePath = Path.Combine(_tempDirectory, "case_test.json");
        var json = """
        {
            "name": "Cassilda",
            "level": 5,
            "tags": ["Song", "Aldebaran"]
        }
        """;
        File.WriteAllText(filePath, json);

        var result = SaveEngine.LoadFromFile<TestPayload>(filePath);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal("Cassilda", result.Value.Name);
        Assert.Equal(5, result.Value.Level);
        Assert.Equal(2, result.Value.Tags.Count);
    }

    [Fact]
    public void LoadFromFile_InvalidJsonFormat_ReturnsFailure()
    {
        var filePath = Path.Combine(_tempDirectory, "invalid_format.json");
        File.WriteAllText(filePath, "{ nomeIncompleto: ");

        var result = SaveEngine.LoadFromFile<TestPayload>(filePath);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Contains("Erro ao desserializar", result.Error);
    }

    [Fact]
    public void LoadFromFile_JsonNullLiteral_ReturnsFailure()
    {
        var filePath = Path.Combine(_tempDirectory, "null_literal.json");
        File.WriteAllText(filePath, "null");

        var result = SaveEngine.LoadFromFile<TestPayload>(filePath);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("A desserialização retornou um objeto nulo.", result.Error);
    }

    [Fact]
    public void LoadFromFile_PathIsDirectory_ReturnsFailure()
    {
        var result = SaveEngine.LoadFromFile<TestPayload>(_tempDirectory);

        // File.Exists para diretório retorna false
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Contains("não existe", result.Error);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void SaveToFile_NullOrWhiteSpacePath_ReturnsFailure(string? filePath)
    {
        var data = new TestPayload("Pallid Mask", 1, []);

        var result = SaveEngine.SaveToFile(filePath!, data);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("O caminho do arquivo não pode ser nulo ou vazio.", result.Error);
    }

    [Fact]
    public void SaveToFile_NullData_ReturnsFailure()
    {
        var filePath = Path.Combine(_tempDirectory, "null_data.json");

        var result = SaveEngine.SaveToFile<TestPayload>(filePath, null!);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal("Nenhum dado fornecido para salvamento.", result.Error);
    }

    [Fact]
    public void SaveToFile_ValidData_CreatesFileWithIndentedJsonAndReturnsSuccess()
    {
        var filePath = Path.Combine(_tempDirectory, "saved_payload.json");
        var data = new TestPayload("Stranger", 7, ["Phantom", "Truth"]);

        var result = SaveEngine.SaveToFile(filePath, data);

        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.True(File.Exists(filePath));

        var fileContent = File.ReadAllText(filePath);
        Assert.Contains("\n", fileContent); // Indented JSON contém quebras de linha
        Assert.Contains("Stranger", fileContent);

        var loaded = JsonSerializer.Deserialize<TestPayload>(fileContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        Assert.NotNull(loaded);
        Assert.Equal(data.Name, loaded.Name);
        Assert.Equal(data.Level, loaded.Level);
        Assert.Equal(data.Tags, loaded.Tags);
    }

    [Fact]
    public void SaveToFile_ExistingFile_OverwritesContentAndReturnsSuccess()
    {
        var filePath = Path.Combine(_tempDirectory, "overwrite.json");
        var initialData = new TestPayload("Initial", 1, []);
        var updatedData = new TestPayload("Updated", 2, ["NewTag"]);

        SaveEngine.SaveToFile(filePath, initialData);
        var result = SaveEngine.SaveToFile(filePath, updatedData);

        Assert.True(result.IsSuccess);
        var loadedResult = SaveEngine.LoadFromFile<TestPayload>(filePath);
        Assert.True(loadedResult.IsSuccess);
        Assert.NotNull(loadedResult.Value);
        Assert.Equal("Updated", loadedResult.Value.Name);
        Assert.Equal(2, loadedResult.Value.Level);
    }

    [Fact]
    public void SaveToFile_InvalidDirectory_ReturnsFailure()
    {
        var nonExistentDir = Path.Combine(_tempDirectory, "sub_inexistente", "outro_sub");
        var filePath = Path.Combine(nonExistentDir, "save.json");
        var data = new TestPayload("Test", 1, []);

        var result = SaveEngine.SaveToFile(filePath, data);

        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Contains("Falha ao salvar o arquivo", result.Error);
    }

    [Fact]
    public void SaveAndLoad_RoundTrip_PreservesDataIntegrity()
    {
        var filePath = Path.Combine(_tempDirectory, "roundtrip.json");
        var original = new TestPayload("Camilla", 99, ["Tattered", "Crown", "Lake of Hali"]);

        var saveResult = SaveEngine.SaveToFile(filePath, original);
        Assert.True(saveResult.IsSuccess);

        var loadResult = SaveEngine.LoadFromFile<TestPayload>(filePath);
        Assert.True(loadResult.IsSuccess);
        Assert.NotNull(loadResult.Value);
        Assert.Equal(original.Name, loadResult.Value.Name);
        Assert.Equal(original.Level, loadResult.Value.Level);
        Assert.Equal(original.Tags, loadResult.Value.Tags);
    }
}
