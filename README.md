# DiezX.Api.Commons
Este módulo central está equipado con clases esenciales diseñadas para apoyar el desarrollo de APIs RESTful utilizando el marco de desarrollo de DiezX. Proporciona herramientas y servicios comunes que se pueden utilizar a lo largo del proyecto.

# Dependencias del Proyecto

Este proyecto requiere las siguientes dependencias:

- NodaTime, versión `3.1.11`
- MailKit, versión `4.4.1`
- System.IdentityModel.Tokens.Jwt, versión `7.4.0`
- MimeTypesMap, versión `1.0.8`
- SonarAnalyzer.CSharp, versión `9.21.0.87781`

## Estructura del Proyecto
El proyecto está organizado en varias carpetas y archivos que se detallan a continuación:

- **Collections**
  - `CollectionUtil.cs`: Utilidades para trabajar con colecciones de datos.

- **Converters**
  - `JsonDateTimeConverter.cs`: Convertidor para manejar la serialización de fechas en JSON.

- **Date**
  - `DateUtil.cs`: Utilidades para operaciones comunes relacionadas con fechas.

- **ExceptionsHandlers**
    - **Dtos**
        - `ExtendedProblemDetail.cs`: Clase para proporcionar detalles adicionales en las respuestas de errores de la API, extendiendo la estructura de respuesta estándar.

    - **Exceptions**
        - `ApiGeneralException.cs`: Excepción personalizada para errores generales de la API.
        - `ApiValidationParamsException.cs`: Excepción para manejar errores específicos de validación de parámetros de la API.
        - `TokenExpiredException.cs`: Excepción personalizada que se lanza cuando un token de autenticación ha expirado.

    - **Filters**
        - `ValidateModelAttribute.cs`: Filtro de acción para validar el modelo de entrada en las solicitudes a la API antes de llegar al controlador.
        - `DefaultExceptionHandler.cs`: Manejador por defecto para capturar y procesar excepciones no controladas a nivel de la aplicación.

- **Notifications**
  - **Configurations**: Configuraciones relacionadas con notificaciones.
  - **Dto**: Objetos de Transferencia de Datos para notificaciones.
  - **Services**
    - `DefaultMailSenderService.cs`: Servicio predeterminado para enviar correos electrónicos.
    - `SendMailService.cs`: Servicio para enviar correos electrónicos.
  - **Templates**
    - `EmailConfirmationTemplate.html`: Plantilla para correos de confirmación.
    - `FirstPasswordTemplate.html`: Plantilla para correos de primera contraseña.
    - `PasswordRecoveryTemplate.html`: Plantilla para correos recuperación de contraseña.
    - `PasswordUpdatedTemplate.html`: Plantilla para correos de actualización de contraseña.
    - `RejectionTemplate.html`: Plantilla para correos donde se rechaza solicitud.
- **Remote**
    - `RemoteUtils.cs`: Clase de utilidades para manejar aspectos de las solicitudes remotas. Implementa un metodo para obtener la IP del usuario.

- **Utils**
  - `TemplateUtil.cs`: Utilidades para trabajar con plantillas de correo electrónico.

- **Properties**
  - `launchSettings.json`: Configuraciones de lanzamiento para el entorno de desarrollo.

- **Resources**
  - `EmbeddedResourceUtil.cs`: Utilidades para trabajar con recursos incrustados.
  - `StaticFileUtil.cs`: Utilidades para trabajar con archivos estáticos.
  - `StreamUtils.cs`: Utilidades para trabajar con flujos de datos.

- **Security**
  - **Configurations**: Configuraciones relacionadas con la seguridad.
  - **Dto**: Objetos de Transferencia de Datos para la seguridad.
  - **Services**
    - `TokenService.cs`: Servicio para la creación y manejo de tokens.
    - `UserRequestService.cs`: Servicio para manejar solicitudes de usuario.

- **Strings**
  - `StringUtil.cs`: Utilidades para operaciones comunes con cadenas de texto.

- **Validators**
  - `AdvancedEmailAttribute.cs`: Validador de atributo para la estructura y formato de direcciones de correo electrónico avanzadas (requiere que el correo tengo un dominio de primer nivel)
  - `DateRangeValidation.cs`: Validador de atributo para asegurar que una fecha se encuentre dentro de un rango específico.
  - `FileExtensionAttribute.cs`: Validador de atributo para verificar que un archivo tenga una de las extensiones de archivo permitidas.
  - `FileSizeAttribute.cs`: Validador de atributo para el tamaño de un archivo, asegurando que no exceda un tamaño máximo especificado.
  - `PasswordValidationAttribute.cs`: Validador de atributo para contraseñas, imponiendo reglas para una complejidad mínima requerida.

## Uso

Este proyecto está diseñado para ser utilizado como una biblioteca de clases dentro de un proyecto más grande de ASP.NET Core. Se debe hacer referencia a este proyecto desde su solución principal para acceder a las funcionalidades comunes que proporciona.

## Contribuciones

Las contribuciones son bienvenidas siguiendo los estándares y lineamientos de DiezX.

---

DiezX.Api.Commons es un esfuerzo para proporcionar un punto de partida robusto y eficiente para el desarrollo de APIs con el marco de desarrollo DiezX.
