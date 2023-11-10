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
using System.ComponentModel.DataAnnotations;

namespace DiezX.Api.Commons.Validators
{
    /// <summary>
    /// Atributo de validación que verifica el tamaño máximo de un archivo.
    /// </summary>
    public class FileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FileSizeAttribute"/>.
        /// </summary>
        /// <param name="maxSize">El tamaño máximo permitido en bytes.</param>
        public FileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        /// <summary>
        /// Valida el tamaño máximo del archivo.
        /// </summary>
        /// <param name="value">El valor del atributo a validar.</param>
        /// <param name="validationContext">El contexto de validación.</param>
        /// <returns>Un objeto <see cref="ValidationResult"/> que representa el resultado de la validación.</returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxSize)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }

}

