using DroneDelivery.Shared.Domain.Core.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;

namespace DroneDelivery.Api.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IEventBus _eventBus;
        protected IEventBus EventBus => _eventBus ??= HttpContext.RequestServices.GetService<IEventBus>();

    }
}
