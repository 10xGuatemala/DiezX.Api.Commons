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
namespace DiezX.Api.Commons.Exceptions
{
    using System;

    /// <summary>
    /// Exception that is thrown when required data is not found.
    /// </summary>
    public class DataNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the DataNotFoundException class with a specific message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public DataNotFoundException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the DataNotFoundException class with a specific message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
        public DataNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}

