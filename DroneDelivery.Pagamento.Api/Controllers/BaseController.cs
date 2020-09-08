using DroneDelivery.Shared.Domain.Core.Bus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;

namespace DroneDelivery.Pagamento.Api.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {

        protected readonly IEventBus EventBus;

        public BaseController(IEventBus eventBus)
        {
            EventBus = eventBus;
        }

    }
}
