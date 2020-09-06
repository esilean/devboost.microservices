using DroneDelivery.Shared.Domain.Core.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DroneDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IEventBus _eventBus;
        protected IEventBus EventBus => _eventBus ??= HttpContext.RequestServices.GetService<IEventBus>();

    }
}
