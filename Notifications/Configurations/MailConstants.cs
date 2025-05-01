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
    /// Constantes para los nombres de parámetros principales utilizados en los servicios de correo.
    /// </summary>
    public static class MailConstants
    {
        public const string SENDER_COMPANY = "SenderCompany";
        public const string SENDER_SYSTEM = "SenderSystem";
        public const string USERNAME = "Username";
        public const string RECOVERY_URL = "RecoveryUrl";
        public const string TOKEN_EXPIRATION = "TokenExpiration";
        public const string PROCESS_ID = "ProcessId";
        public const string REJECTION_DESC = "RejectionDesc";
        public const string MFA_CODE = "MfaCode";
        public const string CODE_EXPIRATION = "CodeExpiration";
        
        // Constantes para claims
        public const string PROCESS_ID_CLAIM = "process_id";
    }
} 