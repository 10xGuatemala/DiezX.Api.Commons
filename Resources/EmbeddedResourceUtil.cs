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
using System.Reflection;

namespace DiezX.Api.Commons.Resources
{

    /// <summary>
    /// Clase de utilidad para leer y administrar recursos incrustados (Embedded Resources).
    /// Para que un archivo pueda ser encontrado debe estar marcada con Acción de compilación Embedded Resource
    /// o agregarla manualmente en el .csproj en ItemGroup por ejemplo:
    /// <ItemGroup>
    /// ...
    /// <EmbeddedResource Include = "Templates\FirstPasswordTemplate.html" />
    ///</ ItemGroup >
    /// </summary>
    public static class EmbeddedResourceUtil
    {
        // Diccionario para almacenar el contenido de los recursos.
        private static readonly Dictionary<string, string> ResourceCache = new();

        /// <summary>
        /// Obtiene el contenido del recurso incrustado a partir de una parte de su nombre.
        /// </summary>
        /// <param name="partialName">Parte del nombre del recurso que se desea obtener.</param>
        /// <returns>Contenido del recurso incrustado.</returns>
        /// <exception cref="ArgumentException">Se lanza si el recurso no se encuentra.</exception>
        public static string GetResource(string partialName)
        {
            // Intentar obtener el recurso del cache.
            if (ResourceCache.TryGetValue(partialName, out string cachedContent))
            {
                return cachedContent;
            }

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames().First(r => r.EndsWith(partialName));
            if (resourceName != null)
            {
                var content = GetEmbeddedResourceContent(resourceName);
                // Guardar el contenido en el cache.
                ResourceCache[partialName] = content;
                return content;
            }

            throw new ArgumentException($"El recurso '{partialName}' no se encuentra en el ensamblado.", nameof(partialName));
        }


        /// <summary>
        /// Obtiene el contenido de un recurso incrustado específico.
        /// </summary>
        /// <param name="resourceName">Nombre completo del recurso incrustado.</param>
        /// <returns>Contenido del recurso incrustado.</returns>
        private static string GetEmbeddedResourceContent(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            try
            {
                using Stream stream = assembly.GetManifestResourceStream(resourceName);
                using StreamReader reader = new(stream);
                return reader.ReadToEnd();
            }
            catch (Exception)
            {
                throw new ArgumentException($"El recurso '{resourceName}' no se pudo leer del ensamblado.", nameof(resourceName));
            }
        }
    }
}


