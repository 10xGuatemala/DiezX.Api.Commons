//
//  Copyright 2024  Copyright © 10X de Guatemala, S.A.
//
//  Para más detalles sobre los términos y condiciones,
//  consulte la licencia completa en: https://www.10x.gt/code-license
//

namespace DiezX.Api.Commons.Security.Configurations
{
    /// <summary>
    /// Configuración para las cookies de autenticación.
    /// </summary>
    public class CookieConfig
    {
        /// <summary>
        /// Nombre de la cookie que contiene el token de acceso JWT.
        /// Por defecto: "X-DiezX-Auth-Token"
        /// </summary>
        public string AuthTokenCookieName { get; set; } = "X-DiezX-Auth-Token";

        /// <summary>
        /// Nombre de la cookie que contiene el refresh token.
        /// Por defecto: "X-DiezX-Refresh-Token"
        /// </summary>
        public string RefreshTokenCookieName { get; set; } = "X-DiezX-Refresh-Token";

        /// <summary>
        /// Tiempo de vida de las cookies en días.
        /// Por defecto: 30 días
        /// </summary>
        public int LifetimeDays { get; set; } = 30;

        /// <summary>
        /// Si las cookies deben ser HttpOnly.
        /// Por defecto: true
        /// </summary>
        public bool HttpOnly { get; set; } = true;

        /// <summary>
        /// Si las cookies deben ser Secure (solo HTTPS).
        /// Por defecto: true
        /// </summary>
        public bool Secure { get; set; } = true;

        /// <summary>
        /// Configuración de SameSite para las cookies.
        /// Por defecto: "Strict"
        /// </summary>
        public string SameSite { get; set; } = "Strict";
    }
} 