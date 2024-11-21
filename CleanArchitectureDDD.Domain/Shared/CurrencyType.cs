namespace CleanArchitectureDDD.Domain.Entities.Shared;

public record CurrencyType
{
    public static CurrencyType Usd = new("USD");
    public static CurrencyType Eur = new("EUR");
    public static CurrencyType None = new("");
    private CurrencyType(string code) => Code = code;
    public string? Code { get; init; }
    
    public static readonly IReadOnlyCollection<CurrencyType> All = new List<CurrencyType>
    {
        Usd,
        Eur
    }.AsReadOnly();
    
    public static CurrencyType FromCode(string code) => All.FirstOrDefault(x => x.Code == code) ?? 
                                                        throw new ApplicationException($"CurrencyType with code {code} not found");
}