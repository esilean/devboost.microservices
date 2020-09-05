using DroneDelivery.Domain.Core.Mediator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DroneDelivery.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private IMediatorHandler _mediator;
        protected IMediatorHandler Mediator => _mediator ?? (_mediator = HttpContext.RequestServices.GetService<IMediatorHandler>());

    }
}
