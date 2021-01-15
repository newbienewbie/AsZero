using AsZero.Wpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsZero.Wpf.Views
{
    public static class ViewsServiceCollectionExtensions
    {
        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddSingleton<RouterViewModel>();
            services.AddSingleton<LoginPageViewModel>();
            services.AddSingleton<LoginWindow>();

            services.AddSingleton<MainWindow>();
            return services;
        }
    }
}
