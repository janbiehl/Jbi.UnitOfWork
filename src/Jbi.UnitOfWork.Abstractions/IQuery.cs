using System.Data;

namespace Jbi.UnitOfWork.Abstractions;

public interface IQuery<out T>
{
	T Execute(IDbConnection connection, IDbTransaction? transaction = null);
}

public interface IAsyncQuery<T>
{
	Task<T> ExecuteAsync(IDbConnection connection, IDbTransaction? transaction = null, CancellationToken cancellationToken = default);
}