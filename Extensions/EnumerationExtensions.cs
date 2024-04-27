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
    /// Provides extension methods for <see cref="IEnumerable{T}"/> that enable additional
    /// functionality like throwing exceptions if certain conditions are not met.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Returns the first element of a sequence, or throws DataNotFoundException if no element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to return the first element of.</param>
        /// <param name="exceptionMessage">The message of the exception if no element is found.</param>
        /// <returns>The first element in the sequence.</returns>
        /// <exception cref="DataNotFoundException">Thrown if the sequence is empty.</exception>
        public static T FirstOrThrow<T>(this IEnumerable<T> source, string exceptionMessage)
        {
            T result = source.FirstOrDefault();
            return result == null ? throw new DataNotFoundException(exceptionMessage) : result;
        }


        /// <summary>
        /// Asynchronously returns the first element of a sequence, or throws DataNotFoundException if no element is found.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IQueryable{T}"/> to return the first element of.</param>
        /// <param name="exceptionMessage">The message of the exception if no element is found.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first element in the sequence.</returns>
        /// <exception cref="DataNotFoundException">Thrown if the sequence is empty.</exception>
        public static async Task<T> FirstOrThrowAsync<T>(this IQueryable<T> source, string exceptionMessage)
        {
            var result = await source.FirstOrDefaultAsync();
            return result == null ? throw new DataNotFoundException(exceptionMessage) : result;
        }

        /// <summary>
        /// Converts the sequence to a list and throws an exception if the list is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IEnumerable{T}"/> to convert to a list.</param>
        /// <param name="exceptionMessage">The message of the exception if the list is empty.</param>
        /// <returns>The converted list of <see cref="IEnumerable{T}"/> if it is not empty.</returns>
        /// <exception cref="DataNotFoundException">Thrown if the list is empty.</exception>
        public static List<T> ToListOrThrow<T>(this IEnumerable<T> source, string exceptionMessage = "The list is empty.")
        {
            List<T> list = source.ToList();
            return !list.Any() ? throw new DataNotFoundException(exceptionMessage) : list;
        }


        /// <summary>
        /// Converts the sequence to a list asynchronously and throws an exception if the list is empty.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">The <see cref="IQueryable{T}"/> to convert to a list.</param>
        /// <param name="exceptionMessage">The message of the exception if the list is empty.</param>
        /// <returns>The converted list of <see cref="IEnumerable{T}"/> if it is not empty.</returns>
        /// <exception cref="DataNotFoundException">Thrown if the list is empty.</exception>
        public static async Task<List<T>> ToListOrThrowAsync<T>(this IQueryable<T> source, string exceptionMessage = "The list is empty.")
        {
            List<T> list = await source.ToListAsync();
            return !list.Any() ? throw new DataNotFoundException(exceptionMessage) : list;
        }
    }

}

