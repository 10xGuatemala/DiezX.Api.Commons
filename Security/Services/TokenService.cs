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
using DiezX.Api.Commons.ExceptionHandlers.Exceptions;
using DiezX.Api.Commons.Exceptions;
using DiezX.Api.Commons.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DiezX.Api.Commons.Security.Jwt
{
    /// <summary>
    /// Servicio para la generación de tokens JWT.
    /// </summary>
    public class TokenService
    {
        /// <summary>
        /// Servicio para obtener la fecha y hora actual con precisión.
        /// </summary>
        private readonly DateUtil _dateUtil;

        /// <summary>
        /// Configuración necesaria para la generación de tokens.
        /// </summary>
        private readonly TokenConfig _tokenConfig;

        /// <summary>
        /// Servicio de loggeo
        /// </summary>
        private readonly ILogger<TokenService> _logger;

        /// <summary>
        /// Constructor para injección de servicios
        /// </summary>
        /// <param name="dateUtil"></param>
        /// <param name="tokenConfig"></param>
        /// <param name="logger"></param>
        public TokenService(DateUtil dateUtil, IOptions<TokenConfig> tokenConfig, ILogger<TokenService> logger)
        {
            _dateUtil = dateUtil;
            _tokenConfig = tokenConfig.Value;
            _logger = logger;
        }
        /// <summary>
        /// Genera un token JWT basado en los claims proporcionados y la duración especificada del token.
        /// </summary>
        /// <param name="claims">Una lista de claims que se incluirán en el token.</param>
        /// <param name="tokenLifeTime">La duración del token en horas.</param>
        /// <returns>Un token JWT como una cadena de texto.</returns>
        public string Create(List<Claim> claims, long tokenLifeTime)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret));
            var now = _dateUtil.GetTime();
            var expirationTime = now.AddSeconds(tokenLifeTime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationTime,
                NotBefore = now,
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            _logger.LogInformation("Token creado exitosamente para {Claims} con duración de {Lifetime} segundos.", claims, tokenLifeTime);


            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Crea un token estándar para un usuario con una lista de roles.
        /// </summary>
        /// <param name="username">El nombre de usuario para el cual se crea el token.</param>
        /// <param name="roles">Una lista de roles asociados con el usuario. Cada rol se agregará como un reclamo separado.</param>
        /// <returns>Una representación en cadena del token creado.</returns>
        /// <remarks>
        /// Este método genera un token que incluye tanto el nombre de usuario como los roles del usuario como reclamos (claims).
        /// Los roles se agregan como reclamos individuales del tipo ClaimTypes.Role.
        /// Esto permite un control detallado y flexibilidad en el manejo de permisos y roles de los usuarios.
        /// La expiración del token se establece en base a la configuración _tokenConfig.DefaultTokenExpiration.
        /// </remarks>
        public string CreateStandardToken(string username, List<string> roles)
        {
            var claims = new List<Claim>
            {
                // Agrega el nombre de usuario como un reclamo
                new Claim(ClaimTypes.Name, username)
            };

            // Itera sobre la lista de roles y agrega cada uno como un reclamo
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Crea el token con la lista de reclamos
            return Create(claims, _tokenConfig.DefaultTokenExpiration);
        }


        /// <summary>
        /// Decodifica un token JWT y valida su integridad y vigencia.
        /// </summary>
        /// <param name="token">El token JWT que será decodificado.</param>
        /// <returns>Un ClaimsPrincipal derivado del token.</returns>
        /// <exception cref="TokenExpiredException">Se lanza si el token ha expirado.</exception>
        /// <exception cref="ApiGeneralException">Se lanza si el token no contiene claims válidos.</exception>
        public ClaimsPrincipal Decode(string token)
        {
            // Manejador para los tokens JWT.
            var handler = new JwtSecurityTokenHandler();

            // Parámetros de validación para el token.
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ClockSkew = TimeSpan.Zero
            };

            // Validación del token y obtención de los claims.
            var claimsPrincipal = handler.ValidateToken(token, validations, out var securityToken);

            // Obtener el momento actual utilizando el servicio DateUtil.
            DateTime now = _dateUtil.GetTime();

            // Verificar si el token ha expirado.
            if (now > securityToken.ValidTo)
            {
                _logger.LogWarning("Intento de uso de token expirado. Fecha actual {Now}, fecha de expiración del token {ValidTo}", now, securityToken.ValidTo);
                throw new TokenExpiredException("Lo sentimos, el token que está utilizando ya no es válido");
            }

            // Verificar si el token contiene claims.
            if (claimsPrincipal == null || CollectionUtil.IsEmpty(claimsPrincipal.Claims))
            {
                _logger.LogWarning("Intento de uso de token sin información {ClaimsPrincipal}", claimsPrincipal);
                throw new ApiGeneralException(StatusCodes.Status422UnprocessableEntity, "El token no contiene información válida");
            }

            // Retornar el ClaimsPrincipal obtenido del token.
            return claimsPrincipal;
        }

    }

}

