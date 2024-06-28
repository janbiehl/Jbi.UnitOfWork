using System.Data;
using Jbi.UnitOfWork.Abstractions;

namespace Jbi.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
	private bool _disposed;
	private IDbConnection _connection;
	private IDbTransaction? _transaction;
	
	/// <summary>
	/// Initializes a new instance of <see cref="UnitOfWork"/>.
	/// </summary>
	/// <remarks>Should not be used directly, use <see cref="UnitOfWorkFactory{TConnection}"/> instead.</remarks>
	/// <param name="connection"></param>
	/// <param name="transactional"></param>
	/// <param name="isolationLevel"></param>
	public UnitOfWork(IDbConnection connection, bool transactional, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
	{
		_connection = connection;
		if (transactional)
			_transaction = connection.BeginTransaction(isolationLevel);
	}

	/// <inheritdoc />
	public T Query<T>(IQuery<T> query) => query.Execute(_connection, _transaction);

	/// <inheritdoc />
	public Task<T> QueryAsync<T>(IAsyncQuery<T> query, CancellationToken cancellationToken = default) => query.ExecuteAsync(_connection, _transaction, cancellationToken);

	/// <inheritdoc />
	public void Execute(ICommand command)
	{
		if (command.RequiresTransaction && _transaction is null)
			throw new InvalidOperationException($"Command of type {command.GetType()} requires a transaction but none is present");
		
		command.Execute(_connection, _transaction);
	}

	/// <inheritdoc />
	public Task ExecuteAsync(IAsyncCommand command, CancellationToken cancellationToken = default)
	{
		if (command.RequiresTransaction && _transaction is null)
			throw new InvalidOperationException($"Command of type {command.GetType()} requires a transaction but none is present");
		
		return command.ExecuteAsync(_connection, _transaction, cancellationToken);
	}

	/// <inheritdoc />
	public T? Execute<T>(ICommand<T?> command)
	{
		if (command.RequiresTransaction && _transaction is null)
			throw new InvalidOperationException($"Command of type {command.GetType()} requires a transaction but none is present");
		
		return command.Execute(_connection, _transaction);
	}

	/// <inheritdoc />
	public Task<T?> ExecuteAsync<T>(IAsyncCommand<T> command, CancellationToken cancellationToken = default)
	{
		if (command.RequiresTransaction && _transaction is null)
			throw new InvalidOperationException($"Command of type {command.GetType()} requires a transaction but none is present");
		
		return command.ExecuteAsync(_connection, _transaction, cancellationToken);
	}

	/// <inheritdoc />
	public void Commit()
	{
		if (_transaction is null)
			throw new InvalidOperationException("No transaction to commit");
		
		_transaction.Commit();
	}

	/// <inheritdoc />
	public void Rollback()
	{
		if (_transaction is null)
			throw new InvalidOperationException("No transaction to rollback");
		
		_transaction.Rollback();
	}
	
	~UnitOfWork()
	{
		Dispose();
	}

	private void Dispose(bool disposing)
	{
		if (_disposed)
			return;

		if (disposing)
		{
			_transaction?.Dispose();
			_connection.Dispose();
		}
		
		_transaction = null;
		_connection = null!;
		_disposed = true;
	}
	
	/// <inheritdoc />
	public void Dispose()
	{
		Dispose(true);
		GC.SuppressFinalize(this);
	}
}