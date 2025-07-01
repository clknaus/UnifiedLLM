namespace Abstractions;

public class Models
{
}

public readonly struct SafeRef<T> where T : class
{
    private readonly T? _value;
    public SafeRef(T? value) => _value = value;

    public bool IsNull => _value == null;
    public bool IsNotNull => _value != null;

    public T? Value => _value;

    public static implicit operator T?(SafeRef<T> safe) => safe._value;
    public static implicit operator SafeRef<T>(T? value) => new(value);
}
