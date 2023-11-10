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
using System.Security.Claims;
using DiezX.Api.Commons.Exceptions;
using DiezX.Api.Commons.Security.Dto;
using DiezX.Api.Commons.Utils;

namespace DiezX.Api.Commons.Repositories.Services
{
    /// <summary>
    /// Servicio para obtener el usuario que esta realizando la peticion http
    /// </summary>
    /// <remarks>
    /// Version: 1.1.0
    /// </remarks>
    public class UsuarioRequestService
    {
        private readonly HttpContext _httpContext;
        private readonly ILogger<UsuarioRequestService> _logger;
        private readonly string anomUser = "anonimo";

        /// <summary>
        /// Constructor para la inyección de dependencias.
        /// </summary>
        /// <param name="httpContextAccessor">Accesor para el contexto HTTP.</param>
        /// <param name="logger">Instancia del logger.</param>
        public UsuarioRequestService(IHttpContextAccessor httpContextAccessor,
            ILogger<UsuarioRequestService> logger)
        {
            this._httpContext = httpContextAccessor.HttpContext
                ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this._logger = logger;
        }
        /// <summary>
        /// Retorna información sobre el usuario que realizó la solicitud, incluyendo sus roles.
        /// </summary>
        /// <returns>Objeto UsuarioRequest que contiene información del usuario.</returns>
        public UsuarioRequestDto GetRequestUser()
        {
            var currentUser = _httpContext.User?.Identity?.Name ?? anomUser;

            var roles = _httpContext.User?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();

            if (!currentUser.Equals(anomUser) && CollectionUtil.IsEmpty(roles))
                throw new ApiException(StatusCodes.Status404NotFound, $"No se encontraron roles para el usuario {currentUser}");

            _logger.LogInformation($"El usuario que realiza la petición es: {currentUser}");

            return new UsuarioRequestDto(currentUser, roles);
        }

    }
}

