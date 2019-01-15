using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Exceptions;
using Serilog.Events;
using Himsa.Samples.Models.Interface;
using Himsa.Samples.MemoryQueueProvider;
using Himsa.Samples.AzureStorageQueueProvider;
using Himsa.Samples.ConsolNoahEventHandler;
using Himsa.Samples.NoahEventProcessor;

namespace Himsa.Samples.WebHookEventReciever
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.{Environment.MachineName}.json", reloadOnChange: true, optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            var loggerConfig = new LoggerConfiguration()
                                    .Enrich.FromLogContext()
                                    .Enrich.WithExceptionDetails()
                                    .WriteTo.Console();

            Log.Logger = loggerConfig.CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.AddLogging();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Add our Config object so it can be injected
            var configurationSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(configurationSection);

            // Register the Services

            // The NoahMemoryQueue is ONLY usable for development, use the AzureStorageQueue provider for real systems or implement another Queue Provider
            services.AddSingleton<INoahEventQueue, NoahMemoryQueue>();

            //services.AddSingleton<INoahEventQueue, NoahAzureStorageQueue>();

            services.AddSingleton<INoahEventHandler, ConsoleEventHandler>();
            services.AddSingleton<INoahEventProcessor, NoahEventProcessor.NoahEventProcessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            var eventProcessor = app.ApplicationServices.GetRequiredService<INoahEventProcessor>();
            eventProcessor.Start();
        }
    }
}
