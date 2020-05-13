using Contracts.Extensions;
using dotnetcore.azFunction.AppShim;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

[assembly: FunctionsStartup(typeof(azFunc_logger.Startup))]
namespace azFunc_logger
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            builder.Services.AddSerializer();
            var functionsAppShim = new FunctionsAppShim<WebApplication_Startup_Logging.Startup>
            {
                LoadConfigurationsDelegate = WebApplication_Startup_Logging.Program.LoadConfigurations
            };
            builder.Services.AddSingleton<IFunctionsAppShim>(functionsAppShim);
        }
    }
}
