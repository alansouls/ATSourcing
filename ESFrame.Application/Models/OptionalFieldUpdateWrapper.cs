using System.Diagnostics.CodeAnalysis;

namespace ESFrame.Application.Models;

public class NullableFieldUpdateWrapper<T>
{
    [MemberNotNullWhen(true, nameof(Value))]
    public bool IsUpdated { get; set; }

    public T? Value { get; set; }
}
