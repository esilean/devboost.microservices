using AutoMapper;
using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Core.Domain;
using DroneDelivery.Domain.Core.Validator;
using DroneDelivery.Domain.Models;
using Flunt.Notifications;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.CommandHandlers.Drones
{
    public class CriarDroneHandler : ValidatorResponse, IRequestHandler<CriarDroneCommand, ResponseResult>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CriarDroneHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<ResponseResult> Handle(CriarDroneCommand request, CancellationToken cancellationToken)
        {
            request.Validate();
            if (request.Notifications.Any())
            {
                _response.AddNotifications(request.Notifications);
                return _response;
            }

            if (request.Capacidade > Utility.Utils.CAPACIDADE_MAXIMA_GRAMAS)
            {
                _response.AddNotification(new Notification("drone", $"capacidade do drone não pode ser maior que {Utility.Utils.CAPACIDADE_MAXIMA_GRAMAS / 1000} KGs"));
                return _response;
            }

            var drone = _mapper.Map<Drone>(request);

            await _unitOfWork.Drones.AdicionarAsync(drone);
            await _unitOfWork.SaveAsync();

            return _response;
        }
    }
}
