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
using Contracts;
using System.Net.Http;
using System.Text;

namespace azFunc_logger
{
    public class Crud
    {
        private IFunctionsAppShim _functionsAppShim;
        private ISerializer _serializer;

        public Crud(IFunctionsAppShim functionsAppShim,ISerializer serializer)
        {
            _functionsAppShim = functionsAppShim;
            _serializer = serializer;
        }

        [FunctionName("Crud")]
        public async Task<IActionResult> Run(
            Microsoft.Azure.WebJobs.ExecutionContext context,
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "Crud")] HttpRequest req,
            ILogger logger)
        {
            if (!Globals.Initialized)
            {
                await _functionsAppShim.Initialize(logger);
                Globals.Initialized = true;
            }

            logger.LogInformation("C# HTTP trigger function processed a request.");
            var job = new Job
            {
                Id = Guid.NewGuid().ToString(),
                IssuedTime = DateTime.UtcNow,
                Name = "My SuperDuper Job"
            };
            var json = _serializer.Serialize(job);
            logger.LogInformation($"C# ServiceBus queue trigger function processed message: {json}");


            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/MyCrud")
            {
                Content = new StringContent(
                json,
                Encoding.UTF8, "application/json")
            };


            var response = await _functionsAppShim.SendAsync(context, httpRequestMessage);
            return new OkObjectResult(job);
        }
    }
}
