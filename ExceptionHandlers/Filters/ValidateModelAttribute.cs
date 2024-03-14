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
using Microsoft.AspNetCore.Mvc.Filters;

namespace DiezX.Api.Commons.ExceptionHandling.Filters
{
    /// <summary>
    /// Un filtro de acción que valida el estado del modelo antes de ejecutar la acción.
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Se ejecuta antes de la acción del controlador y valida el estado del modelo.
        /// </summary>
        /// <param name="context">Contexto de ejecución de la acción, que contiene información sobre el modelo y el controlador.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Verifica si el estado del modelo es válido.
            if (!context.ModelState.IsValid)
            {
                // Recopila los errores de validación del ModelState y los convierte en una lista de InvalidParam.
                var errors = context.ModelState
                    .Where(kvp => kvp.Value.Errors.Count > 0)
                    .SelectMany(kvp => kvp.Value.Errors.Select(error => new InvalidParam
                    {
                        Name = kvp.Key,
                        Reason = string.IsNullOrEmpty(error.ErrorMessage)
                                 ? error.Exception?.Message
                                 : error.ErrorMessage
                    }))
                    .ToList();

                // Lanza una excepción personalizada que contiene los detalles de los errores de validación.
                throw new ApiValidationParamsException(errors);
            }
        }
    }

}

