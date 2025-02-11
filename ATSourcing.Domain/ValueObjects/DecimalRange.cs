namespace ATSourcing.Domain.ValueObjects;

public record DecimalRange(decimal Min, decimal Max)
{
    public override string ToString() => $"{Min}-{Max}";

    public static DecimalRange Parse(string value)
    {
        var parts = value.Split('-');
        if (parts.Length != 2)
        {
            throw new FormatException("Invalid range format");
        }
        return new DecimalRange(decimal.Parse(parts[0]), decimal.Parse(parts[1]));
    }
}
