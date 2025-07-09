//
//  Copyright © 2024 10X de Guatemala, S.A.
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiezX.Api.Commons.Converters
{
    /// <summary>
    /// Clase base para convertidores de fecha en JSON
    /// </summary>
    /// <typeparam name="T">Tipo de fecha a convertir (DateTime o DateTimeOffset)</typeparam>
    public abstract class JsonDateConverterBase<T> : JsonConverter<T>
    {
        /// <summary>
        /// Formato de fecha utilizado para la serialización/deserialización
        /// </summary>
        protected readonly string _dateFormat;

        /// <summary>
        /// Constructor que establece el formato de la fecha.
        /// </summary>
        protected JsonDateConverterBase(string dateFormat)
        {
            _dateFormat = dateFormat;
        }

        /// <summary>
        /// Lectura abstracta para la deserialización.
        /// </summary>
        public abstract override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options);

        /// <summary>
        /// Escritura abstracta para la serialización.
        /// </summary>
        public abstract override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options);

    }

    /// <summary>
    /// Convertidor de formato para DateTime en (des)serialización de JSON.
    /// </summary>
    public class JsonDateTimeConverter : JsonDateConverterBase<DateTime>
    {
        /// <summary>
        /// Constructor con un formato específico o predeterminado.
        /// </summary>
        public JsonDateTimeConverter(string? dateFormat = null)
        : base(dateFormat ?? "dd/MM/yyyy") { }

        /// <summary>
        /// Deserialización de DateTime.
        /// </summary>
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            DateTime.ParseExact(reader.GetString()!, _dateFormat, CultureInfo.InvariantCulture);

        /// <summary>
        /// Serialización de DateTime.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString(_dateFormat, CultureInfo.InvariantCulture));
    }

    /// <summary>
    /// Convertidor de formato para DateTimeOffset en (des)serialización de JSON.
    /// </summary>
    public class JsonDateTimeOffsetConverter : JsonDateConverterBase<DateTimeOffset>
    {
        /// <summary>
        /// Constructor con un formato específico o predeterminado.
        /// </summary>
        public JsonDateTimeOffsetConverter(string? dateFormat = null)
        : base(dateFormat ?? "dd/MM/yyyy HH:mm:ss") { }

        /// <summary>
        /// Deserialización de DateTimeOffset.
        /// </summary>
        public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
            DateTimeOffset.ParseExact(reader.GetString(), _dateFormat, CultureInfo.InvariantCulture);

        /// <summary>
        /// Serialización de DateTimeOffset.
        /// </summary>
        public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString(_dateFormat, CultureInfo.InvariantCulture));


    }
}
