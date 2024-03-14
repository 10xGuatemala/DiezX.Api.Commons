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
namespace DiezX.Api.Commons.Security.Dto
{
    /// <summary>
	/// Objeto de transferencia de datos para la solicitud de usuario.
	/// </summary>
	/// <remarks>
	/// Versión: 1.1.0
	/// </remarks>
	public class UsuarioRequestDto
    {
        /// <summary>
        /// Nombre de usuario del usuario que realiza la solicitud.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Rol del usuario que realiza la solicitud.
        /// </summary>
        public string[] Role { get; set; }

        /// <summary>
        /// Constructor para UsuarioRequestViewModel.
        /// </summary>
        /// <param name="username">Nombre de usuario del usuario que realiza la solicitud.</param>
        /// <param name="role">Rol del usuario que realiza la solicitud.</param>
        public UsuarioRequestDto(string username, string[] role)
        {
            Username = username;
            Role = role;
        }
    }
}

