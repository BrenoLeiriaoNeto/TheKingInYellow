using TheKingInYellow.Core.Domain.ValueObjects;
using TheKingInYellow.Core.Domain.ValueObjects.Investigator;
using TheKingInYellow.Core.Services;

namespace TheKingInYellow.Core.Domain.Entities;

/// <summary>
/// Entidade que representa um Investigador no jogo, contendo seus atributos, perícias, estatísticas derivadas,
/// histórico biográfico, finanças, condição médica e conhecimento esotérico.
/// </summary>
public partial class Investigator
{
    /// <summary>
    /// O nome do investigador.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// A idade do investigador em anos.
    /// </summary>
    public int Age { get; private set; }

    /// <summary>
    /// O identificador da ocupação ou profissão do investigador.
    /// </summary>
    public string OccupationId { get; private set; }

    /// <summary>
    /// Informações biográficas e histórico pessoal do investigador.
    /// </summary>
    public Biography InvestigatorBiography { get; private set; } = new();

    /// <summary>
    /// Informações financeiras, gastos e patrimônio do investigador.
    /// </summary>
    public Finances InvestigatorFinances { get; private set; } = new(0, 0, 0);

    /// <summary>
    /// Condição médica, ferimentos e saúde mental do investigador.
    /// </summary>
    public MedicalState InvestigatorMedicalState { get; private set; } = new();

    /// <summary>
    /// Conhecimento de ocultismo, tomos proibidos e rituais conhecidos.
    /// </summary>
    public EsotericKnowledge InvestigatorEsotericKnowledge { get; private set; } = new();

    private readonly List<string> _inventoryItemIds = [];

    /// <summary>
    /// Lista de identificadores dos itens atualmente no inventário do investigador.
    /// </summary>
    public IReadOnlyList<string> InventoryItemIds => _inventoryItemIds;

    /// <summary>
    /// Atributo de Força (STR).
    /// </summary>
    public StatValue Str { get; private set; }

    /// <summary>
    /// Atributo de Constituição (CON).
    /// </summary>
    public StatValue Con { get; private set; }

    /// <summary>
    /// Atributo de Tamanho (SIZ).
    /// </summary>
    public StatValue Siz { get; private set; }

    /// <summary>
    /// Atributo de Destreza (DEX).
    /// </summary>
    public StatValue Dex { get; private set; }

    /// <summary>
    /// Atributo de Aparência (APP).
    /// </summary>
    public StatValue App { get; private set; }

    /// <summary>
    /// Atributo de Inteligência (INT).
    /// </summary>
    public StatValue Int { get; private set; }

    /// <summary>
    /// Atributo de Poder (POW).
    /// </summary>
    public StatValue Pow { get; private set; }

    /// <summary>
    /// Atributo de Educação (EDU).
    /// </summary>
    public StatValue Edu { get; private set; }

    /// <summary>
    /// Atributo de Sorte (Luck).
    /// </summary>
    public StatValue Luck { get; private set; }

    /// <summary>
    /// Pontos de Vida (HP) atuais.
    /// </summary>
    public int CurrentHp { get; private set; }

    /// <summary>
    /// Pontos de Vida (HP) máximos.
    /// </summary>
    public int MaxHp { get; private set; }

    /// <summary>
    /// Pontos de Lucidez / Sanidade atuais.
    /// </summary>
    public int CurrentLucidity { get; private set; }

    /// <summary>
    /// Pontos de Lucidez / Sanidade máximos.
    /// </summary>
    public int MaxLucidity { get; private set; }

    /// <summary>
    /// Pontos de Magia (MP) atuais.
    /// </summary>
    public int CurrentMp { get; private set; }

    /// <summary>
    /// Pontos de Magia (MP) máximos.
    /// </summary>
    public int MaxMp { get; private set; }

    /// <summary>
    /// Taxa de Movimento (MOV).
    /// </summary>
    public int MovementRate { get; private set; }

    /// <summary>
    /// Bônus de Dano (Damage Bonus).
    /// </summary>
    public string DamageBonus { get; private set; }

    /// <summary>
    /// Porte físico (Build).
    /// </summary>
    public int Build { get; private set; }

    /// <summary>
    /// Nível de Crédito (Credit Rating).
    /// </summary>
    public int CreditRating { get; private set; }

    private readonly Dictionary<string, StatValue> _skills = new();

    /// <summary>
    /// Dicionário contendo as perícias alocadas e seus respectivos valores.
    /// </summary>
    public IReadOnlyDictionary<string, StatValue> Skills => _skills;

    /// <summary>
    /// Inicializa uma nova instância de <see cref="Investigator"/> com seus atributos e perícias calculadas.
    /// </summary>
    /// <param name="name">Nome do investigador.</param>
    /// <param name="age">Idade do investigador.</param>
    /// <param name="occupationId">Identificador da ocupação.</param>
    /// <param name="baseAttributes">Dicionário contendo os valores base dos atributos (STR, CON, SIZ, DEX, APP, INT, POW, EDU).</param>
    /// <param name="luck">Valor do atributo Sorte.</param>
    /// <param name="creditRating">Valor da perícia Nível de Crédito.</param>
    /// <param name="allocatedSkills">Dicionário contendo as perícias alocadas e seus valores finais.</param>
    public Investigator(
        string name,
        int age,
        string occupationId,
        IReadOnlyDictionary<string, int> baseAttributes,
        int luck,
        int creditRating,
        IReadOnlyDictionary<string, int> allocatedSkills)
    {
        Name = name;
        Age = age;
        OccupationId = occupationId;
        CreditRating = creditRating;

        Str = GetStatOrFallback(baseAttributes,"STR");
        Con = GetStatOrFallback(baseAttributes,"CON");
        Siz = GetStatOrFallback(baseAttributes,"SIZ");
        Dex = GetStatOrFallback(baseAttributes,"DEX");
        App = GetStatOrFallback(baseAttributes,"APP");
        Int = GetStatOrFallback(baseAttributes,"INT");
        Pow = GetStatOrFallback(baseAttributes,"POW");
        Edu = GetStatOrFallback(baseAttributes,"EDU");
        Luck = new StatValue(luck);

        MaxHp = InvestigatorCalculator.CalculateMaxHp(Con, Siz);
        CurrentHp = MaxHp;

        MaxLucidity = InvestigatorCalculator.CalculateMaxLucidity(Pow);
        CurrentLucidity = MaxLucidity;

        MaxMp = InvestigatorCalculator.CalculateMaxMp(Pow);
        CurrentMp = MaxMp;

        MovementRate =
            InvestigatorCalculator.CalculateMovementRate(Str, Dex, Siz, Age);
        (DamageBonus, Build) =
            InvestigatorCalculator.CalculateDamageBonusAndBuild(Str, Siz);

        
        foreach (var (skillId, val) in allocatedSkills)
        {
            _skills[skillId] = new StatValue(val);
        }
    }

    /// <summary>
    /// Aplica dano à Lucidez do investigador, reduzindo-a até o limite mínimo de zero.
    /// </summary>
    /// <param name="amount">Quantidade de dano de lucidez a receber.</param>
    public void TakeLucidityDamage(int amount) =>
        CurrentLucidity = Math.Max(0, CurrentLucidity - amount);

    /// <summary>
    /// Restaura Pontos de Vida (HP) do investigador, limitando ao valor máximo de HP.
    /// </summary>
    /// <param name="amount">Quantidade de pontos de vida a curar.</param>
    public void HealHp(int amount) => CurrentHp = Math.Min(MaxHp, CurrentHp + amount);

    /// <summary>
    /// Processa a leitura de um livro proibido, aumentando o conhecimento do proibido, adicionando o tomo lido ao histórico e aplicando perda de lucidez.
    /// </summary>
    /// <param name="bookId">Identificador do livro proibido lido.</param>
    /// <param name="lucidityLoss">Quantidade de perda de lucidez sofrida.</param>
    /// <param name="loreGain">Quantidade de ganho em conhecimento proibido (Lore).</param>
    public void ReadForbiddenBook(string bookId, int lucidityLoss, int loreGain)
    {
        InvestigatorEsotericKnowledge = InvestigatorEsotericKnowledge with
        {
            ForbiddenLoreRating = InvestigatorEsotericKnowledge.ForbiddenLoreRating + loreGain,
            ReadForbiddenBooks =
            [.. (InvestigatorEsotericKnowledge.ReadForbiddenBooks ?? []), bookId]
        };

        TakeLucidityDamage(lucidityLoss);
    }

    /// <summary>
    /// Adiciona uma cicatriz, marca ou lesão permanente ao histórico biográfico do investigador.
    /// </summary>
    /// <param name="description">Descrição da cicatriz ou lesão sofrida.</param>
    public void AddScarOrInjury(string description)
    {
        var updatedScars = new List<string>(InvestigatorBiography.ScarsAndInjuries ?? [])
            { description };
        InvestigatorBiography = InvestigatorBiography with
        {
            ScarsAndInjuries = updatedScars
        };
    }

    private static StatValue GetStatOrFallback(IReadOnlyDictionary<string, int>? dict, string key,
        int fallback = 10)
    {
        if (dict != null && dict.TryGetValue(key, out var val))
            return new StatValue(val);
        
        return new StatValue(fallback);
    }
}