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
using DiezX.Api.Commons.Security.Jwt;
using DiezX.Api.Commons.Notifications.Configurations;
using DiezX.Api.Commons.Notifications.Dto;
using DiezX.Api.Commons.Notifications.Utils;
using Microsoft.Extensions.Options;
using DiezX.Api.Commons.Resources;

namespace DiezX.Api.Commons.Notifications.Services
{
    /// <summary>
    /// Gestiona el envío de varios tipos de notificaciones por correo electrónico.
    /// </summary>
    public class DefaultMailSenderService
    {
        private readonly SendMailService _sendMailService;
        private readonly NotificationsConfig _notificationConfig;
        private readonly TokenService _tokenService;
        private readonly ILogger<DefaultMailSenderService> _logger;

        private const string PROCESS_ID_CLAIM = "process_id";

        /// <summary>
        /// Inicializa una nueva instancia de la clase DefaultMailSenderService.
        /// </summary>
        /// <param name="logger">Logger para el registro de eventos.</param>
        /// <param name="config">Configuraciones de notificación.</param>
        /// <param name="sendMailService">Servicio para enviar correos electrónicos.</param>
        /// <param name="tokenService">Servicio para generar tokens.</param>
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
        /// Envía un correo electrónico con el enlace para establecer la primera contraseña a un nuevo usuario.
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        /// <param name="username">Nombre de usuario para el inicio de sesión.</param>
        /// <param name="email">Dirección de correo electrónico del usuario.</param>
        /// <param name="rol">Rol del usuario en el sistema.</param>
        public async Task SendFirstPasswordMailAsync(string name, string username, string email, string rol)
        {
            string subject = $"Bienvenido a tu cuenta del Sistema {_notificationConfig.SenderSystem}";
            await SendRecoveryWithTokenAsync(name, username, email, rol, "FirstPasswordTemplate.html", subject);
        }

        /// <summary>
        /// Envía un correo electrónico de recuperación de contraseña al usuario.
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        /// <param name="username">Nombre de usuario para el inicio de sesión.</param>
        /// <param name="email">Dirección de correo electrónico del usuario.</param>
        /// <param name="rol">Rol del usuario en el sistema.</param>
        public async Task SendPasswordRecoveryMailAsync(string name, string username, string email, string rol)
        {
            string subject = $"Solicitud de recuperación de contraseña del Sistema {_notificationConfig.SenderSystem}";
            await SendRecoveryWithTokenAsync(name, username, email, rol, "PasswordRecoveryTemplate.html", subject);
        }

        /// <summary>
        /// Envía un correo electrónico notificando al usuario que su contraseña ha sido actualizada.
        /// </summary>
        /// <param name="name">Nombre del destinatario.</param>
        /// <param name="email">Dirección de correo electrónico del destinatario.</param>
        public async Task SendPasswordUpdatedMailAsync(string name, string email)
        {
            var parameters = new Dictionary<string, string>
            {
                { "SenderCompany", _notificationConfig.SenderCompany },
                { "SenderSystem", _notificationConfig.SenderSystem }
            };

            string subject = $"Tu contraseña del Sistema {_notificationConfig.SenderSystem} ha sido actualizada.";
            await SendEmailAsync(name, email, "PasswordUpdatedTemplate.html", parameters, subject);
        }

        /// <summary>
        /// Envía un correo electrónico de confirmación con el enlace y el identificador del proceso.
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        /// <param name="username">Nombre de usuario para el inicio de sesión.</param>
        /// <param name="email">Dirección de correo electrónico del usuario.</param>
        /// <param name="processId">Identificador del proceso de la gestión</param>
        /// <param name="codigoSolicitud">GUID de solicitud.</param>
        public async Task SendConfirmationEmailAsync(string name, string username, string email, string processId, string codigoSolicitud)
        {
            var recoveryToken = GenerateTokenProcess(username, processId);
            var recoveryUrl = GetConfirmationUrl(recoveryToken, codigoSolicitud);
            var tokenExpiration = GetTokenExpiration();

            var parameters = new Dictionary<string, string>
            {
                { "SenderCompany", _notificationConfig.SenderCompany },
                { "ProcessId", processId },
                { "SenderSystem", _notificationConfig.SenderSystem },
                { "Username", email },
                { "RecoveryUrl", recoveryUrl },
                { "TokenExpiration", tokenExpiration }
            };

            string subject = $"Confirmación de gestión de adhesión {processId} al {_notificationConfig.SenderSystem}";
            await SendEmailAsync(name, email, "ConfirmationEmailTemplate.html", parameters, subject);
        }

        /// <summary>
        /// Envía un correo electrónico informando de la denegación de adhesión 
        /// </summary>
        /// <param name="name">Nombre del usuario.</param>
        /// <param name="email">Dirección de correo electrónico del usuario.</param>
        /// <param name="processId">Identificador del proceso de unión.</param>
        /// <param name="rejectionDesc">Descripción o motivo del rechazo</param>
        public async Task SendRejectionMailAsync(string name, string email, string processId, string rejectionDesc)
        {
            var parameters = new Dictionary<string, string>
            {
                { "SenderCompany", _notificationConfig.SenderCompany },
                { "SenderSystem", _notificationConfig.SenderSystem },
                { "ProcessId", processId },
                { "RejectionDesc", rejectionDesc }
            };

            string subject = $"Tu solicitud de Adhesión al Sistema {_notificationConfig.SenderSystem} ha sido denegada.";
            await SendEmailAsync(name, email, "RejectionTemplate.html", parameters, subject);
        }

        /// <summary>
        /// Auxiliar para enviar correos electrónicos que requieren un token de autenticación o recuperación.
        /// </summary>
        /// <param name="name">Nombre del destinatario del correo.</param>
        /// <param name="username">Nombre de usuario para el token.</param>
        /// <param name="email">Correo electrónico del destinatario.</param>
        /// <param name="rol">Rol del usuario para el token.</param>
        /// <param name="templateName">Nombre de la plantilla de correo electrónico a utilizar.</param>
        /// <param name="subject">Asunto del correo electrónico.</param>
        private async Task SendRecoveryWithTokenAsync(string name, string username, string email, string rol, string templateName, string subject)
        {
            var recoveryToken = GenerateTokenRol(username, rol);
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

            await SendEmailAsync(name, email, templateName, parameters, subject);
        }

        /// <summary>
        /// Genera un token de recuperación basado en el nombre de usuario y el identificador del proceso.
        /// </summary>
        /// <param name="username">El nombre de usuario para el cual se generará el token.</param>
        /// <param name="processId">El identificador del proceso asociado con la acción del token.</param>
        /// <returns>Una cadena que representa el token de recuperación generado.</returns>
        /// <remarks>
        /// Crea un token de recuperación que incluye claims:
        /// el nombre de usuario (como ClaimTypes.NameIdentifier) y el identificador del proceso.
        /// El token es creado utilizando el servicio de token configurado y tiene un período
        /// de validez definido por la configuración de expiración del token de recuperación.
        /// Este token se utiliza típicamente para operaciones que requieren una confirmación
        /// de identidad o proceso, como la confirmación de email o la recuperación de contraseña.
        /// </remarks>
        private string GenerateTokenProcess(string username, string processId)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, username),
                new Claim(PROCESS_ID_CLAIM, processId)
            };

            return _tokenService.Create(claims, _notificationConfig.RecoveryTokenExpiration);
        }


        /// <summary>
        /// Genera un token de autenticación para un usuario con su rol específico.
        /// </summary>
        /// <param name="username">El nombre de usuario para el cual se generará el token.</param>
        /// <param name="rol">El rol del usuario que se incluirá en el token.</param>
        /// <returns>Una cadena que representa el token de autenticación generado.</returns>
        private string GenerateTokenRol(string username, string rol)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, rol)
            };

            return _tokenService.Create(claims, _notificationConfig.RecoveryTokenExpiration);
        }

        /// <summary>
        /// Construye la URL de recuperación utilizando el token proporcionado.
        /// </summary>
        /// <param name="token">El token de recuperación.</param>
        /// <returns>La URL completa para la recuperación de contraseña.</returns>
        private string GetRecoveryUrl(string token)
        {
            return _notificationConfig.RecoveryPageUrl.Replace(":token", token);
        }

        /// <summary>
        /// Construye la URL de confirmación utilizando el token proporcionado y el código de solicitud.
        /// </summary>
        /// <param name="token">El token de confirmación.</param>
        /// <param name="codigoSolicitud">El código de solicitud para la operación.</param>
        /// <returns>La URL completa para la operación de confirmación.</returns>
        private string GetConfirmationUrl(string token, string codigoSolicitud)
        {
            return _notificationConfig.ConfirmationPageUrl.Replace(":token", token).Replace(":codigoSolicitud", codigoSolicitud);
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
        /// Enviar correos electrónicos utilizando los parámetros y la plantilla especificada.
        /// </summary>
        /// <param name="name">El nombre del destinatario.</param>
        /// <param name="email">El correo electrónico del destinatario.</param>
        /// <param name="templateName">El nombre del archivo de la plantilla del correo electrónico.</param>
        /// <param name="parameters">Los parámetros para rellenar en la plantilla.</param>
        /// <param name="subject">El asunto del correo electrónico.</param>
        private async Task SendEmailAsync(string name, string email, string templateName, Dictionary<string, string> parameters, string subject)
        {
            var body = TemplateUtil.GetHtmlContent(EmbeddedResourceUtil.GetResource(templateName), parameters);
            _logger.LogInformation("Preparando contenido del email con la plantilla {TemplateName}", templateName);
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

        /// <summary>
        /// Decodifica un token de confirmación de correo y extrae la información relevante.
        /// </summary>
        /// <param name="token">El token JWT que será decodificado.</param>
        /// <returns>
        /// Un objeto <see cref="MailTokenDecodedDto"/> que contiene información extraída del token,
        /// como el nombre de usuario y el ID del proceso.
        /// </returns>
        /// <remarks>
        /// Este método utiliza <see cref="TokenService.Decode"/> para obtener los claims del token.
        /// Luego, extrae y retorna información específica como el nombre de usuario (Username)
        /// y el ID del proceso (ProcessId) basándose en los claims del token.
        /// </remarks>
        public MailTokenDecodedDto DecodeMailConfirmationToken(string token)
        {
            // Decodifica el token para obtener los claims
            ClaimsPrincipal tokenClaims = _tokenService.Decode(token);

            var mailToken = new MailTokenDecodedDto()
            {
                Username = tokenClaims.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                ProcessId = tokenClaims.FindFirst(PROCESS_ID_CLAIM)?.Value,
            };

            _logger.LogInformation("Información del token: User: {Username}, Proceso: {ProcessId}",
                mailToken.Username, mailToken.ProcessId);

            // Retorna un nuevo objeto MailTokenDecodedDto con la información extraída
            return mailToken;
        }

    }
}

