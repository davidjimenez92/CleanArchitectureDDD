namespace CleanArchitectureDDD.Domain.Shared;

public record Currency(decimal Price, CurrencyType CurrencyType)
{
    public static Currency operator +(Currency c1, Currency c2)
    {
        if (c1.CurrencyType != c2.CurrencyType)
        {
            throw new InvalidOperationException("Cannot add two different currencies");
        }

        return new Currency(c1.Price + c2.Price, c1.CurrencyType);
    }
    
    public static Currency Zero() => new(0, CurrencyType.None);
    public static Currency Zero(CurrencyType currencyType) => new(0, currencyType);
    public bool IsZero() => this == Zero(CurrencyType);
}