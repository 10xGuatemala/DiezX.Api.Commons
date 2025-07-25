﻿//
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

namespace DiezX.Api.Commons.Exceptions
{

    /// <summary>
    /// Excepción que incluye un código de estado (StatusCode) para operaciones en API REST.
    /// </summary>
    public class ApiGeneralException : Exception
    {
        /// <summary>
        /// Código de estado HTTP.
        /// </summary>
        public int StatusCode { get; private set; }

        /// <summary>
        /// Título o encabezado de la excepción.
        /// </summary>
        public string? Title { get; private set; }

        /// <summary>
        /// Constructor para excepción con mensaje personalizado y código de estado.
        /// </summary>
        /// <param name="statusCode">Código de estado HTTP.</param>
        /// <param name="message">Mensaje de la excepción.</param>
        public ApiGeneralException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }

        /// <summary>
        /// Constructor para excepción con código de estado, título y mensaje.
        /// </summary>
        /// <param name="statusCode">Código de estado HTTP.</param>
        /// <param name="title">Título o encabezado de la excepción.</param>
        /// <param name="message">Mensaje de la excepción.</param>
        public ApiGeneralException(int statusCode, string title, string message) : base(message)
        {
            StatusCode = statusCode;
            Title = title;
        }

        /// <summary>
        /// Constructor predeterminado protegido.
        /// </summary>
        protected ApiGeneralException()
        { }

    }
}