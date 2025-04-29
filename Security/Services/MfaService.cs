//
//    Copyright © 2025 10X de Guatemala, S.A.
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
using OtpNet;
using Microsoft.Extensions.Options;
using System;

namespace DiezX.Api.Commons.Security
{
    /// <summary>
    /// Servicio para la autenticación multifactor (MFA) utilizando el algoritmo TOTP (Time-Based One-Time Password).
    /// Implementa la generación y validación de códigos TOTP basados en una clave secreta.
    /// </summary>
    public class MfaService
    {
        private readonly MfaConfig _config;

        /// <summary>
        /// Constructor que inyecta la configuración de MFA desde <c>appsettings.json</c>.
        /// </summary>
        /// <param name="config">Objeto de configuración <see cref="MfaConfig"/> con los valores de TOTP.</param>
        public MfaService(IOptions<MfaConfig> config)
        {
            _config = config.Value;
        }

        /// <summary>
        /// Genera un código TOTP basado en la clave secreta almacenada en la configuración.
        /// </summary>
        /// <returns>Código TOTP de 6 dígitos válido para autenticación MFA.</returns>
        /// <exception cref="InvalidOperationException">Se lanza si la clave secreta no está configurada.</exception>
        public string Create()
        {
            if (string.IsNullOrWhiteSpace(_config.SecretKey))
                throw new InvalidOperationException("La clave secreta de MFA no está configurada.");

            return Create(_config.SecretKey);
        }

        /// <summary>
        /// Genera un código TOTP basado en una clave secreta proporcionada.
        /// </summary>
        /// <param name="secretKey">Clave secreta en formato Base32 utilizada para generar el código.</param>
        /// <returns>Código TOTP de 6 dígitos válido para autenticación MFA.</returns>
        /// <exception cref="ArgumentException">Se lanza si la clave secreta es inválida.</exception>
        public string Create(string secretKey)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentException("La clave secreta no puede estar vacía.", nameof(secretKey));

            try
            {
                var totp = new Totp(Base32Encoding.ToBytes(secretKey), _config.Step, OtpHashMode.Sha1, _config.TotpSize);
                return totp.ComputeTotp();
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al generar el código TOTP.", ex);
            }
        }

        /// <summary>
        /// Valida un código TOTP ingresado por el usuario utilizando la clave secreta de la configuración.
        /// </summary>
        /// <param name="code">Código TOTP ingresado por el usuario.</param>
        /// <returns><c>true</c> si el código es válido; de lo contrario, <c>false</c>.</returns>
        /// <exception cref="InvalidOperationException">Se lanza si la clave secreta no está configurada.</exception>
        public bool Validate(string code)
        {
            if (string.IsNullOrWhiteSpace(_config.SecretKey))
                throw new InvalidOperationException("La clave secreta de MFA no está configurada.");

            return Validate(_config.SecretKey, code);
        }

        /// <summary>
        /// Valida un código TOTP ingresado por el usuario con una clave secreta proporcionada.
        /// </summary>
        /// <param name="secretKey">Clave secreta en formato Base32 utilizada para validar el código.</param>
        /// <param name="code">Código TOTP ingresado por el usuario.</param>
        /// <returns><c>true</c> si el código es válido; de lo contrario, <c>false</c>.</returns>
        /// <exception cref="ArgumentException">Se lanza si la clave secreta o el código son inválidos.</exception>
        public bool Validate(string secretKey, string code)
        {
            if (string.IsNullOrWhiteSpace(secretKey))
                throw new ArgumentException("La clave secreta no puede estar vacía.", nameof(secretKey));

            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("El código no puede estar vacío.", nameof(code));

            try
            {
                var totp = new Totp(Base32Encoding.ToBytes(secretKey), _config.Step, OtpHashMode.Sha1, _config.TotpSize);
                var verificationWindow = new VerificationWindow(previous: 1, future: 1);
                return totp.VerifyTotp(code, out _, verificationWindow);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error al validar el código TOTP.", ex);
            }
        }
    }
}
