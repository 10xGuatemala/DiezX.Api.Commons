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
using DiezX.Api.Commons.Security.Jwt;
using DiezX.Api.Commons.Notifications.Configurations;
using DiezX.Api.Commons.Notifications.Dto;
using DiezX.Api.Commons.Notifications.Utils;
using Microsoft.Extensions.Options;
using DiezX.Api.Commons.Resources;

namespace DiezX.Api.Commons.Notifications.Services
{
    public class DefaultMailSenderService
    {
        private readonly SendMailService _sendMailService;
        private readonly NotificationsConfig _notificationConfig;
        private readonly TokenService _tokenService;
        private readonly ILogger<DefaultMailSenderService> _logger;

        /// <summary>
        /// Constructor de la clase para inicializar servicios necesarios.
        /// </summary>
        /// <param name="logger">Servicio de registro de eventos.</param>
        /// <param name="config">Configuraciones de notificaciones.</param>
        /// <param name="sendMailService">Servicio de envío de correos.</param>
        /// <param name="tokenService">Servicio de creación de tokens.</param>
        public DefaultMailSenderService(ILogger<DefaultMailSenderService> logger,
                                        IOptions<NotificationsConfig> config,
                                        SendMailService sendMailService,
                                        TokenService tokenService)
        {
            _logger = logger;
            _notificationConfig = config.Value;
            _sendMailService = sendMailService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Envía un correo electrónico con el enlace para restablecer la contraseña del usuario.
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        /// <param name="username">Nombre de usuario para acceso.</param>
        /// <param name="email">Correo electrónico del usuario.</param>
        public async Task SendRecoveryMailAsync(string name, string username, string email)
        {
            var recoveryToken = GenerateToken(username);
            var recoveryUrl = GetRecoveryUrl(recoveryToken);
            var tokenExpiration = GetTokenExpiration();

            var parameters = new Dictionary<string, string>
        {
            { "SenderCompany", _notificationConfig.SenderCompany },
            { "SenderSystem", _notificationConfig.SenderSystem },
            { "Username", email },
            { "RecoveryUrl", recoveryUrl },
            { "TokenExpiration", tokenExpiration }
        };

            var subject = $"[no-responder] Bienvenido a tu cuenta del Sistema {_notificationConfig.SenderSystem}";
            await SendEmailAsync(name, email, "FirstPasswordTemplate.html", parameters, subject);
        }

        /// <summary>
        /// Envía un correo electrónico notificando al usuario que su contraseña ha sido actualizada.
        /// </summary>
        /// <param name="name">Nombre del destinatario del correo.</param>
        /// <param name="email">Correo electrónico del destinatario.</param>
        public async Task SendPasswordUpdatedMailAsync(string name, string email)
        {
            var parameters = new Dictionary<string, string>
        {
            { "SenderCompany", _notificationConfig.SenderCompany },
            { "SenderSystem", _notificationConfig.SenderSystem }
        };

            var subject = $"[no-responder] Tu contraseña del Sistema {_notificationConfig.SenderSystem} ha cambiado";
            await SendEmailAsync(name, email, "PasswordUpdatedTemplate.html", parameters, subject);
        }

        /// <summary>
        /// Envía un correo electrónico de confirmación de correo con el enlace y el identificador del proceso.
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        /// <param name="username">Nombre de usuario para acceso.</param>
        /// <param name="email">Correo electrónico del usuario.</param>
        /// <param name="processId">Identificador del proceso de adhesión.</param>
        public async Task SendEmailConfirmationAsync(string name, string username, string email, string processId)
        {
            var recoveryToken = GenerateToken(username);
            var recoveryUrl = GetRecoveryUrl(recoveryToken);
            var tokenExpiration = GetTokenExpiration();

            var parameters = new Dictionary<string, string>
        {
            { "SenderCompany", _notificationConfig.SenderCompany },
            { "processId", processId },
            { "SenderSystem", _notificationConfig.SenderSystem },
            { "Username", email },
            { "RecoveryUrl", recoveryUrl },
            { "TokenExpiration", tokenExpiration }
        };

            var subject = $"[no-responder] Confirmación de gestión de adhesión {processId} al {_notificationConfig.SenderSystem}";
            await SendEmailAsync(name, email, "EmailConfirmationTemplate.html", parameters, subject);
        }

        /// <summary>
        /// Genera un token de recuperación basado en el nombre de usuario.
        /// </summary>
        /// <param name="username">El nombre de usuario para quien se genera el token.</param>
        /// <returns>El token de recuperación generado.</returns>
        private string GenerateToken(string username)
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, username) };
            return _tokenService.Create(claims, _notificationConfig.RecoveryTokenExpiration);
        }

        /// <summary>
        /// Construye la URL de recuperación utilizando el token proporcionado.
        /// </summary>
        /// <param name="token">El token de recuperación.</param>
        /// <returns>La URL completa para la recuperación de contraseña.</returns>
        private string GetRecoveryUrl(string token)
        {
            return $"{_notificationConfig.RecoveryPageUrl}{token}";
        }

        /// <summary>
        /// Calcula la duración de la expiración del token en horas.
        /// </summary>
        /// <returns>La duración en horas.</returns>
        private string GetTokenExpiration()
        {
            return (_notificationConfig.RecoveryTokenExpiration / 3600).ToString();
        }

        /// <summary>
        /// Método de ayuda para enviar correos electrónicos utilizando los parámetros y la plantilla especificada.
        /// </summary>
        /// <param name="name">El nombre del destinatario.</param>
        /// <param name="email">El correo electrónico del destinatario.</param>
        /// <param name="templateName">El nombre del archivo de la plantilla del correo electrónico.</param>
        /// <param name="parameters">Los parámetros para rellenar en la plantilla.</param>
        /// <param name="subject">El asunto del correo electrónico.</param>
        private async Task SendEmailAsync(string name, string email, string templateName, Dictionary<string, string> parameters, string subject)
        {
            var body = TemplateUtil.GetHtmlContent(EmbeddedResourceUtil.GetResource(templateName), parameters);
            var mail = new EmailDto
            {
                Email = email,
                Body = body,
                Name = name,
                Subject = subject
            };

            await _sendMailService.SendEmailAsync(mail);
            _logger.LogInformation("Correo enviado a {Email} con asunto: {Subject}", email, subject);

        }
    }


}



