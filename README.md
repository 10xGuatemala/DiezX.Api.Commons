# DiezX.Api.Commons
Este módulo central está equipado con clases esenciales diseñadas para apoyar el desarrollo de APIs RESTful utilizando el marco de desarrollo de DiezX. Proporciona herramientas y servicios comunes que se pueden utilizar a lo largo del proyecto.

## Estructura del Proyecto
El proyecto está organizado en varias carpetas y archivos que se detallan a continuación:

- **Collections**
  - `CollectionUtil.cs`: Utilidades para trabajar con colecciones de datos.

- **Converters**
  - `JsonDateTimeConverter.cs`: Convertidor para manejar la serialización de fechas en JSON.

- **Date**
  - `DateUtil.cs`: Utilidades para operaciones comunes relacionadas con fechas.

- **Exceptions**
  - `ApiException.cs`: Clase personalizada para manejar excepciones de la API.
  - `CustomProblemDetails.cs`: Detalles personalizados para problemas de la API.
  - `ExceptionHandler.cs`: Manejador global de excepciones para la API.

- **Notifications**
  - **Configurations**: Configuraciones relacionadas con notificaciones.
  - **Dto**: Objetos de Transferencia de Datos para notificaciones.
  - **Services**
    - `DefaultMailSenderService.cs`: Servicio predeterminado para enviar correos electrónicos.
    - `SendMailService.cs`: Servicio para enviar correos electrónicos.
  - **Templates**
    - `EmailConfirmationTemplate.html`: Plantilla para correos de confirmación.
    - `FirstPasswordTemplate.html`: Plantilla para correos de primera contraseña.
    - `PasswordUpdatedTemplate.html`: Plantilla para correos de actualización de contraseña.

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
  - `FileSizeAttribute.cs`: Validador de atributo para el tamaño de un archivo.
  - `PasswordValidationAttribute.cs`: Validador de atributo para contraseñas.

## Uso

Este proyecto está diseñado para ser utilizado como una biblioteca de clases dentro de un proyecto más grande de ASP.NET Core. Se debe hacer referencia a este proyecto desde su solución principal para acceder a las funcionalidades comunes que proporciona.

## Contribuciones

Las contribuciones son bienvenidas siguiendo los estándares y lineamientos de DiezX.

---

DiezX.Api.Commons es un esfuerzo para proporcionar un punto de partida robusto y eficiente para el desarrollo de APIs con el marco de desarrollo DiezX.
