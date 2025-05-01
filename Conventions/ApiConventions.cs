//
//  Copyright 2024  Copyright © 10X de Guatemala, S.A.
//
//  Para más detalles sobre los términos y condiciones,
//  consulte la licencia completa en: https://www.10x.gt/code-license
//
using DiezX.Api.Commons.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace DiezX.Api.Commons.Conventions
{
    /// <summary>
    /// Convención que incluye las posibles respuestas estándar del api para robustecer la documentación de swagger
    /// </summary>
    public static class ApiConventions
    {
        /// <summary>
        /// Convención que incluye las posibles respuestas estándar del api para peticiones post
        /// </summary>
        /// <remarks>
        /// Esta convención se aplica a métodos que pueden devolver errores de autenticación, recursos no encontrados o solicitudes mal formadas.
        /// </remarks>
        /// <response code="400">Si los campos de entrada no superan la validación de datos</response>
        /// <response code="401">Si el usuario de la peticion no esta autorizado</response>
        /// <response code="401">Si el usuario tiene prohibido realizar la petición</response>
        /// <response code="404">Si uno de los recursos necesarios para realizar la acción no se encuentra disponible</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType(typeof(ExtendedProblemDetail))]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]
        public static void Created() 
        { 
            // Este método está intencionalmente vacío. Su propósito es únicamente definir convenciones de API.
        }


        /// <summary>
        /// Convención que incluye las posibles respuestas estándar del api para peticiones get
        /// </summary>
        /// <remarks>
        /// Esta convención se aplica a métodos que pueden devolver errores de autenticación, recursos no encontrados o solicitudes mal formadas.
        /// </remarks>
        /// <response code="400">Si los campos de entrada no superan la validación de datos</response>
        /// <response code="401">Si el usuario de la peticion no esta autorizado</response>
        /// <response code="401">Si el usuario tiene prohibido realizar la petición</response>
        /// <response code="404">Si uno de los recursos necesarios para realizar la acción no se encuentra disponible</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType(typeof(ExtendedProblemDetail))]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]
        public static void Ok() 
        { 
            // Este método está intencionalmente vacío. Su propósito es únicamente definir convenciones de API.
        }


        /// <summary>
        /// Convención que incluye las posibles respuestas estándar del api para peticiones put
        /// </summary>
        /// <remarks>
        /// Esta convención se aplica a métodos que pueden devolver errores de autenticación, recursos no encontrados o solicitudes mal formadas.
        /// </remarks>
        /// <response code="400">Si los campos de entrada no superan la validación de datos</response>
        /// <response code="401">Si el usuario de la peticion no esta autorizado</response>
        /// <response code="401">Si el usuario tiene prohibido realizar la petición</response>
        /// <response code="404">Si uno de los recursos necesarios para realizar la acción no se encuentra disponible</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(ExtendedProblemDetail), StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType(typeof(ExtendedProblemDetail))]
        [ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]
        public static void NoContent() 
        { 
            // Este método está intencionalmente vacío. Su propósito es únicamente definir convenciones de API.
        }

    }
}

