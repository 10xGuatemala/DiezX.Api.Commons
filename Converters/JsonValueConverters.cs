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

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace DiezX.Api.Commons.Converters
{
    /// <summary>
    /// Fábrica de conversores JSON para columnas tipadas como json en la base de datos.
    /// </summary>
    public static class JsonValueConverters
    {
        private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
            WriteIndented = false
        };

        /// <summary>
        /// Crea un conversor Nullable para tipos complejos que se serializan como JSON.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto que se serializa.</typeparam>
        public static ValueConverter<T?, string?> CreateNullableConverter<T>() where T : class =>
            new(
                v => v == null ? null : JsonSerializer.Serialize(v, SerializerOptions),
                v => string.IsNullOrWhiteSpace(v) ? default(T) : JsonSerializer.Deserialize<T>(v!, SerializerOptions)
            );

        /// <summary>
        /// Conversor especializado para JsonDocument manteniendo la estructura original.
        /// </summary>
        public static ValueConverter<JsonDocument?, string?> CreateJsonDocumentConverter() =>
            new(
                v => v == null ? null : v.RootElement.GetRawText(),
                v => string.IsNullOrWhiteSpace(v) ? null : JsonDocument.Parse(v!, default)
            );
    }
}
