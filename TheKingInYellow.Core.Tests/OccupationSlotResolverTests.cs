using TheKingInYellow.Core.Domain.ValueObjects;
using TheKingInYellow.Core.Services;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class OccupationSlotResolverTests
{
    private readonly List<SkillDefinition> _sampleSkills =
    [
        new("charm", "Charm", 15, "Social", "", Group: "interpersonal"),
        new("fast_talk", "Fast Talk", 5, "Social", "", Group: "interpersonal"),
        new("persuade", "Persuade", 10, "Social", "", Group: "interpersonal"),
        new("intimidate", "Intimidate", 15, "Social", "", Group: "interpersonal"),
        new("physics", "Physics", 1, "Academic", "", Group: "science"),
        new("chemistry", "Chemistry", 1, "Academic", "", Group: "science"),
        new("biology", "Biology", 1, "Academic", "", Group: "science"),
        new("painting", "Painting", 5, "Art", "", Group: "art_and_labor"),
        new("latin", "Latin", 1, "Academic", "", Group: "languages"),
        new("spot_hidden", "Spot Hidden", 25, "Awareness", "", Group: null),
        new("stealth", "Stealth", 20, "Physical", "", Group: null)
    ];

    [Theory]
    [InlineData("CHOICE_INTERPERSONAL", true)]
    [InlineData("CHOICE_SCIENCE", true)]
    [InlineData("CHOICE_ART_AND_LABOR", true)]
    [InlineData("CHOICE_LANGUAGES", true)]
    [InlineData("CHOICE:stealth|disguise", true)]
    [InlineData("ANY", true)]
    [InlineData("spot_hidden", false)]
    [InlineData("listen", false)]
    [InlineData("", false)]
    public void IsWildcardToken_ReturnsExpectedResult(string token, bool expected)
    {
        var result = OccupationSlotResolver.IsWildcardToken(token);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetValidOptionsForToken_ChoicePrefix_ReturnsSplittedOptions()
    {
        var token = "CHOICE:stealth|disguise|locksmith";
        var result = OccupationSlotResolver.GetValidOptionsForToken(token, _sampleSkills);

        Assert.Equal(3, result.Count);
        Assert.Contains("stealth", result);
        Assert.Contains("disguise", result);
        Assert.Contains("locksmith", result);
    }

    [Fact]
    public void GetValidOptionsForToken_AnyToken_ReturnsAllSkillIds()
    {
        var result = OccupationSlotResolver.GetValidOptionsForToken("ANY", _sampleSkills);

        Assert.Equal(_sampleSkills.Count, result.Count);
        foreach (var skill in _sampleSkills)
        {
            Assert.Contains(skill.Id, result);
        }
    }

    [Theory]
    [InlineData("CHOICE_INTERPERSONAL", new[] { "charm", "fast_talk", "persuade", "intimidate" })]
    [InlineData("CHOICE_SCIENCE", new[] { "physics", "chemistry", "biology" })]
    [InlineData("CHOICE_ART_AND_LABOR", new[] { "painting" })]
    [InlineData("CHOICE_LANGUAGES", new[] { "latin" })]
    public void GetValidOptionsForToken_GroupTokens_ReturnsSkillsMatchingGroup(string token, string[] expectedSkillIds)
    {
        var result = OccupationSlotResolver.GetValidOptionsForToken(token, _sampleSkills);

        Assert.Equal(expectedSkillIds.Length, result.Count);
        foreach (var id in expectedSkillIds)
        {
            Assert.Contains(id, result);
        }
    }

    [Fact]
    public void GetValidOptionsForToken_DirectSkillId_ReturnsSingleElementList()
    {
        var result = OccupationSlotResolver.GetValidOptionsForToken("spot_hidden", _sampleSkills);

        Assert.Single(result);
        Assert.Equal("spot_hidden", result[0]);
    }
}
