using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsZero.Core.Services.HostedServices;
using AsZero.DbContexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AsZero.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region infra
            services.AddLogging(logging => {
                logging.AddLog4Net();
            });
            services.AddDbContext<AsZeroDbContext>(opts => {
                opts.UseMySql(Configuration.GetConnectionString("AsZeroDbContext"), builder =>{
                    var thisAssembly =typeof(Startup).Assembly;
                    builder.MigrationsAssembly(thisAssembly.GetName().Name);
                });
            });
            services.AddAsZeroHostedServices();
            #endregion

            #region web
            services.AddControllersWithViews()
                .AddNewtonsoftJson(opts =>{
                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            services.AddSignalR()
                .AddNewtonsoftJsonProtocol(opts => {
                    opts.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
            // Forwarded headers
            services.Configure<ForwardedHeadersOptions>(opts => {
                opts.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost;
                opts.KnownNetworks.Clear();
                opts.KnownProxies.Clear();
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseForwardedHeaders();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
