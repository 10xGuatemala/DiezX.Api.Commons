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
    /// Constantes utilizadas en el sistema de correo electrónico
    /// </summary>
    public static class MailConstants
    {
        /// <summary>
        /// Nombre de la compañía que envía el correo
        /// </summary>
        public const string SENDER_COMPANY = "SenderCompany";

        /// <summary>
        /// Nombre del sistema que envía el correo
        /// </summary>
        public const string SENDER_SYSTEM = "SenderSystem";

        /// <summary>
        /// Nombre de usuario para las plantillas de correo
        /// </summary>
        public const string USERNAME = "Username";

        /// <summary>
        /// URL base para la recuperación de contraseña
        /// </summary>
        public const string RECOVERY_URL = "RecoveryUrl";

        /// <summary>
        /// Tiempo de expiración del token
        /// </summary>
        public const string TOKEN_EXPIRATION = "TokenExpiration";

        /// <summary>
        /// Identificador del proceso
        /// </summary>
        public const string PROCESS_ID = "ProcessId";

        /// <summary>
        /// Descripción del motivo de rechazo
        /// </summary>
        public const string REJECTION_DESC = "RejectionDesc";

        /// <summary>
        /// Código MFA para autenticación de dos factores
        /// </summary>
        public const string MFA_CODE = "MfaCode";

        /// <summary>
        /// Tiempo de expiración del código
        /// </summary>
        public const string CODE_EXPIRATION = "CodeExpiration";

        /// <summary>
        /// Claim para el identificador del proceso
        /// </summary>
        public const string PROCESS_ID_CLAIM = "processId";
    }
} 