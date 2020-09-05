using DroneDelivery.Pagamento.Application.Dtos;
using DroneDelivery.Pagamento.Application.Interfaces;
using DroneDelivery.Pagamento.Data.Repositorios.Interfaces;
using DroneDelivery.Pagamento.Domain.Enums;
using DroneDelivery.Pagamento.Domain.Models;
using System.Threading.Tasks;

namespace DroneDelivery.Pagamento.Application.Services
{
    public class PedidoPagamentoService : IPedidoPagamentoService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PedidoPagamentoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task RealizarPagamento(CriarPedidoPagamentoDto criarPedidoPagamentoDto)
        {
            //validacoes do DTO


            var pagamento = new PedidoPagamento(
                criarPedidoPagamentoDto.PedidoId,
                criarPedidoPagamentoDto.Valor,
                criarPedidoPagamentoDto.NumeroCartao,
                criarPedidoPagamentoDto.VencimentoCartao,
                criarPedidoPagamentoDto.CodigoSeguranca);

            var status = pagamento.ValidarCartao() ? PagamentoStatus.Aprovado : PagamentoStatus.Reprovado;
            pagamento.AtualizarStatus(status);

            // publicar uma resposta



            await _unitOfWork.PedidoPagamentos.AdicionarAsync(pagamento);
            await _unitOfWork.SaveAsync();
        }
    }
}
