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


        //Este metodo debe llamarse Invoke o InvokeAsync
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
