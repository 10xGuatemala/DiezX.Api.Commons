//
//  Copyright 2023  Copyright Soluciones Modernas 10x
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
namespace DiezX.Api.Commons.Remote
{

    /// <summary>
    /// Clase de utilidades para manejar aspectos de las solicitudes remotas.
    /// </summary>
    public static class RemoteUtil
    {
        /// <summary>
        /// Obtiene la dirección IP del cliente que realiza la solicitud.
        /// </summary>
        /// <param name="context">El contexto HTTP de la solicitud actual.</param>
        /// <returns>
        /// Una cadena que representa la dirección IP del cliente.
        /// Si la dirección IP no se puede determinar, retorna "IP no disponible".
        /// </returns>
        /// <remarks>
        /// Este método intenta primero obtener la dirección IP del cliente
        /// del encabezado HTTP 'X-Real-IP'. Si este encabezado no está presente,
        /// se recurre a la propiedad 'RemoteIpAddress' del contexto de conexión.
        /// Si utilizas Nginx te recomendamos leer: https://medium.com/@10xers/how-to-get-the-clients-ip-address-in-net-core-when-behind-an-nginx-reverse-proxy-a128bf2a8450
        /// </remarks>
        public static string GetClientIpAddress(HttpContext context)
        {
            // Intenta obtener la dirección IP del cliente a partir del encabezado X-Real-IP
            var clientIp = context.Request.Headers["X-Real-IP"].ToString();

            // Si el encabezado X-Real-IP no está presente, usa la propiedad RemoteIpAddress
            if (string.IsNullOrEmpty(clientIp))
            {
                clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "IP no disponible";
            }

            return clientIp;
        }
    }
}

