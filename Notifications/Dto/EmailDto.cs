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
namespace DiezX.Api.Commons.Notifications.Dto
{
    /// <summary>
    /// DTO para la configuración de correo electrónico
    /// </summary>
    public class EmailDto
    {
        /// <summary>
        /// Nombre del destinatario.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Correo electrónico del destinatario.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Asunto del correo electrónico.
        /// </summary>
        public required string Subject { get; set; }

        /// <summary>
        /// Cuerpo del correo electrónico.
        /// </summary>
        public required string Body { get; set; }
    }

}

