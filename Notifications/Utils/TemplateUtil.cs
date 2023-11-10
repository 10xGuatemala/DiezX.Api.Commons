//
//  Copyright 2023  Copyright Soluciones Modernas 10x
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
using System.Text;

namespace DiezX.Api.Commons.Notifications.Utils
{

    public static class TemplateUtil
    {
        /// <summary>
        /// Obtiene el contenido HTML de un archivo y reemplaza los marcadores de posición con los valores proporcionados en un diccionario.
        /// </summary>
        /// <param name="filePath">Ruta al archivo HTML.</param>
        /// <param name="parameters">Diccionario con los valores para reemplazar los marcadores de posición.</param>
        /// <returns>El contenido HTML con los marcadores de posición reemplazados.</returns>
        public static string GetHtmlContent(string filePath, Dictionary<string, string> parameters)
        {

            var stringBuilder = new StringBuilder(filePath);

            foreach (var parameter in parameters)
            {
                var placeholder = $"{{{parameter.Key}}}";
                stringBuilder.Replace(placeholder, parameter.Value);
            }

            return stringBuilder.ToString();
        }
    }

}

