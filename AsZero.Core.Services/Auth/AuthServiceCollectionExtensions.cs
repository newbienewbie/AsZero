using AsZero.Core.Services.Auth;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AsZero.Core.Services.Auth
{
    public static class AuthServiceCollectionExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher, DefaultPasswordHasher>();
            services.AddScoped<IUserManager, DefaultUserManager>();
            services.AddSingleton<IPrincipalAccessor, ClaimsPrincipalAccessor>();
            return services;
        }
    }
}
