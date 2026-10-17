using TheKingInYellow.Core.Interfaces;

namespace TheKingInYellow.Core.Services;

public static class AgeModifierEngine
{
    public static int PerformEduImprovementCheck(int currentEdu, IDiceRoller diceRoller)
    {
        var roll = diceRoller.RollD100();

        if (roll <= currentEdu && roll <= 95) return currentEdu;
        var gain = diceRoller.RollD10();
        return Math.Min(99, currentEdu + gain);

    }

    public static (int Str, int Con, int Dex, int App, int Edu, int Siz) ApplyAgeAdjustments(
        int age, int str, int con, int dex, int app, int edu, int siz, IDiceRoller diceRoller)
    {
        if (age <= 19)
        {
            str = Math.Max(15, str - 5);
            edu = Math.Max(15, edu - 5);
            return (str, con, dex, app, edu, siz);
        }

        if (age <= 39)
        {
            edu = PerformEduImprovementCheck(edu, diceRoller);
            return (str, con, dex, app, edu, siz);
        }

        int eduChecks = age switch
        {
            <= 49 => 2,
            <= 59 => 3,
            _ => 4
        };

        for (var i = 0; i < eduChecks; i++)
        {
            edu = PerformEduImprovementCheck(edu, diceRoller);
        }

        int physicalDeduction = age switch
        {
            <= 49 => 5,
            <= 59 => 10,
            <= 69 => 20,
            <= 79 => 40,
            _ => 80
        };

        int appDeduction = age switch
        {
            <= 49 => 5,
            <= 59 => 10,
            <= 69 => 15,
            <= 79 => 20,
            _ => 25
        };
        
        app = Math.Max(15, app - appDeduction);

        while (physicalDeduction > 0)
        {
            if (str >= con && str >= dex && str > 15)
            {
                str -= 5;
            }
            else if (con >= dex && con > 15)
            {
                con -= 5;
            }
            else if (dex > 15)
            {
                dex -= 5;

            }
            else break;
            
            physicalDeduction -= 5;
        }
        
        return (str, con, dex, app, edu, siz);
    }
}