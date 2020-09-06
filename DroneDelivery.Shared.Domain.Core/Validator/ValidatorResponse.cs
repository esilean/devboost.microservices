using DroneDelivery.Shared.Domain.Core.Domain;

namespace DroneDelivery.Shared.Domain.Core.Validator
{
    public class ValidatorResponse
    {
        protected readonly ResponseResult _response;

        public ValidatorResponse()
        {
            _response = new ResponseResult();
        }
    }
}
