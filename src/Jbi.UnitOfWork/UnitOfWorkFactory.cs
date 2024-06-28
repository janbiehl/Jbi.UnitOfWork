using System.Data;
using System.Data.Common;
using Jbi.UnitOfWork.Abstractions;

namespace Jbi.UnitOfWork;

public sealed class UnitOfWorkFactory<TConnection> : IUnitOfWorkFactory where TConnection : DbConnection, new()
{
	private readonly string _connectionString;

	public UnitOfWorkFactory(string connectionString)
	{
		_connectionString = connectionString;
	}

	/// <inheritdoc />
	public IUnitOfWork Create(bool transactional, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted)
	{
		TConnection connection = new()
		{
			ConnectionString = _connectionString
		};
		
		connection.Open();
		return new UnitOfWork(connection, transactional, isolationLevel);
	}

	/// <inheritdoc />
	public async Task<IUnitOfWork> CreateAsync(bool transactional, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted,
		CancellationToken cancellationToken = default)
	{
		TConnection connection = new()
		{
			ConnectionString = _connectionString
		};

		await connection.OpenAsync(cancellationToken);
		
		return new UnitOfWork(connection, transactional, isolationLevel);
	}
}