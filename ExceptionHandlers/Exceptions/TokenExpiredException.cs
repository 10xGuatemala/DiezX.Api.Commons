﻿//
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
namespace DiezX.Api.Commons.ExceptionHandlers.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class TokenExpiredException : Exception
    {
        // Constructor predeterminado
        public TokenExpiredException()
        {
        }

        // Constructor que acepta el mensaje de error
        public TokenExpiredException(string message)
            : base(message)
        {
        }

        // Constructor que acepta el mensaje de error y una excepción interna
        public TokenExpiredException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // Constructor de deserialización
        protected TokenExpiredException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        //Necesario si necesitas serializar datos adicionales
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            // Agrega aquí la serialización de cualquier dato adicional si es necesario
        }
    }

}

