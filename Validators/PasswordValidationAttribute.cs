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
using System.Text.RegularExpressions;

namespace DiezX.Api.Commons.Validators
{

    /// <summary>
    /// Atributo de validación personalizado para validar contraseñas según criterios específicos.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public partial class PasswordValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Expresión regular para validar contraseñas. La contraseña debe:
        /// - Tener al menos 6 caracteres de longitud.
        /// - Contener al menos una letra minúscula y una letra mayúscula, O
        /// - Contener al menos una letra minúscula y un dígito numérico, O
        /// - Contener al menos una letra mayúscula y un dígito numérico.
        /// </summary>
        [GeneratedRegex("^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,}).")]
        private static partial Regex MyRegex();

        /// <summary>
        /// Determina si el valor proporcionado es válido según los criterios de contraseña definidos.
        /// </summary>
        /// <param name="value">El valor de la contraseña a validar.</param>
        /// <returns>Verdadero si la contraseña es válida; de lo contrario, falso.</returns>
        public override bool IsValid(object value)
        {
            if (value == null) return false;

            if (value is string password)
            {

                Regex regex = MyRegex();
                return regex.IsMatch(password);
            }
            return false;
        }


    }
}

