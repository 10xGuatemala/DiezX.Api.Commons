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
    /// Atributo de validación personalizado para validar contraseñas según criterios de seguridad específicos.
    /// Este atributo garantiza que las contraseñas cumplan con ciertas reglas para mejorar la seguridad de las credenciales de los usuarios.
    /// </summary>
    /// <remarks>
    /// El atributo se puede aplicar a cualquier propiedad de tipo string en modelos de datos que representen contraseñas.
    /// El objetivo es prevenir el uso de contraseñas débiles que pueden ser fácilmente comprometidas.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public partial class PasswordValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Define la expresión regular utilizada para la validación de contraseñas. 
        /// La contraseña debe cumplir con los siguientes criterios:
        /// - Tener al menos 6 caracteres de longitud para asegurar una complejidad mínima.
        /// - Contener al menos una letra minúscula y una letra mayúscula, o una letra minúscula y un dígito numérico, o una letra mayúscula y un dígito numérico.
        /// Esto asegura que la contraseña no sea demasiado simple y sea resistente a intentos comunes de ataques de fuerza bruta.
        /// </summary>
        [GeneratedRegex("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,}).")]
        private static partial Regex PasswordRegex();

        /// <summary>
        /// Valida que la contraseña proporcionada cumpla con los criterios definidos por la expresión regular.
        /// </summary>
        /// <param name="value">La contraseña a validar.</param>
        /// <param name="validationContext">Contexto de validación que proporciona información adicional sobre el entorno de validación.</param>
        /// <returns>
        /// Retorna <see cref="ValidationResult.Success"/> si la contraseña es válida de acuerdo con la expresión regular.
        /// En caso contrario, retorna un <see cref="ValidationResult"/> con un mensaje de error que indica el motivo de la falla.
        /// </returns>
        /// <exception cref="ArgumentException">Lanzada cuando el valor proporcionado es null, asegurando que el atributo no se aplique a contraseñas no proporcionadas.</exception>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                throw new ArgumentException("El valor del atributo es requerido");
            }

            if (value is string stringValue && PasswordRegex().IsMatch(stringValue))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(string.IsNullOrWhiteSpace(ErrorMessage) ?
                "La contraseña no tiene un formato válido. Asegúrese de que cumpla con los criterios de complejidad requeridos."
                : FormatErrorMessage(validationContext.DisplayName));
        }
    }
}
