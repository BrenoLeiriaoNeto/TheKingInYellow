using TheKingInYellow.Core.Domain.ValueObjects;
using TheKingInYellow.Core.Services;
using Xunit;

namespace TheKingInYellow.Core.Tests;

public class OccupationValidatorTests
{
    private readonly List<SkillDefinition> _allSkills =
    [
        new("accounting", "Accounting", 5, "Academic", "", Group: null),
        new("anthropology", "Anthropology", 1, "Academic", "", Group: null),
        new("history", "History", 5, "Academic", "", Group: null),
        new("library_use", "Library Use", 20, "Academic", "", Group: null),
        new("charm", "Charm", 15, "Social", "", Group: "interpersonal"),
        new("fast_talk", "Fast Talk", 5, "Social", "", Group: "interpersonal"),
        new("persuade", "Persuade", 10, "Social", "", Group: "interpersonal"),
        new("intimidate", "Intimidate", 15, "Social", "", Group: "interpersonal"),
        new("physics", "Physics", 1, "Academic", "", Group: "science"),
        new("chemistry", "Chemistry", 1, "Academic", "", Group: "science"),
        new("biology", "Biology", 1, "Academic", "", Group: "science"),
        new("painting", "Painting", 5, "Art", "", Group: "art_and_labor"),
        new("latin", "Latin", 1, "Academic", "", Group: "languages"),
        new("german", "German", 1, "Academic", "", Group: "languages"),
        new("spot_hidden", "Spot Hidden", 25, "Awareness", "", Group: null),
        new("listen", "Listen", 20, "Awareness", "", Group: null),
        new("stealth", "Stealth", 20, "Physical", "", Group: null),
        new("locksmith", "Locksmith", 1, "Technical", "", Group: null),
        new("drive_auto", "Drive Auto", 20, "Practical", "", Group: null),
        new("credit_rating", "Credit Rating", 0, "Social", "", Group: null)
    ];

    private static OccupationDefinition CreateOccupation(params string[] skillSlots)
    {
        return new OccupationDefinition(
            Id: "test_occupation",
            Name: "Test Occupation",
            Description: "Test Description",
            CreditRatingMin: 10,
            CreditRatingMax: 50,
            Formula: new OccupationFormula("EDU", 4, [], 0),
            SkillIds: skillSlots.ToList(),
            SuggestedContacts: []
        );
    }

    [Fact]
    public void ValidateSkillSelection_LessThan8Skills_ReturnsFalse()
    {
        var occupation = CreateOccupation("accounting", "anthropology", "history", "library_use", "spot_hidden", "listen", "stealth", "ANY");
        var chosen = new[] { "accounting", "anthropology", "history", "library_use", "spot_hidden", "listen", "stealth" }; // 7 skills

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.False(result);
    }

    [Fact]
    public void ValidateSkillSelection_MoreThan8Skills_ReturnsFalse()
    {
        var occupation = CreateOccupation("accounting", "anthropology", "history", "library_use", "spot_hidden", "listen", "stealth", "ANY");
        var chosen = new[] { "accounting", "anthropology", "history", "library_use", "spot_hidden", "listen", "stealth", "locksmith", "german" }; // 9 skills

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.False(result);
    }

    [Fact]
    public void ValidateSkillSelection_DuplicateChosenSkills_ReturnsFalse()
    {
        var occupation = CreateOccupation("accounting", "anthropology", "history", "library_use", "spot_hidden", "listen", "stealth", "ANY");
        var chosen = new[] { "accounting", "accounting", "history", "library_use", "spot_hidden", "listen", "stealth", "locksmith" }; // duplicate accounting

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.False(result);
    }

    [Fact]
    public void ValidateSkillSelection_ExactMatchingDirectSkills_ReturnsTrue()
    {
        var occupation = CreateOccupation(
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "locksmith");

        var chosen = new[]
        {
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "locksmith"
        };

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.True(result);
    }

    [Fact]
    public void ValidateSkillSelection_WithChoiceToken_ValidChoice_ReturnsTrue()
    {
        var occupation = CreateOccupation(
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "CHOICE:locksmith|drive_auto");

        var chosen = new[]
        {
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "drive_auto"
        };

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.True(result);
    }

    [Fact]
    public void ValidateSkillSelection_WithChoiceToken_InvalidChoice_ReturnsFalse()
    {
        var occupation = CreateOccupation(
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "CHOICE:locksmith|drive_auto");

        // "painting" is not in the choice token
        var chosen = new[]
        {
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "painting"
        };

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.False(result);
    }

    [Fact]
    public void ValidateSkillSelection_WithGroupToken_ValidGroupSkill_ReturnsTrue()
    {
        var occupation = CreateOccupation(
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "CHOICE_INTERPERSONAL");

        // "persuade" is in "interpersonal" group
        var chosen = new[]
        {
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "persuade"
        };

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.True(result);
    }

    [Fact]
    public void ValidateSkillSelection_WithGroupToken_InvalidGroupSkill_ReturnsFalse()
    {
        var occupation = CreateOccupation(
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "CHOICE_INTERPERSONAL");

        // "physics" is in "science" group, not "interpersonal"
        var chosen = new[]
        {
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "physics"
        };

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.False(result);
    }

    [Fact]
    public void ValidateSkillSelection_WithAnyToken_AnyValidSkill_ReturnsTrue()
    {
        var occupation = CreateOccupation(
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "ANY");

        var chosen = new[]
        {
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "german"
        };

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.True(result);
    }

    [Fact]
    public void ValidateSkillSelection_ComplexCombinationOfSlots_ReturnsTrueWhenValid()
    {
        var occupation = CreateOccupation(
            "history", "library_use",
            "CHOICE_LANGUAGES",
            "CHOICE_SCIENCE",
            "CHOICE_INTERPERSONAL",
            "CHOICE:locksmith|stealth",
            "ANY", "ANY"
        );

        var chosen = new[]
        {
            "history", "library_use",
            "latin",       // CHOICE_LANGUAGES
            "chemistry",   // CHOICE_SCIENCE
            "charm",       // CHOICE_INTERPERSONAL
            "locksmith",   // CHOICE:locksmith|stealth
            "drive_auto",  // ANY
            "accounting"   // ANY
        };

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.True(result);
    }

    [Fact]
    public void ValidateSkillSelection_UnknownSkillIdInChosen_ReturnsFalseWhenNotAnyOrDirect()
    {
        var occupation = CreateOccupation(
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "CHOICE_SCIENCE");

        var chosen = new[]
        {
            "accounting", "anthropology", "history", "library_use",
            "spot_hidden", "listen", "stealth", "unknown_skill_id"
        };

        var result = OccupationValidator.ValidateSkillSelection(occupation, chosen, _allSkills);

        Assert.False(result);
    }
}
