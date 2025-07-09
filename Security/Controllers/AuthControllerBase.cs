//
//  Copyright 2024  Copyright © 10X de Guatemala, S.A.
//
//  Para más detalles sobre los términos y condiciones,
//  consulte la licencia completa en: https://www.10x.gt/code-license
//
using DiezX.Api.Commons.Security.Dto;
using DiezX.Api.Commons.Security.Utils;
using Microsoft.AspNetCore.Mvc;

namespace DiezX.Api.Commons.Security.Controllers
{
    /// <summary>
    /// Controlador base con funcionalidad de autenticación por cookies.
    /// Hereda de ControllerBase y añade métodos para manejo de cookies de autenticación.
    /// </summary>
    public abstract class AuthControllerBase : ControllerBase
    {
        private readonly AuthUtil _authUtility;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Constructor base para controladores de autenticación.
        /// </summary>
        /// <param name="authUtility">Utilidad para manejo de autenticación</param>
        /// <param name="environment">Entorno de la aplicación</param>
        protected AuthControllerBase(AuthUtil authUtility, IWebHostEnvironment environment)
        {
            _authUtility = authUtility;
            _environment = environment;
        }

        /// <summary>
        /// Establece las cookies de autenticación y retorna una respuesta apropiada según el entorno.
        /// En desarrollo: retorna el token en el body (útil para Swagger/Postman).
        /// En producción: solo mensaje de éxito por seguridad.
        /// </summary>
        /// <param name="tokenResponse">Respuesta del token</param>
        /// <returns>ActionResult con la respuesta apropiada</returns>
        protected ActionResult<TokenResponseDto> OkWithAuthCookies(TokenResponseDto tokenResponse)
        {
            // Establecer cookies de autenticación
            _authUtility.SetTokenCookie(Response, tokenResponse.AccessToken, _authUtility.AuthTokenCookieName);
            _authUtility.SetTokenCookie(Response, tokenResponse.RefreshToken, _authUtility.RefreshTokenCookieName);

            // En desarrollo, incluir tokens en el body para facilitar testing
            if (_environment.IsDevelopment())
            {
                return Ok(tokenResponse);
            }

            // En producción, solo mensaje de éxito por seguridad
            return Ok(new { message = "Autenticación exitosa." });
        }

        /// <summary>
        /// Remueve las cookies de autenticación y retorna NoContent.
        /// </summary>
        /// <returns>NoContent result</returns>
        protected IActionResult NoContentWithoutAuthCookies()
        {
            _authUtility.RemoveTokenCookies(Response);
            return NoContent();
        }

        /// <summary>
        /// Establece una cookie específica de token.
        /// </summary>
        /// <param name="token">Token a almacenar</param>
        /// <param name="cookieName">Nombre de la cookie</param>
        protected void SetTokenCookie(string token, string cookieName)
        {
            _authUtility.SetTokenCookie(Response, token, cookieName);
        }

        /// <summary>
        /// Remueve una cookie específica.
        /// </summary>
        /// <param name="cookieName">Nombre de la cookie a remover</param>
        protected void RemoveTokenCookie(string cookieName)
        {
            Response.Cookies.Delete(cookieName);
        }
    }
} 