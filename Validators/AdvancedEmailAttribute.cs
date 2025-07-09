//
//  Copyright 2023 Copyright Soluciones Modernas 10x
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
using System.Text.RegularExpressions;

namespace DiezX.Api.Commons.Validators
{
    /// <summary>
    /// Atributo de validación para verificar la estructura de una dirección de correo electrónico.
    /// </summary>
    /// <remarks>
    /// Este atributo personalizado se puede aplicar a propiedades de tipo string
    /// en modelos de datos para asegurarse de que la dirección de correo electrónico tenga un formato válido.
    /// </remarks>
    public partial class AdvancedEmailAttribute : ValidationAttribute
    {

        [GeneratedRegex("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(\\.[a-zA-Z]{2,})+$", RegexOptions.IgnoreCase | RegexOptions.Compiled)]
        private static partial Regex EmailRegex();

        /// <summary>
        /// Valida que la dirección de correo electrónico tenga un formato válido.
        /// </summary>
        /// <param name="value">El valor de la dirección de correo electrónico a validar.</param>
        /// <param name="validationContext">Contexto de validación que proporciona información sobre el modelo y la propiedad.</param>
        /// <returns>
        /// El resultado de la validación. Retorna <see cref="ValidationResult.Success"/> si la validación es exitosa;
        /// de lo contrario, retorna un objeto <see cref="ValidationResult"/> con el mensaje de error.
        /// </returns>
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success; // Si el campo es no mandatorio, de lo contrario usar Required
            }
            if (value is string stringValue && EmailRegex().IsMatch(stringValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.IsNullOrWhiteSpace(ErrorMessage) ?
                GetErrorMessage(value.ToString())
                : FormatErrorMessage(validationContext.DisplayName));
        }

        /// <summary>
        /// Obtiene el mensaje de error personalizado para la validación fallida.
        /// </summary>
        /// <returns>
        /// Una cadena que contiene el mensaje de error. Este mensaje informa al usuario sobre el formato incorrecto de la dirección de correo electrónico.
        /// </returns>
        /// <remarks>
        /// Se utiliza para generar un mensaje de error descriptivo que informa al usuario sobre la dirección de correo electrónico específica que
        /// no cumple con el formato requerido. El mensaje resultante es útil para la retroalimentación directa en formularios, permitiendo a los usuarios identificar y corregir errores de manera más efectiva.
        /// </remarks>
        private static string GetErrorMessage(string mail)
        {
            return $"La dirección de correo electrónico {mail} no tiene un formato válido.";
        }

    }
}
