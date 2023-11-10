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
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using DiezX.Api.Commons.Utils;

namespace DiezX.Api.Commons.Exceptions
{
    /// <summary>
    /// Middleware for handling exceptions and generating problem details responses.
    /// </summary>
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly DateUtil _dateUtil;
        private ILogger<ExceptionHandler> _logger { get; }
        private readonly JsonSerializerOptions _jsonOptions;


        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ExceptionHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente delegado de solicitud.</param>
        /// <param name="dateUtil">La utilidad de fecha.</param>
        /// <param name="logger">El registrador.</param>
        /// <param name="jsonOptions">Las opciones de JSON.</param>
        public ExceptionHandler(RequestDelegate next
             , DateUtil dateUtil
             , ILogger<ExceptionHandler> logger
             , IOptions<JsonOptions> jsonOptions)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dateUtil = dateUtil ?? throw new ArgumentNullException(nameof(dateUtil));
            _jsonOptions = jsonOptions?.Value?.JsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        }


        /// <summary>
        /// Invoca el middleware.
        /// </summary>
        /// <param name="context">El contexto HTTP.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Captura la excepción y crea un detalle de problema
                var problemDetails = CreateProblemDetails(ex);

                // Configura la respuesta HTTP con los detalles de problema generados
                _ = ConfigureResponse(context, problemDetails);

                // Registra la excepción con fines de seguimiento o registro
                LogException(ex);
            }
        }

        /// <summary>
        /// Crea los detalles de problema a partir de una excepción.
        /// </summary>
        /// <param name="ex">La excepción.</param>
        /// <returns>Los detalles de problema generados.</returns>
        private CustomProblemDetails CreateProblemDetails(Exception ex)
        {
            var problemDetails = new CustomProblemDetails
            {
                Title = "Excepción no controlada",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "Se ha presentado una inconsistencia técnica. Le agradecemos intentar nuevamente en breve.",
                ErrorDate = _dateUtil.GetTime()
            };

            if (ex is ApiException apiException)
            {
                problemDetails.Title = apiException.Message;
                problemDetails.Status = apiException.StatusCode;
                problemDetails.Detail = apiException.Message;
            }
            return problemDetails;
        }

        /// <summary>
        /// Configura la respuesta HTTP con los detalles de problema.
        /// </summary>
        /// <param name="context">El contexto HTTP.</param>
        /// <param name="problemDetails">Los detalles de problema.</param>
        private async Task ConfigureResponse(HttpContext context, CustomProblemDetails problemDetails)
        {
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";

            var jsonResponse = JsonSerializer.Serialize(problemDetails, _jsonOptions);
            await context.Response.WriteAsync(jsonResponse);
        }

        /// <summary>
        /// Registra la excepción en el registro.
        /// </summary>
        /// <param name="ex">La excepción.</param>
        private void LogException(Exception ex)
        {
            Trace.WriteLine(ex);
            _logger.LogError(ex, ex.Message);
        }
    }

}

