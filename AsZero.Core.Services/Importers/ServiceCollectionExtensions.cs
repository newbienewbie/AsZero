using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AsZero.Core.Services.Importers
{

    public static class ImportersServiceCollectionExtensions
    {

        public static IServiceCollection AddExcelService(this IServiceCollection services)
        {
            return services;
        }

    }

}
