using AutoMapper;
using DroneDelivery.Application.Dtos.Usuario;
using DroneDelivery.Application.Queries.Usuarios;
using DroneDelivery.Data.Repositorios.Interfaces;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Domain;
using DroneDelivery.Shared.Domain.Core.Validator;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DroneDelivery.Application.QueryHandlers.Usuarios
{
    public class ListarUsuariosHandler : ValidatorResponse, IRequestHandler<UsuariosQuery, ResponseResult>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ListarUsuariosHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseResult> Handle(UsuariosQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _unitOfWork.Usuarios.ObterTodosAsync();

            _response.AddValue(new
            {
                usuarios = _mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioDto>>(usuarios)
            });

            return _response;
        }
    }
}
