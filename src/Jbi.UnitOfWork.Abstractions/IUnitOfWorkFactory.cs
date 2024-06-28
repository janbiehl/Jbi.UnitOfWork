using System.Data;

namespace Jbi.UnitOfWork.Abstractions;

public interface IUnitOfWorkFactory
{
	IUnitOfWork Create(bool transactional, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted);
	Task<IUnitOfWork> CreateAsync(bool transactional, IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
}