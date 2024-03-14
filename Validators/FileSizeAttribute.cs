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

namespace DiezX.Api.Commons.Validators
{

    /// <summary>
    /// Atributo de validación para verificar el tamaño de un archivo.
    /// </summary>
    /// <remarks>
    /// Este atributo se puede aplicar a propiedades de tipo IFormFile en modelos de datos para asegurarse de que el tamaño del archivo cargado no exceda un tamaño máximo especificado en bytes.
    /// </remarks>
    public class FileSizeAttribute : ValidationAttribute
    {
        /// <summary>
        /// Representa un kilobyte (KB).
        /// </summary>
        public const int KB = 1024;

        /// <summary>
        /// Representa un megabyte (MB).
        /// </summary>
        public const int MB = 1024 * KB;

        private readonly long _maxSize;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FileSizeAttribute"/> con un tamaño máximo en bytes.
        /// </summary>
        /// <param name="maxSize">El tamaño máximo permitido en bytes.</param>
        public FileSizeAttribute(int maxSize)
        {
            _maxSize = maxSize;
        }

        /// <summary>
        /// Valida el tamaño del archivo.
        /// </summary>
        /// <param name="value">El valor del atributo a validar, esperado como IFormFile.</param>
        /// <param name="validationContext">El contexto de validación.</param>
        /// <returns>
        /// Un objeto <see cref="ValidationResult"/> que representa el resultado de la validación. Retorna <see cref="ValidationResult.Success"/> si la validación es exitosa; de lo contrario, retorna un objeto <see cref="ValidationResult"/> con un mensaje de error.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file && file.Length > _maxSize)
            {
                return new ValidationResult(string.IsNullOrWhiteSpace(ErrorMessage) ?
                           GetErrorMessage(file.FileName, file.Length)
                           : FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Genera un mensaje de error personalizado que incluye el nombre y el tamaño del archivo.
        /// </summary>
        /// <param name="fileName">El nombre del archivo que se está validando.</param>
        /// <param name="fileSize">El tamaño del archivo en bytes.</param>
        /// <returns>Una cadena con el mensaje de error.</returns>
        private string GetErrorMessage(string fileName, long fileSize)
        {
            return $"El archivo '{fileName}' ({FormatSize(fileSize)}) excede el límite permitido de {FormatSize(_maxSize)}.";
        }

        /// <summary>
        /// Formatea el tamaño del archivo de bytes a KB o MB para una mejor legibilidad.
        /// </summary>
        /// <param name="size">El tamaño del archivo en bytes.</param>
        /// <returns>Una cadena representando el tamaño del archivo en un formato legible.</returns>
        private static string FormatSize(long size)
        {
            return size switch
            {
                < KB => $"{size} bytes",
                < MB => $"{size / (double)KB:F2} KB",
                _ => $"{size / (double)MB:F2} MB"
            };
        }
    }



}

