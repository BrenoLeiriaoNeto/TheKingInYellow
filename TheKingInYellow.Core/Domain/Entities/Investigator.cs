using TheKingInYellow.Core.Domain.ValueObjects;
using TheKingInYellow.Core.Services;

namespace TheKingInYellow.Core.Domain.Entities;

public class Investigator
{
    public string Name { get; private set; }
    public int Age { get; private set; }
    public string OccupationId { get; private set; }
    
    public StatValue Str { get; private set; }
    public StatValue Con { get; private set; }
    public StatValue Siz { get; private set; }
    public StatValue Dex { get; private set; }
    public StatValue App { get; private set; }
    public StatValue Int { get; private set; }
    public StatValue Pow { get; private set; }
    public StatValue Edu { get; private set; }
    public StatValue Luck { get; private set; }
    
    public int CurrentHp { get; private set; }
    public int MaxHp { get; private set; }

    public int CurrentLucidity { get; private set; }
    public int MaxLucidity { get; private set; }

    public int CurrentMp { get; private set; }
    public int MaxMp { get; private set; }
    
    public int MovementRate { get; private set; }
    public string DamageBonus { get; private set; }
    public int Build { get; private set; }
    public int CreditRating { get; private set; }
    
    private readonly Dictionary<string, StatValue> _skills = new();
    public IReadOnlyDictionary<string, StatValue> Skills => _skills;

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
        
        Str = new StatValue(baseAttributes["STR"]);
        Con = new StatValue(baseAttributes["CON"]);
        Siz = new StatValue(baseAttributes["SIZ"]);
        Dex = new StatValue(baseAttributes["DEX"]);
        App = new StatValue(baseAttributes["APP"]);
        Int = new StatValue(baseAttributes["INT"]);
        Pow = new StatValue(baseAttributes["POW"]);
        Edu = new StatValue(baseAttributes["EDU"]);
        Luck = new StatValue(luck);
        
        MaxHp = InvestigatorCalculator.CalculateMaxHp(Con.Base, Siz.Base);
        CurrentHp = MaxHp;

        MaxLucidity = InvestigatorCalculator.CalculateMaxLucidity(Pow.Base);
        CurrentLucidity = MaxLucidity;

        MaxMp = InvestigatorCalculator.CalculateMaxMp(Pow.Base);
        CurrentMp = MaxMp;

        MovementRate =
            InvestigatorCalculator.CalculateMovementRate(Str.Base, Dex.Base, Siz.Base, Age);
        (DamageBonus, Build) =
            InvestigatorCalculator.CalculateDamageBonusAndBuild(Str.Base, Siz.Base);

        foreach (var (skillId, val) in allocatedSkills)
        {
            _skills[skillId] = new StatValue(val);
        }
    }

    public void TakeLucidityDamage(int amount) =>
        CurrentLucidity = Math.Max(0, CurrentLucidity - amount);
    
    public void HealHp(int amount) => CurrentHp = Math.Min(MaxHp, CurrentHp + amount);

}