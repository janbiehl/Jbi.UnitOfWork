using System.Data;
using Jbi.UnitOfWork.Abstractions;
using Npgsql;

namespace Jbi.UnitOfWork.Npqsql;

public sealed class NpgsqlUnitOfWorkFactory : IUnitOfWorkFactory, IDisposable, IAsyncDisposable
{
	private readonly NpgsqlDataSource _dataSource;

	public NpgsqlUnitOfWorkFactory(string connectionString)
	{
		_dataSource = NpgsqlDataSource.Create(connectionString);
	}

	public IUnitOfWork Create(bool transactional = false, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
	{
		var connection = _dataSource.OpenConnection();
		return new UnitOfWork(connection, transactional, isolationLevel);
	}

	public async Task<IUnitOfWork> CreateAsync(
		bool transactional = false,
		IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
		CancellationToken cancellationToken = default)
	{
		var connection = await _dataSource.OpenConnectionAsync(cancellationToken);
		return new UnitOfWork(connection, transactional, isolationLevel);
	}

	/// <inheritdoc />
	public void Dispose()
	{
		_dataSource.Dispose();
	}

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		await _dataSource.DisposeAsync();
	}
}