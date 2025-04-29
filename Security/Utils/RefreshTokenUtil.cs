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
using System.Security.Cryptography;

namespace DiezX.Api.Commons.Security.Utils
{
    /// <summary>
    /// Utilidad para la generación de Refresh Tokens seguros.
    /// </summary>
    public static class RefreshTokenUtil
    {
        private const int TokenSize = 64; // 64 bytes = 512 bits

        /// <summary>
        /// Genera un Refresh Token seguro utilizando un generador criptográfico de números aleatorios.
        /// </summary>
        /// <returns>Un Refresh Token aleatorio en formato Base64.</returns>
        public static string Create()
        {
            byte[] randomBytes = new byte[TokenSize];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
