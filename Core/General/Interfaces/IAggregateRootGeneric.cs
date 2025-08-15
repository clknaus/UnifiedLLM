using Core.General.Models;

namespace Core.General.Interfaces;
public interface IAggregateRootGeneric<T> : IAggregateRoot
{
    Result<T> ValidateThenRaiseEvent();
}