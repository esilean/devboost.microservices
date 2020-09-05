using DroneDelivery.Domain.Core.Domain;

namespace DroneDelivery.Domain.Core.Validator
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
