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
        /// Inicializa una nueva instancia de la clase <see cref="TokenService"/>.
        /// </summary>
        /// <param name="dateUtil">Servicio para obtener la fecha y hora actual.</param>
        /// <param name="tokenConfig">Configuración para la generación de tokens.</param>
        public TokenService(DateUtil dateUtil, IOptions<TokenConfig> tokenConfig)
        {
            _dateUtil = dateUtil;
            _tokenConfig = tokenConfig.Value;
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

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Genera un token JWT estándar para un usuario y rol especificados.
        /// </summary>
        /// <param name="username">Nombre de usuario para el que se generará el token.</param>
        /// <param name="role">Rol del usuario para el que se generará el token.</param>
        /// <returns>Un token JWT como una cadena de texto.</returns>
        public string CreateStandardToken(string username, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, username)
            };

            return Create(claims, _tokenConfig.DefaultTokenExpiration);
        }

    }

}

