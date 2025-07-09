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
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace DiezX.Api.Commons.Security.Dto
{
    /// <summary>
    /// DTO para el token decodificado
    /// </summary>
    public class DecodedTokenDto
    {
        /// <summary>
        /// Claims principales del token
        /// </summary>
        public ClaimsPrincipal ClaimPrincipal { get; set; }

        /// <summary>
        /// Token de seguridad
        /// </summary>
        public SecurityToken SecurityToken { get; set; }
    }
}

