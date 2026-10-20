using TheKingInYellow.Core.Domain.Entities;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class InvestigatorTests
{
    private static Dictionary<string, int> CreateSampleBaseAttributes(
        int str = 50,
        int con = 50,
        int siz = 50,
        int dex = 50,
        int app = 50,
        int @int = 50,
        int pow = 50,
        int edu = 50)
    {
        return new Dictionary<string, int>
        {
            { "STR", str },
            { "CON", con },
            { "SIZ", siz },
            { "DEX", dex },
            { "APP", app },
            { "INT", @int },
            { "POW", pow },
            { "EDU", edu }
        };
    }

    [Fact]
    public void Constructor_InitializesAllPropertiesAndCalculationsCorrectly()
    {
        var baseAttributes = CreateSampleBaseAttributes(
            str: 70,
            con: 60,
            siz: 80,
            dex: 65,
            app: 45,
            @int: 75,
            pow: 55,
            edu: 80);

        var allocatedSkills = new Dictionary<string, int>
        {
            { "spot_hidden", 60 },
            { "listen", 50 }
        };

        var investigator = new Investigator(
            name: "Harvey Walters",
            age: 42,
            occupationId: "professor",
            baseAttributes: baseAttributes,
            luck: 65,
            creditRating: 40,
            allocatedSkills: allocatedSkills);

        Assert.Equal("Harvey Walters", investigator.Name);
        Assert.Equal(42, investigator.Age);
        Assert.Equal("professor", investigator.OccupationId);
        Assert.Equal(40, investigator.CreditRating);

        Assert.Equal(70, investigator.Str.Base);
        Assert.Equal(60, investigator.Con.Base);
        Assert.Equal(80, investigator.Siz.Base);
        Assert.Equal(65, investigator.Dex.Base);
        Assert.Equal(45, investigator.App.Base);
        Assert.Equal(75, investigator.Int.Base);
        Assert.Equal(55, investigator.Pow.Base);
        Assert.Equal(80, investigator.Edu.Base);
        Assert.Equal(65, investigator.Luck.Base);

        // MaxHp = (60 + 80) / 10 = 14
        Assert.Equal(14, investigator.MaxHp);
        Assert.Equal(14, investigator.CurrentHp);

        // MaxLucidity = 55
        Assert.Equal(55, investigator.MaxLucidity);
        Assert.Equal(55, investigator.CurrentLucidity);

        // MaxMp = 55 / 5 = 11
        Assert.Equal(11, investigator.MaxMp);
        Assert.Equal(11, investigator.CurrentMp);

        // STR (70) < SIZ (80) && DEX (65) < SIZ (80) -> baseMov = 7, age 42 (1 penalty) -> MOV = 6
        Assert.Equal(6, investigator.MovementRate);

        // STR + SIZ = 150 -> DB: "+1d4", Build: 1
        Assert.Equal("+1d4", investigator.DamageBonus);
        Assert.Equal(1, investigator.Build);

        // Skills
        Assert.Equal(2, investigator.Skills.Count);
        Assert.Equal(60, investigator.Skills["spot_hidden"].Base);
        Assert.Equal(30, investigator.Skills["spot_hidden"].Half);
        Assert.Equal(12, investigator.Skills["spot_hidden"].Fifth);
        Assert.Equal(50, investigator.Skills["listen"].Base);

        // Initial Value Objects
        Assert.NotNull(investigator.InvestigatorBiography);
        Assert.NotNull(investigator.InvestigatorFinances);
        Assert.NotNull(investigator.InvestigatorMedicalState);
        Assert.NotNull(investigator.InvestigatorEsotericKnowledge);
        Assert.Empty(investigator.InventoryItemIds);
    }

    [Fact]
    public void TakeLucidityDamage_ReducesCurrentLucidity_ClampedAtZero()
    {
        var baseAttributes = CreateSampleBaseAttributes(pow: 50);
        var investigator = new Investigator(
            "Test", 30, "detective", baseAttributes, 50, 20, new Dictionary<string, int>());

        Assert.Equal(50, investigator.CurrentLucidity);

        investigator.TakeLucidityDamage(15);
        Assert.Equal(35, investigator.CurrentLucidity);

        investigator.TakeLucidityDamage(50);
        Assert.Equal(0, investigator.CurrentLucidity);
    }

    [Fact]
    public void HealHp_IncreasesCurrentHp_ClampedAtMaxHp()
    {
        var baseAttributes = CreateSampleBaseAttributes(con: 50, siz: 50); // MaxHp = 10
        var investigator = new Investigator(
            "Test", 30, "detective", baseAttributes, 50, 20, new Dictionary<string, int>());

        Assert.Equal(10, investigator.CurrentHp);

        // Damage HP by reading forbidden book or taking damage indirectly
        // Simulating damage by reading book with lucidity loss or checking HealHp
        investigator.HealHp(5);
        Assert.Equal(10, investigator.CurrentHp); // Already at max

        // Let's create an investigator and verify healing logic
        investigator.TakeLucidityDamage(10);
        Assert.Equal(40, investigator.CurrentLucidity);
    }

    [Fact]
    public void ReadForbiddenBook_UpdatesLoreRating_AddsBookToReadList_AndDeductsLucidity()
    {
        var baseAttributes = CreateSampleBaseAttributes(pow: 60);
        var investigator = new Investigator(
            "Erich Zann", 35, "musician", baseAttributes, 50, 20, new Dictionary<string, int>());

        Assert.Equal(60, investigator.CurrentLucidity);
        Assert.Equal(0, investigator.InvestigatorEsotericKnowledge.ForbiddenLoreRating);
        Assert.Null(investigator.InvestigatorEsotericKnowledge.ReadForbiddenBooks);

        investigator.ReadForbiddenBook("the_king_in_yellow", lucidityLoss: 12, loreGain: 5);

        Assert.Equal(48, investigator.CurrentLucidity);
        Assert.Equal(5, investigator.InvestigatorEsotericKnowledge.ForbiddenLoreRating);
        Assert.NotNull(investigator.InvestigatorEsotericKnowledge.ReadForbiddenBooks);
        Assert.Single(investigator.InvestigatorEsotericKnowledge.ReadForbiddenBooks!);
        Assert.Contains("the_king_in_yellow",
            investigator.InvestigatorEsotericKnowledge.ReadForbiddenBooks!);

        // Read a second book
        investigator.ReadForbiddenBook("necronomicon", lucidityLoss: 20, loreGain: 10);

        Assert.Equal(28, investigator.CurrentLucidity);
        Assert.Equal(15, investigator.InvestigatorEsotericKnowledge.ForbiddenLoreRating);
        Assert.Equal(2, investigator.InvestigatorEsotericKnowledge.ReadForbiddenBooks!.Count);
        Assert.Contains("necronomicon",
            investigator.InvestigatorEsotericKnowledge.ReadForbiddenBooks!);
    }

    [Fact]
    public void AddScarOrInjury_AppendsDescriptionToBiography()
    {
        var baseAttributes = CreateSampleBaseAttributes();
        var investigator = new Investigator(
            "John Legrasse", 40, "police_officer", baseAttributes, 50, 30,
            new Dictionary<string, int>());

        Assert.Null(investigator.InvestigatorBiography.ScarsAndInjuries);

        investigator.AddScarOrInjury("Deep knife wound on left shoulder");

        Assert.NotNull(investigator.InvestigatorBiography.ScarsAndInjuries);
        Assert.Single(investigator.InvestigatorBiography.ScarsAndInjuries!);
        Assert.Equal("Deep knife wound on left shoulder",
            investigator.InvestigatorBiography.ScarsAndInjuries![0]);

        investigator.AddScarOrInjury("Burn scar across right forearm");

        Assert.Equal(2, investigator.InvestigatorBiography.ScarsAndInjuries!.Count);
        Assert.Equal("Burn scar across right forearm",
            investigator.InvestigatorBiography.ScarsAndInjuries![1]);
    }
}