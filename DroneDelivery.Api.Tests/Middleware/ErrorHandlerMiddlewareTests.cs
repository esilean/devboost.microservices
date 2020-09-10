using DroneDelivery.Api.Middeware;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DroneDelivery.Api.Tests.Middleware
{
    public class ErrorHandlerMiddlewareTests
    {

        private readonly AutoMocker _mocker;
        private readonly ErrorHandlerMiddleware _middleware;

        public ErrorHandlerMiddlewareTests()
        {
            _mocker = new AutoMocker();
            _middleware = _mocker.CreateInstance<ErrorHandlerMiddleware>();
        }


        [Fact(DisplayName = "Deve retornar um erro 500 Internal Server")]
        public async Task ErrorHandlerMiddleware_AoReceberUmThrow_RetornarStatus500()
        {
            // Arrange
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();

            _mocker.GetMock<RequestDelegate>()
                    .Setup(x => x.Invoke(It.IsAny<HttpContext>()))
                    .Throws(new Exception());

            // Act
            await _middleware.Invoke(context);
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(context.Response.Body);
            var streamText = reader.ReadToEnd();

            //Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);
            Assert.Contains("Exception", streamText);
        }
    }
}
