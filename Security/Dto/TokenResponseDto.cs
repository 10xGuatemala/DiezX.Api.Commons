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

using System.Text.Json.Serialization;

namespace DiezX.Api.Commons.Security
{
    /// <summary>
    /// Representa una respuesta de token de acuerdo con la RFC 6749.
    /// </summary>
    /// <remarks>
    /// Esta clase se adhiere a la especificación detallada en la sección 5.1 de la RFC 6749.
    /// </remarks>
    public class TokenResponseDto
    {
        /// <summary>
        /// Obtiene o establece el token de acceso emitido por el servidor de autorización.
        /// </summary>
        /// <remarks>
        /// Definido por RFC 6749 sección 5.1.
        /// </remarks>
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }

        /// <summary>
        /// Obtiene o establece el tipo de token emitido. Por defecto es "Bearer".
        /// </summary>
        /// <remarks>
        /// Definido por RFC 6749 sección 5.1.
        /// </remarks>
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// Obtiene o establece la vida útil en segundos del token de acceso.
        /// </summary>
        /// <remarks>
        /// Definido por RFC 6749 sección 5.1.
        /// </remarks>
        [JsonPropertyName("expires_in")]
        public required long Expiration { get; set; }

        /// <summary>
        /// Obtiene o establece el token de actualización que se puede usar para obtener nuevos tokens de acceso.
        /// </summary>
        /// <remarks>
        /// Definido por RFC 6749 sección 5.1.
        /// </remarks>
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Obtiene o establece el alcance de la solicitud de acceso según lo descrito por RFC 6749 sección 3.3.
        /// </summary>
        /// <remarks>
        /// Definido por RFC 6749 sección 5.1.
        /// </remarks>
        [JsonPropertyName("scope")]
        public required string Scope { get; set; }
    }

}

