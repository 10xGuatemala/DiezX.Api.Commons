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
using DiezX.Api.Commons.ExceptionHandlers.Exceptions;
using DiezX.Api.Commons.Exceptions;
using DiezX.Api.Commons.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        /// Genera un token JWT standard basado en los claims proporcionados y la duración especificada del token.
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
        /// Crea un token JWT utilizando una lista de reclamos, tiempo de vida del token y credenciales de firma.
        /// </summary>
        /// <param name="claims">Una lista de <see cref="Claim"/> que se incluirán en el JWT.</param>
        /// <param name="tokenLifeTime">La duración del token en segundos.</param>
        /// <param name="sign">Las credenciales de firma utilizadas para firmar el token.</param>
        /// <returns>Un string que representa el token JWT firmado.</returns>
        /// <remarks>
        /// Este método utiliza <see cref="JwtSecurityTokenHandler"/> para crear un token JWT.
        /// Se establece un tiempo de expiración para el token basado en el parámetro <paramref name="tokenLifeTime"/>.
        /// La fecha y hora actuales se obtienen utilizando una instancia de <c>_dateUtil.GetTime()</c>, que define el momento de creación y el momento antes del cual el token no es válido (<see cref="SecurityTokenDescriptor.NotBefore"/>).
        /// Se registra un mensaje informativo una vez que el token es creado exitosamente.
        /// </remarks>
        private string Create(List<Claim> claims, long tokenLifeTime, SigningCredentials sign)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = _dateUtil.GetTime();
            var expirationTime = now.AddSeconds(tokenLifeTime);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expirationTime,
                NotBefore = now,
                SigningCredentials = sign
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
            List<Claim> claims = ToClaimList(username, roles);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret));
            SigningCredentials sign = new(key, SecurityAlgorithms.HmacSha256Signature);

            // Crear y retorna como cadena el token JWT basado en la descripción proporcionada.
            return Create(claims, _tokenConfig.DefaultTokenExpiration, sign);
        }

        /// <summary>
        /// Crea un JWT utilizando una clave RSA privada.
        /// </summary>
        /// <returns>Un string que representa el token JWT firmado.</returns>
        public string CreateRSAToken(string username, List<string> roles)
        {
            // Crear una instancia de RSA para el manejo de la criptografía.
            using RSA rsa = RSA.Create();
            // Importar la clave privada desde un archivo PEM.
            rsa.ImportFromPem(File.ReadAllText(_tokenConfig.RsaKeyPath));

            // Configurar las credenciales de firma utilizando el algoritmo RSA SHA-256.
            var sign = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256);

            // Transformar los datos a una lista de claim
            List<Claim> claims = ToClaimList(username, roles);

            // Crear y retorna como cadena el token JWT basado en la descripción proporcionada.
            return Create(claims, _tokenConfig.DefaultTokenExpiration, sign);
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

        /// <summary>
        /// Convierte un nombre de usuario y una lista de roles en una lista de objetos <see cref="Claim"/>.
        /// </summary>
        /// <param name="username">El nombre de usuario para el cual se generan los reclamos.</param>
        /// <param name="roles">Una lista de roles que se incluirán como reclamos de tipo 'Role'.</param>
        /// <returns>Una lista de objetos <see cref="Claim"/> que contienen el nombre de usuario y los roles como reclamos.</returns>
        /// <remarks>
        /// Este método crea un reclamo inicial con el tipo <see cref="ClaimTypes.Name"/> para el nombre de usuario.
        /// Luego, itera sobre la lista de roles proporcionada, creando un reclamo para cada rol con el tipo <see cref="ClaimTypes.Role"/>.
        /// Cada uno de estos reclamos se agrega a la lista de reclamos que se devuelve.
        /// </remarks>
        public static List<Claim> ToClaimList(string username, List<string> roles)
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

            return claims;

        }

    }

}

