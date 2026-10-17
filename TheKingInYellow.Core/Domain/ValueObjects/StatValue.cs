namespace TheKingInYellow.Core.Domain.ValueObjects;

public readonly record struct StatValue(int Base)
{
    public int Half => Base / 2;
    public int Fifth => Base / 5;
}