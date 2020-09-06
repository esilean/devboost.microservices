using AutoMapper;
using DroneDelivery.Application.Dtos.Drone;
using DroneDelivery.Application.Queries.Drones;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Validator;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.QueryHandlers.Drones
{
    public class ListarDronesHandler : ValidatorResponse, IRequestHandler<DronesQuery, ResponseResult>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ListarDronesHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseResult> Handle(DronesQuery request, CancellationToken cancellationToken)
        {
            var drones = await _unitOfWork.Drones.ObterTodosAsync();

            _response.AddValue(new
            {
                drones = _mapper.Map<IEnumerable<Drone>, IEnumerable<DroneDto>>(drones)
            });

            return _response;
        }
    }
}
