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


        public override async Task Conexion(WebSocket socket)
        {
            await base.Conexion(socket);

            var socketID = Administrador.obtenerID(socket);

            await enviarMensajeATodos($"{socketID} Websockets");
        }

        public override async Task Recibido(WebSocket socket, WebSocketReceiveResult resultado, byte[] buffer)
        {
            var socketID = Administrador.obtenerID(socket);


            var mensaje = $"{socketID} {Encoding.UTF8.GetString(buffer,0,resultado.Count)}";


            await enviarMensajeATodos(mensaje);
        }
    }
}
