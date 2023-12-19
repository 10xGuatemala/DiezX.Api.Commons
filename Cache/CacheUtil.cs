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
using Microsoft.Extensions.Caching.Memory;

namespace DiezX.Api.Commons.Cache
{
    /// <summary>
    /// Servicio de caché estático para almacenar y recuperar datos.
    /// </summary>
    ///
    public static class CacheUtil
    {
        private static readonly Lazy<IMemoryCache> _cache = new(() => new MemoryCache(new MemoryCacheOptions()));

        /// <summary>
        /// Obtiene un elemento de la caché si existe; de lo contrario, lo genera, lo agrega a la caché y lo devuelve.
        /// </summary>
        /// <typeparam name="T">El tipo de dato a recuperar o almacenar en la caché.</typeparam>
        /// <param name="cacheKey">La clave única para almacenar/recuperar el elemento en la caché.</param>
        /// <param name="getItemCallback">Función para generar el elemento si no está en caché.</param>
        /// <param name="duration">Duración de cómo tiempo el elemento debe permanecer en la caché.</param>
        /// <returns>El elemento solicitado desde la caché o recién generado.</returns>
        public static T GetOrSet<T>(string cacheKey, Func<T> getItemCallback, TimeSpan duration)
        {
            var cache = _cache.Value;
            if (!cache.TryGetValue(cacheKey, out T cacheEntry))
            {
                cacheEntry = getItemCallback();
                cache.Set(cacheKey, cacheEntry, duration);
            }
            return cacheEntry;
        }

        /// <summary>
        /// Intenta obtener un elemento de la caché.
        /// </summary>
        /// <typeparam name="T">El tipo de dato a recuperar de la caché.</typeparam>
        /// <param name="cacheKey">La clave única para recuperar el elemento en la caché.</param>
        /// <returns>El elemento de tipo T si se encuentra en la caché, de lo contrario, el valor predeterminado para el tipo T.</returns>
        public static T Get<T>(string cacheKey)
        {
            var cache = _cache.Value;
            if (cache.TryGetValue(cacheKey, out T item))
            {
                return item;
            }
            return default;
        }

        /// <summary>
        /// Agrega un elemento a la caché o actualiza su valor si ya existe.
        /// </summary>
        /// <typeparam name="T">El tipo de dato a almacenar en la caché.</typeparam>
        /// <param name="cacheKey">La clave única para almacenar el elemento en la caché.</param>
        /// <param name="item">El elemento a almacenar en la caché.</param>
        /// <param name="duration">Duración de cómo tiempo el elemento debe permanecer en la caché.</param>
        public static void Set<T>(string cacheKey, T item, TimeSpan duration)
        {
            var cache = _cache.Value;
            cache.Set(cacheKey, item, duration);
        }

    }


}

