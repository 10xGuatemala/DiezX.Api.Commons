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

namespace DiezX.Api.Commons.ExceptionHandling
{

    /// <summary>
    /// Representa una excepción de parametros con errores de validación en la aplicación. (BadRequest)
    /// </summary>
    public class ApiValidationParamsException : Exception
    {
        /// <summary>
        /// Obtiene una lista de errores de validación.
        /// </summary>
        /// <value>
        /// La lista de <see cref="InvalidParam"/> que contiene los detalles de los errores de validación.
        /// Cada <see cref="InvalidParam"/> en la lista representa un error de validación específico
        /// con información detallada sobre qué validación falló y por qué.
        /// </value>
        public List<InvalidParam> Errors { get; private set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ApiValidationParamsException"/> con una lista específica de errores de validación.
        /// </summary>
        /// <param name="errors">La lista de errores de validación.</param>
        public ApiValidationParamsException(List<InvalidParam> errors)
            : base("Se encontraron errores de validación.")
        {
            Errors = errors;
        }
    }


}

