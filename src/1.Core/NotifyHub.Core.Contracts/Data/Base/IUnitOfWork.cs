namespace NotifyHub.Core.Contracts.Data.Base;

public interface IUnitOfWork
{
    void BeginTransaction();

    void CommitTransaction();

    Task CommitTransactionAsync();

    void RollbackTransaction();

    Task RollbackTransactionAsync();

    int Commit();

    Task<int> CommitAsync(CancellationToken cancellationToken = default);
}