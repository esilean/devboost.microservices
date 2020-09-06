using DroneDelivery.Domain.Enum;
using DroneDelivery.Domain.Models;
using DroneDelivery.Shared.Domain.Core.Domain;
using Flunt.Notifications;
using Xunit;

namespace DroneDelivery.Domain.Tests.Domain.Core
{
    public class ResponseResultTests
    {

        [Fact(DisplayName = "Inserir objeto na classe de resposta")]
        public void ResponseResult_InserirObjeto_DeveRetornarObjetonoData()
        {
            //arrange
            var drone = Drone.Criar(12000, 3, 35, 100, DroneStatus.EmManutencao);

            //Act
            var response = new ResponseResult(drone);

            //Arrange
            Assert.True(((Drone)response.Data).Id == drone.Id);
        }

        [Fact(DisplayName = "Validar mensagens de erro ao converter objeto para string")]
        public void ResponseResult_InserirMensagemErro_DeveRetornarStringComMensagens()
        {
            //arrange
            var response = new ResponseResult();
            response.AddNotification(new Notification("teste", "teste de erro"));

            //Act
            var respAsstring = response.ToString();

            //Arrange
            Assert.Contains("teste de erro", respAsstring);
        }
    }
}
