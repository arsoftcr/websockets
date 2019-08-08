using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace websocketserver
{
    public class SocketMiddleware
    {
        private RequestDelegate  Proximo;

        private SocketControlador Controlador;


        public SocketMiddleware(RequestDelegate proximo,SocketControlador socket)
        {
            Proximo = proximo;

            Controlador = socket;
        }

        /// <summary>
        /// Este método debe llamarse Invoke o InvokeAsync
        /// </summary>
        /// <param name="contexto"></param>
        /// <returns></returns>

        public async  Task InvokeAsync(HttpContext contexto)
        {
            if (contexto.WebSockets.IsWebSocketRequest)
            {
                var socket = await contexto.WebSockets.AcceptWebSocketAsync();


                await Controlador.Conexion(socket);

                await Recibir(socket,async(resultado,buffer)=> {

                    if (resultado.MessageType==WebSocketMessageType.Text)
                    {
                        await Controlador.Recibido(socket,resultado,buffer);

                    }else if (resultado.MessageType == WebSocketMessageType.Close)
                    {
                        await Controlador.Desconexion(socket);
                    }


                });
            }
           
        }

        /// <summary>
        /// Método que escucha cuando se conecta un socket y espera por los mensajes que se van a recibir
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>

        private async Task Recibir(WebSocket socket,
            Action<WebSocketReceiveResult, byte[]>  mensaje)
        {
            var buffer = new byte[1024*4];

            while (socket.State==WebSocketState.Open)
            {
                var resultado = await socket.ReceiveAsync(new ArraySegment<byte>(buffer),CancellationToken.None);

                mensaje(resultado,buffer);
            }
        }
    }
}
