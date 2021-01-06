using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsZero.Core.Services.HostedServices
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundWorkItem(Func<IServiceProvider, CancellationToken, Task> workItem);

        Task<Func<IServiceProvider, CancellationToken, Task>> DequeueAsync( CancellationToken cancellationToken);
    }


}
