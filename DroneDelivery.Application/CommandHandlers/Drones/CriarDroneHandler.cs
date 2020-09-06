using AutoMapper;
using DroneDelivery.Application.Commands.Drones;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Validator;
using DroneDelivery.Shared.Utility;
using DroneDelivery.Shared.Utility.Messages;
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

            if (request.Capacidade > Utils.CAPACIDADE_MAXIMA_GRAMAS)
            {
                _response.AddNotification(new Notification("drone", Erros.ErroDrone_CapacidadeMaxima));
                return _response;
            }

            var drone = _mapper.Map<Drone>(request);

            await _unitOfWork.Drones.AdicionarAsync(drone);
            await _unitOfWork.SaveAsync();

            return _response;
        }
    }
}
