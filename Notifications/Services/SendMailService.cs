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
using DiezX.Api.Commons.Exceptions;
using DiezX.Api.Commons.Notifications.Configurations;
using DiezX.Api.Commons.Notifications.Dto;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DiezX.Api.Commons.Notifications.Services
{

    /// <summary>
    /// Servicio para el envío de correos electrónicos
    /// </summary>
    public class SendMailService
    {
        private readonly NotificationsConfig _notificationConfig;
        private readonly ILogger<SendMailService> _logger;
        // La instancia de SmtpClient se crea en cada envío para evitar problemas de concurrencia.

        //constante
        const string ERROR_ENVIO = "Se produjo un error al enviar un correo electrónico a los destinatarios especificados.";

        /// <summary>
        /// Inicializa una nueva instancia del servicio de envío de correos
        /// </summary>
        /// <param name="notificationConfig">Configuración de notificaciones</param>
        /// <param name="logger">Logger para el servicio</param>
        public SendMailService(
            IOptions<NotificationsConfig> notificationConfig,
            ILogger<SendMailService> logger)
        {
            _notificationConfig = notificationConfig.Value;
            _logger = logger;
            // La instancia de SmtpClient se crea en el método SendEmailAsync.
        }

        /// <summary>
        /// Crea un mensaje MimeMessage con los detalles proporcionados.
        /// </summary>
        /// <param name="name">Nombre del destinatario.</param>
        /// <param name="email">Correo electrónico del destinatario.</param>
        /// <param name="subject">Asunto del correo electrónico.</param>
        /// <param name="body">Cuerpo del correo electrónico.</param>
        /// <returns>Un objeto MimeMessage.</returns>
        private MimeMessage CreateMimeMessage(string name, string email, string subject, string body)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("no-responder", _notificationConfig.MailSender));
            mimeMessage.To.Add(new MailboxAddress(name, email));
            mimeMessage.Subject = subject;
            var builder = new BodyBuilder
            {
                HtmlBody = body,
                TextBody = "Este correo contiene contenido HTML."
            };
            mimeMessage.Body = builder.ToMessageBody();
            return mimeMessage;
        }

        /// <summary>
        /// Envía un correo electrónico con los detalles proporcionados.
        /// </summary>
        /// <param name="emailDto">Detalles del correo electrónico.</param>
        /// <returns>Task.</returns>
        public async Task SendEmailAsync(EmailDto emailDto)
        {
            var message = CreateMimeMessage(emailDto.Name, emailDto.Email, emailDto.Subject, emailDto.Body);
            await SendEmailAsync(message);
        }

        /// <summary>
        /// Envía un correo electrónico utilizando la configuración proporcionada.
        /// </summary>
        /// <param name="message">Mensaje a enviar.</param>
        /// <returns>Una Task.</returns>
        public async Task SendEmailAsync(MimeMessage message)
        {
            try
            {
                // Crear una instancia de SmtpClient por envío
                using var smtp = new SmtpClient();

                // Seleccionar opciones de socket según UseTls: StartTlsWhenAvailable negocia TLS cuando el servidor lo permite
                var options = _notificationConfig.UseTls
                    ? SecureSocketOptions.StartTlsWhenAvailable
                    : SecureSocketOptions.None;

                _logger.LogInformation("Configurando correo para envío a {UserName}", _notificationConfig.UserName);

                await smtp.ConnectAsync(_notificationConfig.SmtpServer, _notificationConfig.Port, options);

                // Autenticar sólo si UseAuthentication es verdadero y el servidor soporta autenticación
                if (_notificationConfig.UseAuthentication &&
                    smtp.Capabilities.HasFlag(MailKit.Net.Smtp.SmtpCapabilities.Authentication))
                {
                    // Quitar XOAUTH2 si el servidor lo anuncia pero no se usa
                    smtp.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Validar que haya credenciales definidas
                    if (string.IsNullOrWhiteSpace(_notificationConfig.UserName) ||
                        string.IsNullOrWhiteSpace(_notificationConfig.Password))
                    {
                        throw new InvalidOperationException("Para usar autenticación SMTP se deben configurar UserName y Password.");
                    }

                    await smtp.AuthenticateAsync(_notificationConfig.UserName, _notificationConfig.Password);
                }

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Correo enviado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ERROR_ENVIO + " Destinatarios: {Recipients}", string.Join(", ", message.To));
                throw new ApiGeneralException(StatusCodes.Status500InternalServerError, ERROR_ENVIO);
            }
        }


    }

}
