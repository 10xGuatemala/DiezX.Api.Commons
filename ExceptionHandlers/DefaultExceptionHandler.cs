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
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Microsoft.Extensions.Options;
using DiezX.Api.Commons.Utils;
using System.Net;
using DiezX.Api.Commons.ExceptionHandling;
using DiezX.Api.Commons.ExceptionHandlers.Exceptions;
using Microsoft.Extensions.Logging;

namespace DiezX.Api.Commons.Exceptions
{
    /// <summary>
    /// Middleware para manejar excepciones y generar respuestas estandarizadas
    /// con detalles del problema, siguiendo el formato RFC 7807.
    /// </summary>
    public class DefaultExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly DateUtil _dateUtil;
        private readonly ILogger<DefaultExceptionHandler> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DefaultExceptionHandler"/>.
        /// </summary>
        /// <param name="next">El siguiente delegado en la cadena de middleware.</param>
        /// <param name="dateUtil">Utilidad para manejar operaciones relacionadas con fechas.</param>
        /// <param name="logger">Logger para registrar información de las excepciones.</param>
        /// <param name="jsonOptions">Opciones de serialización JSON.</param>
        public DefaultExceptionHandler(
            RequestDelegate next,
            DateUtil dateUtil,
            ILogger<DefaultExceptionHandler> logger,
            IOptions<JsonOptions> jsonOptions)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _dateUtil = dateUtil ?? throw new ArgumentNullException(nameof(dateUtil));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _jsonOptions = jsonOptions?.Value?.JsonSerializerOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
        }

        /// <summary>
        /// Procesa cada solicitud HTTP pasando el control al siguiente middleware y capturando cualquier excepción que ocurra.
        /// </summary>
        /// <param name="context">El contexto HTTP de la solicitud actual.</param>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (problemDetails, logLevel, logMessage) = CreateProblemDetailsAndLogInfo(context, ex);
                await ConfigureResponse(context, problemDetails);
                _logger.Log(logLevel, ex, logMessage);
            }
        }

        /// <summary>
        /// Crea un objeto <see cref="ProblemDetails"/> basado en la excepción capturada.
        /// </summary>
        /// <param name="context">Contexto HTTP actual.</param>
        /// <param name="ex">Excepción capturada para procesar.</param>
        /// <returns>Un objeto <see cref="ProblemDetails"/> que representa los detalles del problema.</returns>
        private (ExtendedProblemDetail, LogLevel, string) CreateProblemDetailsAndLogInfo(HttpContext context, Exception ex)
        {
            var problemDetails = new ExtendedProblemDetail
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "InternalServerError",
                Detail = "Se ha presentado una inconsistencia técnica. Por favor, intente nuevamente más tarde.",
                Timestamp = _dateUtil.GetTime(),
                Instance = context.Request.Path
            };

            LogLevel logLevel = LogLevel.Error;
            string logMessage = $"Error crítico no manejado: {ex.Message}";

            if (ex is ApiGeneralException apiException)
            {
                problemDetails.Status = apiException.StatusCode;
                problemDetails.Title = apiException.Title ?? "Error de Aplicación";
                problemDetails.Detail = apiException.Message;
                logLevel = LogLevel.Information;
                logMessage = $"Error de aplicación ({apiException.StatusCode}): {apiException.Message}";
            }
            else if (ex is ApiValidationParamsException validationException)
            {
                problemDetails.Status = StatusCodes.Status400BadRequest;
                problemDetails.Title = "ValidationError";
                problemDetails.Detail = "Se encontraron errores de validación en la petición.";
                problemDetails.InvalidParams = validationException.Errors;
                logLevel = LogLevel.Information;
                logMessage = $"Error de validación: {validationException.Message}. Parámetros inválidos: {JsonSerializer.Serialize(validationException.Errors)}";
            }
            else if (ex is TokenExpiredException tokenException)
            {
                problemDetails.Status = StatusCodes.Status401Unauthorized;
                problemDetails.Title = "TokenError";
                problemDetails.Detail = tokenException.Message;
                logLevel = LogLevel.Warning;
                logMessage = $"Token expirado o inválido: {tokenException.Message}";
            }
            else if (ex is DataNotFoundException dataNotFoundException)
            {
                problemDetails.Status = StatusCodes.Status404NotFound;
                problemDetails.Title = "NoDataFoundError";
                problemDetails.Detail = dataNotFoundException.Message;
                logLevel = LogLevel.Information;
                logMessage = $"Datos no encontrados: {dataNotFoundException.Message}";
            }

            problemDetails.Title ??= Enum.GetName(typeof(HttpStatusCode), problemDetails.Status);

            return (problemDetails, logLevel, logMessage);
        }

        /// <summary>
        /// Configura y envía la respuesta HTTP basada en los detalles del problema.
        /// </summary>
        /// <param name="context">El contexto HTTP de la solicitud.</param>
        /// <param name="problemDetails">Detalles del problema para incluir en la respuesta.</param>
        private async Task ConfigureResponse(HttpContext context, ExtendedProblemDetail problemDetails)
        {
            context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/problem+json";
            var jsonResponse = JsonSerializer.Serialize(problemDetails, _jsonOptions);
            await context.Response.WriteAsync(jsonResponse);
        }
    }
}

