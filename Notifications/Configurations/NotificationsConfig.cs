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
namespace DiezX.Api.Commons.Notifications.Configurations
{
    /// <summary>
    /// Configuraciones para los servicios de notificación.
    /// </summary>
    public class NotificationsConfig
    {
        /// <summary>
        /// Obtiene o establece la compañía que envía el correo.
        /// </summary>
        public required string SenderCompany { get; set; }

        /// <summary>
        /// Obtiene o establece el sistema que envía el correo.
        /// </summary>
        public required string SenderSystem { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección del servidor SMTP.
        /// </summary>
        public required string SmtpServer { get; set; }

        /// <summary>
        /// Obtiene o establece el número de puerto para el servidor SMTP.
        /// </summary>
        public required int Port { get; set; }

        /// <summary>
        /// Nombre de usuario para autenticarse en el servidor SMTP. Cuando <see cref="UseAuthentication"/> es false, puede estar vacío.
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// Contraseña para autenticarse en el servidor SMTP. Cuando <see cref="UseAuthentication"/> es false, puede estar vacía.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Obtiene o establece la dirección de correo electrónico que aparecerá como remitente de las notificaciones por correo electrónico.
        /// </summary>
        public required string MailSender { get; set; }

        /// <summary>
        /// Obtiene o establece la URL de la página de recuperación para el restablecimiento de contraseña.
        /// </summary>
        public required string RecoveryPageUrl { get; set; }

        /// <summary>
        /// Obtiene la URL de la página para confirmar correo.
        /// </summary>
        public required string ConfirmationPageUrl { get; set; }

        /// <summary>
        /// Obtiene o establece la duración de vida del token utilizado para la recuperación de contraseña.
        /// </summary>
        public long RecoveryTokenExpiration { get; set; }

        /// <summary>
        /// Bandera para indicar si se utiliza tls para envio de correo seguro.
        /// </summary>
        public bool UseTls { get; set; } = true;

        /// <summary>
        /// Indica si se debe utilizar autenticación SMTP. Si es false, el servicio enviará correos sin llamar a AuthenticateAsync.
        /// </summary>
        public bool UseAuthentication { get; set; } = true;
    }


}

