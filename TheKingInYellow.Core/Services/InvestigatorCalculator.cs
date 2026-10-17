namespace TheKingInYellow.Core.Services;

public static class InvestigatorCalculator
{
    public static int CalculateMaxHp(int con, int siz) => (con + siz) / 10;
    public static int CalculateMaxLucidity(int pow) => pow;
    public static int CalculateMaxMp(int pow) => pow / 5;
    
    public static (string DamageBonus, int Build) CalculateDamageBonusAndBuild(int str, int siz)
    {
        var sum = str + siz;
        return sum switch
        {
            <= 64 => ("-2", -2),
            <= 84 => ("-1", -1),
            <= 124 => ("+0", 0),
            <= 164 => ("+1d4", 1),
            <= 204 => ("+1d6", 2),
            _ => ("+2d6", 3)
        };
    }

    public static int CalculateMovementRate(int str, int dex, int siz, int age)
    {
        int baseMov;

        if (dex < siz && str < siz)
            baseMov = 7;
        else if (str > siz && dex > siz)
            baseMov = 9;
        else
            baseMov = 8;

        if (age < 40) return Math.Max(1, baseMov);
        
        var decadesAfter40 = (age - 40) / 10;
        var agePenalty = 1 + decadesAfter40;
        baseMov -= agePenalty;

        return Math.Max(1, baseMov);
    }
}