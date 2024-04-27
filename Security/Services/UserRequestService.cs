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
using System.Security.Claims;
using DiezX.Api.Commons.Exceptions;
using DiezX.Api.Commons.Security.Dto;
using DiezX.Api.Commons.Utils;

namespace DiezX.Api.Commons.Repositories.Services
{
    /// <summary>
    /// Servicio para obtener información sobre el usuario que realiza una solicitud HTTP.
    /// </summary>
    /// <remarks>
    /// Utiliza IHttpContextAccessor para acceder al contexto HTTP actual y extraer información de usuario.
    /// Este servicio debe utilizarse dentro del ámbito de una solicitud HTTP activa para que el contexto HTTP no sea null.
    /// </remarks>
    public class UsuarioRequestService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<UsuarioRequestService> _logger;
        private readonly string _anonymousUser = "anonimo";

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="UsuarioRequestService"/>.
        /// </summary>
        /// <param name="httpContextAccessor">Accesor para el contexto HTTP.</param>
        /// <param name="logger">Instancia del logger para registrar información.</param>
        /// <exception cref="ArgumentNullException">Se lanza si httpContextAccessor es null.</exception>
        public UsuarioRequestService(IHttpContextAccessor httpContextAccessor, ILogger<UsuarioRequestService> logger)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Obtiene la información del usuario que realiza la solicitud HTTP actual.
        /// </summary>
        /// <returns>
        /// Un <see cref="UsuarioRequestDto"/> que contiene el nombre del usuario y sus roles.
        /// </returns>
        /// <exception cref="ApiGeneralException">
        /// Se lanza si no hay un contexto HTTP o si no se pueden encontrar roles para el usuario autenticado.
        /// </exception>
        public UsuarioRequestDto GetRequestUser()
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new ApiGeneralException(StatusCodes.Status401Unauthorized, "No se puede obtener el usuario sin un contexto HTTP.");
            var currentUser = httpContext.User?.Identity?.Name ?? _anonymousUser;
            var roles = httpContext.User?.FindAll(ClaimTypes.Role).Select(x => x.Value).ToArray();

            if (!currentUser.Equals(_anonymousUser) && (roles == null || !roles.Any()))
            {
                throw new ApiGeneralException(StatusCodes.Status404NotFound, $"No se encontraron roles para el usuario {currentUser}");
            }

            _logger.LogInformation("El usuario que realiza la petición es: {CurrentUser}", currentUser);

            return new UsuarioRequestDto(currentUser, roles);
        }
    }
}

