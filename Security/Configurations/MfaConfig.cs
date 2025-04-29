
//
//  Copyright © 2025 10X de Guatemala, S.A.
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
namespace DiezX.Api.Commons.Security
{
    /// <summary>
    /// Configuración para la autenticación multifactor (MFA) mediante TOTP (Time-Based One-Time Password).
    /// </summary>
    public class MfaConfig
    {
        /// <summary>
        /// Clave secreta en formato Base32 utilizada para generar y validar códigos TOTP.
        /// </summary>
        /// <remarks>
        /// Esta clave debe ser única para cada usuario y mantenerse segura. Se recomienda almacenarla 
        /// de manera cifrada en la base de datos y cargarla desde un sistema seguro.
        /// </remarks>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Número de dígitos en los códigos TOTP generados.
        /// </summary>
        /// <remarks>
        /// Normalmente, los códigos MFA tienen 6 dígitos, pero se pueden configurar con 8 para mayor seguridad.
        /// </remarks>
        public int TotpSize { get; set; } = 6;

        /// <summary>
        /// Duración en segundos durante la cual un código TOTP es válido.
        /// </summary>
        /// <remarks>
        /// El valor por defecto es 30 segundos, según la especificación de TOTP (RFC 6238).
        /// Se puede aumentar si hay problemas de sincronización de tiempo entre el servidor y el cliente.
        /// </remarks>
        public int Step { get; set; } = 30;

        
    }

}
