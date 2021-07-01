using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MainEnvironment.Core.Interfaces;
using MainEnvironment.Core.Models;
using MainEnvironment.Database;
using MainEnvironment.Web.Interfaces;
using MainEnvironment.Web.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MainEnvironment.Web
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
            services.AddControllers().AddNewtonsoftJson(s=>
            {
                s.SerializerSettings.Converters.Add(new AbstractTypeConverter<ILogModel, LogModel>());
            });

            services.AddTransient<IExperimentRepo, ExperimentRepo>();
            services.AddTransient<ILogRepo, LogRepo>();
            services.AddTransient<IInstructionsRepo, InstructionsRepo>();
            services.AddTransient<IConsentFormService, ConsentFormService>();
            services.AddTransient<ILeanLogService, LeanLogService>();
            services.AddTransient<IDownloadAppService, DownloadAppService>();
            services.AddTransient<IDataAnalysisLogService, DataAnalysisLogService>();
            services.AddTransient<ISecureTokenService, SecureTokenService>();

            services.AddDbContext<EnvironmentContext>(s => s.UseSqlServer(Configuration.GetConnectionString("ConnectionString"), options =>
            {
                options.MigrationsAssembly("MainEnvironment.Database");
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
