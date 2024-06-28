using System.Data;

namespace Jbi.UnitOfWork.Abstractions;

public interface ICommandBase
{
	/// <summary>
	/// Indicates whether the command requires a transaction to be executed.
	/// </summary>
	bool RequiresTransaction { get; }

	TimeSpan Timeout { get; }
}

public interface ICommand : ICommandBase
{
	void Execute(IDbConnection connection, IDbTransaction? transaction = null);
}

public interface ICommand<out T> : ICommandBase
{
	T? Execute(IDbConnection connection, IDbTransaction? transaction = null);
}

public interface IAsyncCommand : ICommandBase
{
	Task ExecuteAsync(IDbConnection connection, IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
}

public interface IAsyncCommand<T> : ICommandBase
{
	Task<T?> ExecuteAsync(IDbConnection connection, IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
}