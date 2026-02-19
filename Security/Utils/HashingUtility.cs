//
//  Copyright 2026  Copyright © 10X de Guatemala, S.A.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//
using System.Security.Cryptography;

namespace DiezX.Api.Commons.Security.Utils
{
    /// <summary>
    /// Proporciona métodos utilitarios para hashear y verificar textos claros, como contraseñas y API Keys.
    /// Utiliza BCrypt para hashing de contraseñas.
    /// </summary>
    public static class HashingUtility
    {
        /// <summary>
        /// Hashea un texto claro utilizando el algoritmo BCrypt.
        /// </summary>
        /// <param name="input">El texto claro que será hasheado.</param>
        /// <returns>El hash generado.</returns>
        /// <example>
        /// <code>
        /// string password = "mySecurePassword";
        /// string hashedPassword = HashingUtility.Hash(password);
        /// </code>
        /// </example>
        public static string Hash(string input)
        {
            return BCrypt.Net.BCrypt.HashPassword(input);
        }

        /// <summary>
        /// Verifica si un texto claro coincide con un hash almacenado.
        /// </summary>
        /// <param name="input">El texto claro.</param>
        /// <param name="hashedInput">El hash almacenado.</param>
        /// <returns>True si el texto claro coincide con el hash almacenado, de lo contrario false.</returns>
        /// <example>
        /// <code>
        /// string password = "mySecurePassword";
        /// string hashedPassword = "..."; // Recuperado de la base de datos
        /// bool isValid = HashingUtility.Verify(password, hashedPassword);
        /// </code>
        /// </example>
        public static bool Verify(string input, string hashedInput)
        {
            return BCrypt.Net.BCrypt.Verify(input, hashedInput);
        }

        /// <summary>
        /// Genera un API key seguro y de mayor longitud.
        /// </summary>
        /// <param name="size">El tamaño en bytes del API key. El valor predeterminado es 64 bytes.</param>
        /// <returns>El API key generado como una cadena Base64.</returns>
        public static string GenerateApiKey(int size = 64)
        {
            var apiKeyBytes = new byte[size];
            RandomNumberGenerator.Fill(apiKeyBytes);
            return Convert.ToBase64String(apiKeyBytes);
        }
    }
}
