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
    /// <summary>
    /// Proporciona métodos de extensión para <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        /// Aplica la paginación a una consulta <see cref="IQueryable{T}"/>.
        /// </summary>
        /// <typeparam name="T">El tipo de los elementos de <paramref name="query"/>.</typeparam>
        /// <param name="query">La consulta a la que se aplicará la paginación.</param>
        /// <param name="pageNumber">El número de página actual. Debe ser mayor o igual a 1.</param>
        /// <param name="pageSize">El número de elementos por página. Debe ser mayor o igual a 1.</param>
        /// <returns>Una consulta <see cref="IQueryable{T}"/> que representa la página específica de elementos.</returns>
        /// <exception cref="ArgumentException">Se lanza si <paramref name="pageNumber"/> o <paramref name="pageSize"/> son menores que 1.</exception>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            if (pageNumber < 1 || pageSize < 1)
                throw new ArgumentException($"El número de página y el tamaño de página deben ser mayores o iguales a 1. Valores recibidos: pageNumber={pageNumber}, pageSize={pageSize}");

            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
