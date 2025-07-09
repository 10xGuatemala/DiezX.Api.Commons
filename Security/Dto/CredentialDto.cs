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
using System.ComponentModel.DataAnnotations;

namespace DiezX.Api.Commons.Security {
    /// <summary>
    /// DTO para las credenciales de autenticación
    /// </summary>
    public class CredentialsDto {
        /// <summary>
        /// Nombre de usuario
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Contraseña del usuario
        /// </summary>
        public required string Password { get; set; }
    }
}
