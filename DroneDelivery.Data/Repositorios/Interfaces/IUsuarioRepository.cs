using DroneDelivery.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DroneDelivery.Data.Repositorios.Interfaces
{
    public interface IUsuarioRepository : IRepository<Pedido>
    {
        Task<IEnumerable<Usuario>> ObterTodosAsync();

        Task<Usuario> ObterPorIdAsync(Guid id);

        Task<Usuario> ObterPorEmailAsync(string email);

        Task AdicionarAsync(Usuario usuario);

    }
}
