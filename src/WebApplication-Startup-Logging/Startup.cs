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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace WebApplication_Startup_Logging
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment _env;
        ILogger _logger;
        private Exception _deferedException;

        public Startup(IConfiguration configuration,IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
            _logger = new LoggerBuffered(LogLevel.Debug);
            _logger.LogInformation($"Create Startup {env.ApplicationName} - {env.EnvironmentName}");
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            try {
                _logger.LogInformation($"ConfigureServices...");
                services.AddControllers();
                _logger.LogInformation($"Just gonna throw an exception now in ConfigureService");
                throw new Exception("Ermaghd something went wrong!");
                // notice that IMySuperDuperService  never gets a chance to be known.  So Sad!
                services.AddSingleton<IMySuperDuperService, MySuperDuperService>();
            }
            catch (Exception ex)
            {
                // this will be thrown after we do some logging.
                _deferedException = ex;
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, 
            IServiceProvider serviceProvider, ILogger<Startup> logger)
        {
            try
            {
                // Fist thing, copy our in-memory logs to the real ILogger
                //----------------------------------------------------------------------
                (_logger as LoggerBuffered).CopyToLogger(logger);

                // Second thing.  We are done with the in-memory stuff, so assign _logger to the real ILogger.
                //----------------------------------------------------------------------
                _logger = logger;

                // Third thing.  Take care of that Defered Exception
                //----------------------------------------------------------------------
                if(_deferedException != null)
                {
                    // defered throw.
                    throw _deferedException;
                }

                _logger.LogInformation("Configure");

                // Fourth Thing. Only rely on IServiceProvider to get downstream services.
                //----------------------------------------------------------------------
                var mySuperDuperService = serviceProvider.GetRequiredService<IMySuperDuperService>();


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
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }
    }
}
