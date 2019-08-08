using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace websocketserver
{
    public class MensajeControlador : SocketControlador
    {
        public MensajeControlador(AdministradorConexion  conexiones) : base(conexiones)
        {

        }



        /// <summary>
        /// Método que conecta un websocket, obtiene el id del socket que se acaba de conectar y luego envía un mensaje a todos los websockets conectados
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>

        public override async Task Conexion(WebSocket socket)
        {
            await base.Conexion(socket);

            var socketID = Administrador.obtenerID(socket);

            await enviarMensajeATodos($"{socketID} Websockets");
        }


        /// <summary>
        /// Método que obtiene el mensaje recibido de parte de el websocket que envió el mensaje y lo reenvía a todos los websockets conectados
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="resultado"></param>
        /// <param name="buffer"></param>
        /// <returns></returns>

        public override async Task Recibido(WebSocket socket, WebSocketReceiveResult resultado, byte[] buffer)
        {
            var socketID = Administrador.obtenerID(socket);


            var mensaje = $"{socketID} {Encoding.UTF8.GetString(buffer,0,resultado.Count)}";


            await enviarMensajeATodos(mensaje);
        }
    }
}
