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
using System.Globalization;

namespace DiezX.Api.Commons.Strings
{
    public static class StringUtils
    {
        private static TextInfo _textInfo;

        /// <summary>
        /// Configura la cultura a utilizar para operaciones de cadena.
        /// </summary>
        /// <param name="cultureName">Nombre de la cultura (ej. "es-GT").</param>
        public static void SetCulture(string cultureName)
        {
            _textInfo = new CultureInfo(cultureName).TextInfo;
        }

        /// <summary>
        /// Convierte una cadena al formato "Title Case" (tipo oración). Los textos que van todas las letras en mayusculas también son convertidas.
        //  Si la cultura o no está configurada (SetCulture), se utiliza la cultura del sistema actual
        /// </summary>
        /// <param name="input">La cadena de entrada.</param>
        /// <returns>La cadena en formato "Title Case".</returns>
        public static string ToTitleCase(string input)
        {
            var textInfo = _textInfo ?? CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLowerInvariant());
        }

    }

}

