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
using HeyRed.Mime;

namespace DiezX.Api.Commons.Utils
{
    /// <summary>
    /// Clase de utilidades para manipular flujos de datos.
    /// </summary>
    public static class StreamUtils
    {
        /// <summary>
        /// Convierte un flujo de datos en un arreglo de bytes de forma asíncrona.
        /// </summary>
        /// <param name="stream">El flujo de datos de entrada.</param>
        /// <returns>Un arreglo de bytes que representa los datos del flujo.</returns>
        public static async Task<byte[]> StreamToByteAsync(Stream stream)
        {
            using MemoryStream memoryStream = new();
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Convierte un archivo en un arreglo de bytes de forma asíncrona.
        /// </summary>
        /// <param name="file">El archivo de entrada.</param>
        /// <returns>Un arreglo de bytes que representa los datos del archivo.</returns>
        public static async Task<byte[]> FormFileToByteAsync(IFormFile file)
        {
            using MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Convierte un arreglo de bytes en un flujo de datos.
        /// </summary>
        /// <param name="data">El arreglo de bytes de entrada.</param>
        /// <returns>Un flujo de datos que contiene los bytes.</returns>
        public static Stream ByteToStream(byte[] data)
        {
            return new MemoryStream(data);
        }

        /// <summary>
        /// Convierte un array de bytes en un objeto IFormFile.
        /// </summary>
        /// <param name="bytes">Array de bytes a convertir.</param>
        /// <param name="fileName">Nombre del archivo.</param>
        /// <param name="contentType">Tipo de contenido (content type) del archivo.</param>
        /// <returns>Objeto IFormFile creado a partir del array de bytes.</returns>
        public static IFormFile ByteArrayToFormFile(byte[] bytes, string fileName, string contentType)
        {
            var stream = new MemoryStream(bytes);
            return new FormFile(stream, 0, bytes.Length, fileName, fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }

        /// <summary>
        /// Obtiene la extensión del archivo basado en el tipo de contenido
        /// </summary>
        /// <param name="contentType">Tipo de contenido MIME del archivo</param>
        /// <returns>La extensión del archivo incluyendo el punto (.)</returns>
        public static string GetExtension(string contentType)
        {
            return MimeTypesMap.GetExtension(contentType);
        }
    }



}

