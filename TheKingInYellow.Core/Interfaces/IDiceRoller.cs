using TheKingInYellow.Core.Services;

namespace TheKingInYellow.Core.Interfaces;

public interface IDiceRoller
{
    int RollD100();
    int RollD20();
    int RollD12();
    int RollD10();
    int RollD8();
    int RollD6();
    int RollD4();
    int RollD3();
}

public class RandomDiceRoller : IDiceRoller
{
    public int RollD100() => DiceRoller.RollD100();
    public int RollD20() => DiceRoller.RollD20();
    public int RollD12() => DiceRoller.RollD12();
    public int RollD10() => DiceRoller.RollD10();
    public int RollD8() => DiceRoller.RollD8();
    public int RollD6() => DiceRoller.RollD6();
    public int RollD4() => DiceRoller.RollD4();
    public int RollD3() => DiceRoller.RollD3();
}

public class FixedDiceRoller(
    int fixedD100 = 1,
    int fixedD20 = 1,
    int fixedD12 = 1,
    int fixedD10 = 1,
    int fixedD8 = 1,
    int fixedD6 = 1,
    int fixedD4 = 1,
    int fixedD3 = 1)
    : IDiceRoller
{
    public int RollD100() => fixedD100;
    public int RollD20() => fixedD20;
    public int RollD12() => fixedD12;
    public int RollD10() => fixedD10;
    public int RollD8() => fixedD8;
    public int RollD6() => fixedD6;
    public int RollD4() => fixedD4;
    public int RollD3() => fixedD3;
}