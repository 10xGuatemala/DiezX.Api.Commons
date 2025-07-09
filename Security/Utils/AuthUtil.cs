//
//  Copyright 2024  Copyright © 10X de Guatemala, S.A.
//
//  Para más detalles sobre los términos y condiciones,
//  consulte la licencia completa en: https://www.10x.gt/code-license
//
using DiezX.Api.Commons.Security.Configurations;
using DiezX.Api.Commons.Security.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DiezX.Api.Commons.Security.Utils
{
    /// <summary>
    /// Utilidad para el manejo de autenticación, tokens y cookies de seguridad.
    /// </summary>
    /// <remarks>
    /// Esta clase proporciona métodos y utilidades para:
    /// <list type="bullet">
    /// <item><description>Gestión de cookies de autenticación (JWT y refresh tokens)</description></item>
    /// <item><description>Configuración de seguridad para cookies (HttpOnly, Secure, SameSite)</description></item>
    /// <item><description>Manejo de respuestas según el entorno (desarrollo/producción)</description></item>
    /// </list>
    /// </remarks>
    /// <example>
    /// Ejemplo de uso en un controlador:
    /// <code>
    /// public class MiControlador : ControllerBase
    /// {
    ///     private readonly AuthUtil _authUtil;
    ///     
    ///     public MiControlador(AuthUtil authUtil)
    ///     {
    ///         _authUtil = authUtil;
    ///     }
    ///     
    ///     [HttpPost("login")]
    ///     public ActionResult&lt;TokenResponseDto&gt; Login(TokenResponseDto tokens)
    ///     {
    ///         return _authUtil.SetTokensAndRespond(this, tokens, _environment);
    ///     }
    /// }
    /// </code>
    /// </example>
    public class AuthUtil
    {
        private readonly CookieConfig _cookieConfig;

        #region Constantes Generales

        /// <summary>
        /// Prefijo estándar para cabeceras de autorización Bearer JWT.
        /// </summary>
        /// <remarks>
        /// Se utiliza para extraer el token JWT de la cabecera Authorization.
        /// El formato completo es: "Bearer {token}"
        /// </remarks>
        public const string BEARER_PREFIX = "Bearer ";

        #endregion

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="AuthUtil"/>.
        /// </summary>
        /// <param name="cookieConfig">Configuración de cookies desde appsettings.json</param>
        /// <remarks>
        /// La configuración debe incluir:
        /// <list type="bullet">
        /// <item><description>Nombres de cookies para tokens</description></item>
        /// <item><description>Configuraciones de seguridad (HttpOnly, Secure)</description></item>
        /// <item><description>Tiempo de vida de las cookies</description></item>
        /// </list>
        /// </remarks>
        public AuthUtil(IOptions<CookieConfig> cookieConfig)
        {
            _cookieConfig = cookieConfig.Value;
        }

        #region Propiedades de Configuración

        /// <summary>
        /// Obtiene el nombre configurado para la cookie del token de acceso JWT.
        /// </summary>
        /// <remarks>
        /// Este valor se configura en appsettings.json bajo la sección CookieConfig.
        /// </remarks>
        public string AuthTokenCookieName => _cookieConfig.AuthTokenCookieName;

        /// <summary>
        /// Obtiene el nombre configurado para la cookie del refresh token.
        /// </summary>
        /// <remarks>
        /// Este valor se configura en appsettings.json bajo la sección CookieConfig.
        /// </remarks>
        public string RefreshTokenCookieName => _cookieConfig.RefreshTokenCookieName;

        #endregion

        #region Métodos Helper para Cookies

        /// <summary>
        /// Establece las cookies de autenticación y genera una respuesta apropiada según el entorno.
        /// </summary>
        /// <param name="controller">Controlador desde donde se llama el método</param>
        /// <param name="result">DTO con los tokens de acceso y refresh</param>
        /// <param name="environment">Entorno de la aplicación para determinar el comportamiento</param>
        /// <returns>
        /// En desarrollo: ActionResult con los tokens en el cuerpo de la respuesta.
        /// En producción: ActionResult con mensaje de éxito sin exponer los tokens.
        /// </returns>
        /// <remarks>
        /// Este método:
        /// <list type="bullet">
        /// <item><description>Establece las cookies seguras para ambos tokens</description></item>
        /// <item><description>Adapta la respuesta según el entorno</description></item>
        /// <item><description>Implementa medidas de seguridad para producción</description></item>
        /// </list>
        /// </remarks>
        public ActionResult<TokenResponseDto> SetTokensAndRespond(
            ControllerBase controller, 
            TokenResponseDto result, 
            IWebHostEnvironment environment)
        {
            // Establece las cookies seguras y HttpOnly para los tokens
            SetTokenCookie(controller.Response, result.AccessToken, _cookieConfig.AuthTokenCookieName);
            SetTokenCookie(controller.Response, result.RefreshToken, _cookieConfig.RefreshTokenCookieName);

            bool esDesarrollo = environment.IsDevelopment();

            // En entorno de desarrollo, devuelve el token también en el cuerpo de la respuesta (útil para Swagger/Postman)
            if (esDesarrollo)
            {
                return controller.Ok(result);
            }

            // En producción, evita exponer los tokens en el body por seguridad
            return controller.Ok(new { message = "Autenticación exitosa." });
        }

        /// <summary>
        /// Elimina las cookies de autenticación del navegador del cliente.
        /// </summary>
        /// <param name="response">Objeto HttpResponse para manipular las cookies</param>
        /// <remarks>
        /// Este método elimina tanto la cookie del token JWT como la del refresh token.
        /// Se utiliza típicamente en operaciones de cierre de sesión o revocación de tokens.
        /// </remarks>
        public void RemoveTokenCookies(HttpResponse response)
        {
            response.Cookies.Delete(_cookieConfig.AuthTokenCookieName);
            response.Cookies.Delete(_cookieConfig.RefreshTokenCookieName);
        }

        /// <summary>
        /// Establece una cookie de token con las configuraciones de seguridad apropiadas.
        /// </summary>
        /// <param name="response">Objeto HttpResponse para manipular las cookies</param>
        /// <param name="token">Token a almacenar en la cookie</param>
        /// <param name="cookieName">Nombre de la cookie a establecer</param>
        /// <remarks>
        /// Aplica las siguientes configuraciones de seguridad:
        /// <list type="bullet">
        /// <item><description>HttpOnly: Previene acceso desde JavaScript</description></item>
        /// <item><description>Secure: Requiere HTTPS</description></item>
        /// <item><description>SameSite: Protección contra CSRF</description></item>
        /// <item><description>Expires: Tiempo de vida configurado</description></item>
        /// </list>
        /// </remarks>
        public void SetTokenCookie(HttpResponse response, string token, string cookieName)
        {
            var sameSiteMode = _cookieConfig.SameSite.ToLower() switch
            {
                "strict" => SameSiteMode.Strict,
                "lax" => SameSiteMode.Lax,
                "none" => SameSiteMode.None,
                _ => SameSiteMode.Strict
            };

            response.Cookies.Append(cookieName, token, new CookieOptions
            {
                HttpOnly = _cookieConfig.HttpOnly,
                Secure = _cookieConfig.Secure,
                SameSite = sameSiteMode,
                Expires = DateTimeOffset.UtcNow.AddDays(_cookieConfig.LifetimeDays)
            });
        }

        #endregion
    }
} 