using AsZero.Core.Services;
using AsZero.Core.Services.Auth;
using AsZero.Wpf.Views;
using FutureTech.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using AsZero.Core.Services.Messages;
using System.Reflection;

namespace AsZero.Wpf
{
    public class HostStartup
    {
        public static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton(typeof(SettingsManager<>), typeof(SettingsManager<>));
            services.AddDbContext<DbContexts.AsZeroDbContext>(opts => {
                opts.UseMySql(context.Configuration.GetConnectionString("AsZeroDbContext"));
                if (context.HostingEnvironment.IsDevelopment())
                {
                    opts.EnableSensitiveDataLogging();
                }
            });
            services.AddOptions<DalOptions>().Configure(o => o.EnableOpsHisotry = false);
            //services.AddHistoryServices<CurrentUserIdProvider>(opts => {
            //    opts.UseMySql(Configuration.GetConnectionString("OpenAuthDBContext"));
            //});

            services.AddAuth();
            services.AddSingleton<IPrincipalAccessor, ClaimsPrincipalAccessor>();

            #region Message Handlers
            services.AddMediatR(typeof(LoginRequest).Assembly);
            #endregion

            services.AddViews();
        }
    }
}
