namespace Jbi.UnitOfWork.Abstractions;

public interface IUnitOfWork : IDisposable
{
	T Query<T>(IQuery<T> query);
	Task<T> QueryAsync<T>(IAsyncQuery<T> query, CancellationToken cancellationToken = default);
	void Execute(ICommand command);
	Task ExecuteAsync(IAsyncCommand command, CancellationToken cancellationToken = default);
	T? Execute<T>(ICommand<T?> command);
	Task<T?> ExecuteAsync<T>(IAsyncCommand<T> command, CancellationToken cancellationToken = default);
	void Commit();
	void Rollback();
}