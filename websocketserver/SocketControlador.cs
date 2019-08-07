using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace websocketserver
{
    public abstract class SocketControlador
    {
        public AdministradorConexion Administrador { get; set; }


        public SocketControlador(AdministradorConexion conexiones)
        {

            Administrador = conexiones;
        }


        public virtual async Task Conexion(WebSocket socket)
        {
            await Task.Run(()=> {

                Administrador.agregarSocket(socket);


            });
        }


        public async  Task Desconexion(WebSocket socket)
        {
            
              await  Administrador.eliminarSocket(Administrador.obtenerID(socket));

           
        }


        public  async Task enviarMensaje(WebSocket socket,string mensaje)
        {
            if (socket.State==WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(mensaje),
                    0,mensaje.Length),WebSocketMessageType.Text,true,CancellationToken.None);
            }
        }

        public async Task enviarMensajeString(string id, string mensaje)
        {

            await enviarMensaje(Administrador.obtenerSocketID(id),mensaje);
            
        }

        public async Task enviarMensajeATodos(string mensaje)
        {

            foreach (var item in Administrador.obtenerConexiones())
            {
                await enviarMensaje(item.Value,mensaje);
            }
        }



        public abstract Task Recibido(WebSocket socket,WebSocketReceiveResult resultado,byte[] buffer);


    }
}
