using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using dotnetcore.azFunction.AppShim;
using System.Net.Http;

namespace azFunc_logger
{
    public class Function1
    {
        private IFunctionsAppShim _functionsAppShim;
        public Function1(IFunctionsAppShim functionsAppShim)
        {
            _functionsAppShim = functionsAppShim;
        }

        bool _initialized = false;
        [FunctionName("Function1")]
        public async Task<HttpResponseMessage> Run(
            Microsoft.Azure.WebJobs.ExecutionContext context,
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", "put", "delete", Route = "{*all}")] HttpRequest request,
            ILogger logger)
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");
            if (!_initialized)
            {
                await _functionsAppShim.Initialize(logger);
                _initialized = true;
            }
            return await _functionsAppShim.Run(context, request);
        }
    }
}
