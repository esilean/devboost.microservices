using DroneDelivery.Shared.Domain.Core.Domain;
using Flunt.Notifications;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DroneDelivery.Api.Middeware
{

    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = new ResponseResult();
                response.AddNotification(new Notification("erro interno", (ex.InnerException != null) ? ex.InnerException.Message : ex.Message));

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var result = JsonSerializer.Serialize(
                    new
                    {
                        erros = response.Fails.Select(x => x.Message)
                    });

                await context.Response.WriteAsync(result);
            }
        }
    }
}
