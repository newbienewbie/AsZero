using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace AsZero.Core.Services.HostedServices
{
    public static class HostedServiceExtensions
    {
        public static IServiceCollection AddAsZeroHostedServices(this IServiceCollection services)
        {
            services.AddSingleton<IBackgroundTaskQueue, DefaultBackgroundTaskQueue>();
            services.AddHostedService<QueuedHostedService>();
            return services;
        }
    }
}
