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
using NodaTime;

namespace DiezX.Api.Commons.Utils
{
    /// <summary>
    /// Utilidad para obtener la fecha y hora basada en una zona horaria específica.
    /// Esta clase se deriva de los problemas de representación de tiempo que tiene .NET.
    /// Para más información: https://blog.nodatime.org/2011/08/what-wrong-with-datetime-anyway.html
    /// </summary>
    public class DateUtil
    {
        private readonly DateTimeZone _timeZone;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="DateUtil"/> con una zona horaria específica.
        /// Esta clase debe ser instanciada explícitamente con una zona horaria por defecto.
        /// </summary>
        /// <param name="timeZone">La zona horaria a utilizar.</param>
        public DateUtil(string timeZone)
        {
            if (string.IsNullOrEmpty(timeZone))
            {
                throw new ArgumentException("El parámetro 'timeZone' no puede ser nulo o vacío.", nameof(timeZone));
            }

            _timeZone = DateTimeZoneProviders.Tzdb[timeZone];
        }

        /// <summary>
        /// Obtiene la fecha y hora actual considerando la zona horaria con la que la clase fue instanciada.
        /// El tipo de fecha y hora devuelto es "unspecified" (no especificado).
        /// </summary>
        /// <returns>La fecha y hora actual en la zona horaria configurada.</returns>
        public DateTime GetTime()
        {
            return Instant.FromDateTimeUtc(DateTime.UtcNow)
                .InZone(_timeZone)
                .ToDateTimeUnspecified();
        }

        /// <summary>
        /// Obtiene un <see cref="DateTimeOffset"/> que representa la fecha y hora actual en la zona horaria configurada.
        /// </summary>
        /// <returns>Un <see cref="DateTimeOffset"/> con la fecha y hora actual y el desplazamiento de la zona horaria.</returns>
        public DateTimeOffset GetTimeOffset()
        {
            return Instant.FromDateTimeOffset(DateTimeOffset.Now)
                .InZone(_timeZone)
                .ToDateTimeOffset();
        }

        // Constructor privado para prevenir la instanciación sin especificar la zona horaria.
        private DateUtil()
        {
        }
    }
}
