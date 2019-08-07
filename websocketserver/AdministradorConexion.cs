using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace websocketserver
{
    public class AdministradorConexion
    {

        public AdministradorConexion(){}

        private ConcurrentDictionary<string, WebSocket> conexiones =
            new ConcurrentDictionary<string, WebSocket>();

        //obtener un socket
        public WebSocket obtenerSocketID(string id)
        {
            return conexiones.FirstOrDefault(x=>x.Key==id).Value;
        }

        //obtener todos los sockets
        public ConcurrentDictionary<string,WebSocket> obtenerConexiones()
        {
            return conexiones;
        }


        public string obtenerID(WebSocket socket)
        {
            return conexiones.FirstOrDefault(x=>x.Value==socket).Key;

            
        }

        public async Task eliminarSocket(string id)
        {
            conexiones.TryRemove(id, out var socket);


            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure,
                "Socket cerrado",CancellationToken.None);
        }


        public void agregarSocket(WebSocket socket)
        {
            conexiones.TryAdd(obtenerIDConexion(),socket);
        }

        private string obtenerIDConexion()
        {
            return Guid.NewGuid().ToString("N");
        }


    }
}
