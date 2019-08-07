using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace websocketserver
{
    public static class Extensiones
    {

        public  static IServiceCollection coleccionServicios(this IServiceCollection servicios)
        {
            servicios.AddTransient<AdministradorConexion>();

            foreach (var item in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if (item.GetTypeInfo().BaseType==typeof(SocketControlador))
                {
                    servicios.AddSingleton(item);
                }
            }



            return servicios;
        }


        public static IApplicationBuilder MapaSockets(this IApplicationBuilder aplicacion,
            PathString ruta,SocketControlador socket)
        {
            return aplicacion.Map(ruta,(x)=>x.UseMiddleware<SocketMiddleware>(socket));
        }
    }
}
