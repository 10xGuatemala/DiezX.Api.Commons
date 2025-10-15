# DiezX.Api.Commons

Este paquete central está equipado con clases esenciales diseñadas para apoyar el desarrollo de APIs RESTful utilizando el marco de desarrollo de DiezX. Proporciona herramientas y servicios comunes que se pueden utilizar a lo largo del proyecto.

## Instalación

### Instalación desde NuGet

La forma más sencilla de instalar la librería es a través del administrador de paquetes NuGet:

1. **Usando la CLI de .NET**:

   ```bash
   dotnet add package DiezX.Api.Commons
   ```

2. **Usando el Package Manager Console en Visual Studio**:

   ```powershell
   Install-Package DiezX.Api.Commons
   ```

3. **Usando el NuGet Package Manager en Visual Studio**:
   - Click derecho en el proyecto
   - Seleccionar "Manage NuGet Packages..."
   - Buscar "DiezX.Api.Commons"
   - Click en "Install"

## Dependencias del Proyecto

Este proyecto requiere las siguientes dependencias:

- NodaTime, versión `3.1.11`
- MailKit, versión `4.6.0` (actualizado desde `4.4.0` para corregir vulnerabilidades de seguridad)
- System.IdentityModel.Tokens.Jwt, versión `7.4.1`
- MimeTypesMap, versión `1.0.8`
- SonarAnalyzer.CSharp, versión `9.22.0.87781`
- Otp.NET versión `1.4.0`
- PreMailer.Net versión `2.6.0`
- Microsoft.EntityFrameworkCore `7.0.18` - Requerido para los métodos de extensión asincrónicos. Compatible con proyectos que usen EF Core 8.x.

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
    - `DefaultMailSenderService.cs`: Servicio predeterminado para enviar correos electrónicos. Soporta CSS personalizado mediante recursos embebidos.
    - `SendMailService.cs`: Servicio para enviar correos electrónicos.
  - **Templates**
    - `EmailConfirmationTemplate.html`: Plantilla para correos de confirmación.
    - `FirstPasswordTemplate.html`: Plantilla para correos de primera contraseña.
    - `PasswordRecoveryTemplate.html`: Plantilla para correos recuperación de contraseña.
    - `PasswordUpdatedTemplate.html`: Plantilla para correos de actualización de contraseña.
    - `RejectionTemplate.html`: Plantilla para correos donde se rechaza solicitud.
    - `MfaCodeEmailTemplate`: Plantilla para enviar por correo código de MFA
    - `email-default-styles.css`: Estilos CSS por defecto para las plantillas de correo
  - **Utils**
    - `TemplateUtil.cs`: Utilidades para trabajar con plantillas de correo electrónico.
  - `README-CSS-PERSONALIZADO.md`: Guía completa para personalizar los estilos CSS de los correos electrónicos
- **Remote**
  - `RemoteUtils.cs`: Clase de utilidades para manejar aspectos de las solicitudes remotas. Implementa un metodo para obtener la IP del usuario.
  
- **Resources**
  - `EmbeddedResourceUtil.cs`: Utilidades para trabajar con recursos incrustados.
  - `StaticFileUtil.cs`: Utilidades para trabajar con archivos estáticos.
  - `StreamUtils.cs`: Utilidades para trabajar con flujos de datos.

- **Security**
  - **Configurations**
    - `CookieConfig.cs`: Configuración para cookies de autenticación, define propiedades como nombres de cookies, tiempo de vida, y opciones de seguridad (HttpOnly, Secure, SameSite).
    - `MfaConfig.cs`: Configuración para autenticación de doble factor, incluye parámetros como longitud del código, tiempo de expiración y opciones del algoritmo TOTP.
    - `TokenConfig.cs`: Configuración para tokens JWT, define propiedades como secreto, tiempo de vida, y opciones de firma.

  - **Controllers**
    - `AuthControllerBase.cs`: Controlador base que proporciona funcionalidad común para respuestas de autenticación, incluyendo manejo de cookies seguras y respuestas adaptadas según el entorno.

  - **Dto**: Objetos de Transferencia de Datos para la seguridad.
  - **Services**
    - `TokenService.cs`: Servicio para la creación y manejo de tokens.
    - `UserRequestService.cs`: Servicio para manejar solicitudes de usuario.
    - `MfaService`: Servicio para MFA con TOTP, permitiendo generar y validar códigos basados en una clave secreta.
  - **Utils**
    - `HeaderUtil.cs`: Utileria para Decodificar la cabecera de autorización tipo BASIC y extraer las credenciales del usuario.
    - `RefreshTokenUtil`: Utileria para la generación de Refresh Tokens seguros.
    - `AuthUtil.cs`: Utilidad para gestión de autenticación que maneja cookies seguras para tokens JWT y refresh tokens, implementa configuraciones de seguridad (HttpOnly, Secure, SameSite) y adapta respuestas según el entorno (desarrollo/producción).

- **Strings**
  - `StringUtil.cs`: Utilidades para operaciones comunes con cadenas de texto.

- **Validators**
  - `AdvancedEmailAttribute.cs`: Validador de atributo para la estructura y formato de direcciones de correo electrónico avanzadas (requiere que el correo tenga un dominio de primer nivel)
  - `DateRangeValidation.cs`: Validador de atributo para asegurar que una fecha se encuentre dentro de un rango específico.
  - `FileExtensionAttribute.cs`: Validador de atributo para verificar que un archivo tenga una de las extensiones de archivo permitidas.
  - `FileSizeAttribute.cs`: Validador de atributo para el tamaño de un archivo, asegurando que no exceda un tamaño máximo especificado.
  - `PasswordValidationAttribute.cs`: Validador de atributo para contraseñas, imponiendo reglas para una complejidad mínima requerida.

- **Extensiones**
  - `EnumerableExtensions.cs`: Provee métodos de extensión para `IEnumerable<T>` que permiten funcionalidades tales como lanzar excepciones personalizadas si el resultado de una consulta está vacía o una condición no se cumple.
  - `QueryableExtensions.cs`:  Añade métodos de extensión para `IQueryable<T>` que ayudan con la paginación de resultados aplicando límites de tamaño de página y número de página a las consultas.

- **Conventions**
  - `ApiConventions`:  Convención que incluye las posibles respuestas estándar del api para robustecer la documentación de swagger

## Uso

Este proyecto está diseñado para ser utilizado como una biblioteca de clases dentro de un proyecto más grande de ASP.NET Core. Se debe hacer referencia a este proyecto desde su solución principal para acceder a las funcionalidades comunes que proporciona.

### Personalización de CSS para correos electrónicos

A partir de la versión 1.8.0, puedes personalizar los estilos CSS de los correos electrónicos incluyendo tu propio archivo `email-styles.css` como recurso embebido en tu proyecto:

1. Crea un archivo `email-styles.css` en tu proyecto
2. Márcalo como **Embedded Resource** en el `.csproj`:

   ```xml
   <ItemGroup>
     <EmbeddedResource Include="Resources\email-styles.css" />
   </ItemGroup>
   ```

3. El servicio `DefaultMailSenderService` automáticamente detectará y usará tu CSS personalizado

Para más detalles, consulta la [guía completa de personalización de CSS](Notifications/README-CSS-PERSONALIZADO.md).

## Contribuciones

Las contribuciones son bienvenidas siguiendo los estándares y lineamientos de DiezX.

---

DiezX.Api.Commons es un esfuerzo para proporcionar un punto de partida robusto y eficiente para el desarrollo de APIs con el marco de desarrollo DiezX.
