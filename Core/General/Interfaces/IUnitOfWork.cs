namespace Core.General.Interfaces;
public interface IUnitOfWork
{
    Task<int> CommitAsync();
}
