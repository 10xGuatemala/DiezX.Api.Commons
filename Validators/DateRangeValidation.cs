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
namespace DiezX.Api.Commons.Validators
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Org.BouncyCastle.Pqc.Crypto.Lms;

    /// <summary>
    /// Atributo de validación para asegurar que la diferencia entre dos fechas no exceda un número especificado de meses.
    /// </summary>
    /// <remarks>
    /// Este atributo se aplica a nivel de clase y requiere los nombres de las propiedades de fecha de inicio y de fin, así como el máximo número de meses permitido entre estas fechas.
    /// Es ideal para validar rangos de fechas en filtros y formularios de búsqueda.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DateRangeAttribute : ValidationAttribute
    {
        private readonly string _startDatePropertyName;
        private readonly string _endDatePropertyName;
        private readonly int _maxMonthDifference;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DateRangeAttribute"/>.
        /// </summary>
        /// <param name="startDatePropertyName">El nombre de la propiedad que representa la fecha de inicio.</param>
        /// <param name="endDatePropertyName">El nombre de la propiedad que representa la fecha de fin.</param>
        /// <param name="maxMonthDifference">El número máximo de meses permitidos entre la fecha de inicio y fin.</param>
        public DateRangeAttribute(string startDatePropertyName, string endDatePropertyName, int maxMonthDifference)
        {
            _startDatePropertyName = startDatePropertyName;
            _endDatePropertyName = endDatePropertyName;
            _maxMonthDifference = maxMonthDifference;
        }

        /// <summary>
        /// Realiza la validación comprobando que la diferencia entre las fechas de inicio y fin no exceda el límite de meses establecido.
        /// </summary>
        /// <param name="value">El objeto que contiene las propiedades a validar.</param>
        /// <param name="validationContext">Contexto que proporciona información sobre la validación.</param>
        /// <returns>
        /// <see cref="ValidationResult.Success"/> si la validación es exitosa; de lo contrario, retorna un objeto <see cref="ValidationResult"/> con un mensaje de error.
        /// </returns>
        /// <exception cref="ArgumentException">Lanzado si las propiedades especificadas no se encuentran en el objeto.</exception>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var startDateProperty = validationContext.ObjectType.GetProperty(_startDatePropertyName);
            var endDateProperty = validationContext.ObjectType.GetProperty(_endDatePropertyName);

            //Esta verificación se realiza para asegurarse de que las propiedades especificadas en los nombres (_startDatePropertyName y _endDatePropertyName) existen en el objeto que se está validando
            if (startDateProperty == null || endDateProperty == null)
            {
                throw new ArgumentException("No se encontraron las propiedades necesarias para validar rango de fechas.");
            }

            var startDate = startDateProperty.GetValue(value, null) as DateTime?;
            var endDate = endDateProperty.GetValue(value, null) as DateTime?;

            // Si ninguno de los valores está presente, sigue adelante sin hacer más validaciones.
            if (!startDate.HasValue && !endDate.HasValue)
            {
                return ValidationResult.Success;
            }


            // Verificar que la fecha de inicio es anterior o igual a la fecha de fin
            if (startDate > endDate)
            {
                return new ValidationResult("La fecha de inicio debe ser anterior o igual a la fecha de fin.");
            }

            int monthDifference = ((endDate.Value.Year - startDate.Value.Year) * 12) + endDate.Value.Month - startDate.Value.Month;
            if (monthDifference > _maxMonthDifference)
            {
                return new ValidationResult(string.IsNullOrWhiteSpace(ErrorMessage) ?
              GetErrorMessage()
            : FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Genera un mensaje de error indicando que la diferencia entre las fechas es mayor al límite permitido.
        /// </summary>
        /// <returns>Una cadena con el mensaje de error.</returns>
        private string GetErrorMessage()
        {
            return $"La diferencia entre las fechas no debe exceder de {_maxMonthDifference} meses.";
        }
    }

}

