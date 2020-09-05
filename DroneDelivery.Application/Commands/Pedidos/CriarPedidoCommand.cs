using DroneDelivery.Domain.Core.Commands;
using Flunt.Validations;
using Newtonsoft.Json;
using System;

namespace DroneDelivery.Application.Commands.Pedidos
{
    public class CriarPedidoCommand : Command
    {
        public double Peso { get; }

        public double Valor { get; set; }

        //public string NumeroCartao { get; set; }

        //public DateTime VencimentoCartao { get; set; }

        //public int CodigoSeguranca { get; set; }


        [JsonConstructor]
        public CriarPedidoCommand(double peso)
        {
            Peso = peso;
        }

        public void Validate()
        {

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Peso, 0, nameof(Peso), "O Peso tem que ser maior que zero"));

            AddNotifications(new Contract()
                .Requires()
                .IsGreaterThan(Valor, 0, nameof(Valor), "O Valor tem que ser maior que zero"));

            AddNotifications(new Contract()
                .Requires()
                .IsLowerOrEqualsThan(Peso, Utility.Utils.CAPACIDADE_MAXIMA_GRAMAS, nameof(Peso), $"O Peso tem que ser menor ou igual a {Utility.Utils.CAPACIDADE_MAXIMA_GRAMAS / 1000} KGs"));

            //AddNotifications(new Contract()
            //    .Requires()
            //    .IsGreaterThan(Valor, 0, nameof(Valor), "O Valor tem que ser maior que zero"));

            //AddNotifications(new Contract()
            //    .Requires()
            //    .IsNotNullOrEmpty(NumeroCartao, nameof(NumeroCartao), "O Número Cartão não pode ser vazio"));

            //AddNotifications(new Contract()
            //    .Requires()
            //    .IsNotNull(VencimentoCartao, nameof(VencimentoCartao), "O Vencimento Cartão não pode ser vazio"));

            //AddNotifications(new Contract()
            //    .Requires()
            //    .IsGreaterThan(CodigoSeguranca, 0, nameof(CodigoSeguranca), "O Código de Segurança tem que ser maior que zero"));


        }
    }
}
