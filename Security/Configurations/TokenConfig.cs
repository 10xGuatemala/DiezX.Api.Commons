//
//  Copyright © 2024 10X de Guatemala, S.A.
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
namespace DiezX.Api.Commons.Security
{
    public class TokenConfig
    {

        /// <summary>
        /// Secreto para cifrar el token
        /// </summary>
        public required string Secret { get; set; }
        /// <summary>
        /// Tiempo de expiranción de token de seguridad 
        /// </summary>
        public long DefaultTokenExpiration { get; set; }

        /// <summary>
        /// Ubicación de la llave privada para firmar los tokens
        /// </summary>
        public string RsaKeyPath { get; set; }

    }


}

