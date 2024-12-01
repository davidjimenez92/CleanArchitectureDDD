namespace CleanArchitectureDDD.Domain.Shared;

public abstract class Enumeration<TEnum>: IEquatable<Enumeration<TEnum>> 
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();
    
    public int Id { get; protected init; }
    public string Name { get; protected init; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        if(other is null) return false;

        return GetType() == other.GetType() && Id.Equals(other.Id);
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);    
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();    
    }

    public override string ToString()
    {
        return Name;
    }

    public static TEnum? FromValue(int id)
    {
        return Enumerations.GetValueOrDefault(id);
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.FirstOrDefault(v => v.Name == name);
    }

    public static IEnumerable<TEnum> GetValues()
    {
        return Enumerations.Values.ToList();
    }

    public static Dictionary<int, TEnum> CreateEnumerations()
    {
        var type = typeof(TEnum);

        var fieldsForType = type.GetFields(
            System.Reflection.BindingFlags.Public | 
            System.Reflection.BindingFlags.Static |
            System.Reflection.BindingFlags.FlattenHierarchy
        ).Where(fieldInfo => type.IsAssignableFrom(fieldInfo.FieldType))
        .Select(fieldInfo => (TEnum)fieldInfo.GetValue(default)!);

        return fieldsForType.ToDictionary(x => x.Id);
    }
}