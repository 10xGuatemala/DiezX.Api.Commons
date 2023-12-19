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
using Microsoft.AspNetCore.Mvc;

namespace DiezX.Api.Commons.ExceptionHandling
{

    /// <summary>
    /// Representa un error de validación específico en una solicitud HTTP.
    /// </summary>
    public class InvalidParam
    {
        /// <summary>
        /// Obtiene o establece el nombre del parámetro que causó el error de validación.
        /// </summary>
        /// <value>El nombre del parámetro inválido.</value>
        public string Name { get; set; }

        /// <summary>
        /// Obtiene o establece la razón por la cual el parámetro es considerado inválido.
        /// </summary>
        /// <value>La descripción del error de validación.</value>
        public string Reason { get; set; }
    }

    /// <summary>
    /// Extiende <see cref="ProblemDetails"/> para proporcionar detalles adicionales
    /// específicos del problema, siguiendo el estándar RFC 7807.
    /// </summary>
    public class ExtendedProblemDetail : ProblemDetails
    {
        /// <summary>
        /// Obtiene o establece la marca de tiempo cuando ocurrió el problema.
        /// </summary>
        /// <value>La fecha y hora del problema</value>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Obtiene o establece una lista de parámetros inválidos asociados con el problema.
        /// Esta propiedad sigue la extensión "invalid-params" del estándar RFC 7807.
        /// </summary>
        /// <value>Una lista de <see cref="InvalidParams"/> que describe cada parámetro inválido.</value>
        public List<InvalidParam> InvalidParams { get; set; }
    }

}

