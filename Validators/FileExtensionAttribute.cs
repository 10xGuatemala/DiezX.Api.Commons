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
    /// Atributo de validación para verificar la extensión de un archivo.
    /// </summary>
    /// <remarks>
    /// Este atributo personalizado se puede aplicar a propiedades de tipo IFormFile
    /// en modelos de datos para asegurarse de que el archivo cargado tenga una de las extensiones permitidas.
    /// </remarks>
    public class FileExtensionAttribute : ValidationAttribute
    {
        /// <summary>
        /// Lista de extensiones de archivo permitidas.
        /// </summary>
        private readonly string[] _extensions;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="FileExtensionAttribute"/> con las extensiones especificadas.
        /// </summary>
        /// <param name="extensions">Array de cadenas que contiene las extensiones permitidas.</param>
        public FileExtensionAttribute(params string[] extensions)
        {
            _extensions = extensions;
        }

        /// <summary>
        /// Valida que la extensión del archivo cargado sea una de las extensiones permitidas.
        /// </summary>
        /// <param name="value">El valor del archivo a validar.</param>
        /// <param name="validationContext">Contexto de validación que proporciona información sobre el modelo y la propiedad.</param>
        /// <returns>
        /// El resultado de la validación. Retorna <see cref="ValidationResult.Success"/> si la validación es exitosa;
        /// de lo contrario, retorna un objeto <see cref="ValidationResult"/> con el mensaje de error.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_extensions.Contains(extension))
                {
                    return new ValidationResult(string.IsNullOrWhiteSpace(ErrorMessage) ?
                               GetErrorMessage(file.FileName)
                               : FormatErrorMessage(validationContext.DisplayName));
                }
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Obtiene el mensaje de error personalizado para la validación fallida, incluyendo el nombre del archivo.
        /// </summary>
        /// <param name="fileName">El nombre del archivo que está siendo validado.</param>
        /// <returns>
        /// Una cadena que contiene el mensaje de error. Este mensaje incluye el nombre del archivo y la lista de extensiones permitidas.
        /// </returns>
        /// <remarks>
        /// Este método se utiliza para generar un mensaje de error descriptivo que informa al usuario sobre el archivo específico que
        /// no cumple con las restricciones de extensión. El mensaje resultante es útil para la retroalimentación directa en formularios
        /// de carga de archivos, permitiendo a los usuarios identificar y corregir errores de manera más efectiva.
        /// </remarks>
        public string GetErrorMessage(string fileName)
        {
            return $"El archivo '{fileName}' no tiene una extensión válida. Extensiones permitidas: {string.Join(", ", _extensions)}.";
        }

    }

}

