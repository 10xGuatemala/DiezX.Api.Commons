//
//  Copyright 2023  Copyright Soluciones Modernas 10X
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
using System.Text;

namespace DiezX.Api.Commons.Security
{

    public static class HeaderUtil
    {
        /// <summary>
        /// Decodifica la cabecera de autorización tipo BASIC para extraer las credenciales del usuario.
        /// </summary>
        /// <param name="authorization">El valor de la cabecera de autorización.</param>
        /// <returns>Un objeto CredentialsDto con el nombre de usuario y la contraseña.</returns>
        /// <exception cref="FormatException">Se lanza si la cabecera de autorización no tiene el formato esperado.</exception>
        public static CredentialsDto DecodeBasicAuthorization(string authorization)
        {
            if (string.IsNullOrWhiteSpace(authorization) || !authorization.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                throw new FormatException("Formato de cabecera de autorización inválido.");
            }

            var encodedCredentials = authorization[6..]; // Omitir el texto "Basic "
            var credentialBytes = Convert.FromBase64String(encodedCredentials);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');

            if (credentials.Length != 2)
            {
                throw new FormatException("Formato de credenciales inválido.");
            }

            return new() { Username = credentials[0], Password = credentials[1] };
        }
    }

}

