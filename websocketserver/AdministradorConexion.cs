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


        /// <summary>
        /// Método para obtener el id de un websocket
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public WebSocket obtenerSocketID(string id)
        {
            return conexiones.FirstOrDefault(x=>x.Key==id).Value;
        }

    
        public ConcurrentDictionary<string,WebSocket> obtenerConexiones()
        {
            return conexiones;
        }

        /// <summary>
        /// Método que obtiene el id de el socket que envió el mensaje
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>

        public string obtenerID(WebSocket socket)
        {
            return conexiones.FirstOrDefault(x=>x.Value==socket).Key;

            
        }

        /// <summary>
        /// Método que desconecta el websocket del servidor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

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
