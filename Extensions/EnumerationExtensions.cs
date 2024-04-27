//
//  Copyright 2024  Copyright © 10X de Guatemala, S.A.
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
namespace DiezX.Api.Commons.Extensions
{
    using System.Linq;
    using System.Collections.Generic;
    using DiezX.Api.Commons.Exceptions;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// Proporciona métodos de extensión para <see cref="IEnumerable{T}"/> que habilitan funcionalidades adicionales
    /// como lanzar excepciones si ciertas condiciones no se cumplen.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Devuelve el primer elemento de una secuencia, o lanza una DataNotFoundException si no se encuentra ningún elemento.
        /// </summary>
        /// <typeparam name="T">El tipo de los elementos de <paramref name="source"/>.</typeparam>
        /// <param name="source">El <see cref="IEnumerable{T}"/> del cual se devuelve el primer elemento.</param>
        /// <param name="exceptionMessage">El mensaje de la excepción si no se encuentra ningún elemento.</param>
        /// <returns>El primer elemento en la secuencia.</returns>
        /// <exception cref="DataNotFoundException">Lanzada si la secuencia está vacía.</exception>
        public static T FirstOrThrow<T>(this IEnumerable<T> source, string exceptionMessage)
        {
            T result = source.FirstOrDefault();
            return EqualityComparer<T>.Default.Equals(result, default) ? throw new DataNotFoundException(exceptionMessage) : result;
        }

        /// <summary>
        /// Devuelve de forma asincrónica el primer elemento de una secuencia, o lanza una DataNotFoundException si no se encuentra ningún elemento.
        /// </summary>
        /// <typeparam name="T">El tipo de los elementos de <paramref name="source"/>.</typeparam>
        /// <param name="source">El <see cref="IQueryable{T}"/> del cual se devuelve el primer elemento.</param>
        /// <param name="exceptionMessage">El mensaje de la excepción si no se encuentra ningún elemento.</param>
        /// <returns>Una tarea que representa la operación asincrónica. El resultado de la tarea contiene el primer elemento en la secuencia.</returns>
        /// <exception cref="DataNotFoundException">Lanzada si la secuencia está vacía.</exception>
        public static async Task<T> FirstOrThrowAsync<T>(this IQueryable<T> source, string exceptionMessage)
        {
            var result = await source.FirstOrDefaultAsync();
            return EqualityComparer<T>.Default.Equals(result, default) ? throw new DataNotFoundException(exceptionMessage) : result;
        }

        /// <summary>
        /// Convierte la secuencia en una lista y lanza una excepción si la lista está vacía.
        /// </summary>
        /// <typeparam name="T">El tipo de los elementos de <paramref name="source"/>.</typeparam>
        /// <param name="source">El <see cref="IEnumerable{T}"/> a convertir en lista.</param>
        /// <param name="exceptionMessage">El mensaje de la excepción si la lista está vacía.</param>
        /// <returns>La lista convertida de <see cref="IEnumerable{T}"/> si no está vacía.</returns>
        /// <exception cref="DataNotFoundException">Lanzada si la lista está vacía.</exception>
        public static List<T> ToListOrThrow<T>(this IEnumerable<T> source, string exceptionMessage = "La lista está vacía.")
        {
            List<T> list = source.ToList();
            return !list.Any() ? throw new DataNotFoundException(exceptionMessage) : list;
        }

        /// <summary>
        /// Convierte la secuencia en una lista de forma asincrónica y lanza una excepción si la lista está vacía.
        /// </summary>
        /// <typeparam name="T">El tipo de los elementos de <paramref name="source"/>.</typeparam>
        /// <param name="source">El <see cref="IQueryable{T}"/> a convertir en lista.</param>
        /// <param name="exceptionMessage">El mensaje de la excepción si la lista está vacía.</param>
        /// <returns>La lista convertida de <see cref="IEnumerable{T}"/> si no está vacía.</returns>
        /// <exception cref="DataNotFoundException">Lanzada si la lista está vacía.</exception>
        public static async Task<List<T>> ToListOrThrowAsync<T>(this IQueryable<T> source, string exceptionMessage = "La lista está vacía.")
        {
            List<T> list = await source.ToListAsync();
            return !list.Any() ? throw new DataNotFoundException(exceptionMessage) : list;
        }
    }

}

