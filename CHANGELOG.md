# Changelog

Todos los cambios notables en este proyecto serán documentados en este archivo.

El formato está basado en [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
y este proyecto adhiere a [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.8.0] - 2025-10-15

### Added

- Nuevo método `TryGetResourceFromCallingAssembly` en `EmbeddedResourceUtil` para cargar recursos desde el proyecto consumidor

### Changed

- Refactorización del método `SendEmailAsync` en `DefaultMailSenderService` para cargar CSS desde recursos embebidos
- Eliminada dependencia de `IWebHostEnvironment` en `DefaultMailSenderService`

### Security

- Actualización de MailKit de `4.4.0` a `4.6.0` para corregir vulnerabilidades en BouncyCastle.Cryptography (CWE-835, CWE-203, CWE-770)

## [1.7.1] - 2025-07-22

### Fixed

- Se corrigió la plantilla de correo **FirstPassword** para que muestre correctamente las variables correctas.
- Se mejoró la estructura y claridad del mensaje en la plantilla **FirstPassword**.
- Ajuste general a las plantillas 
  
## [1.7.0] - 2025-07-12

### Added

- Configuración flexible de cookies mediante CookieConfig desde el appsettings
- AuthUtil para centralizar lógica de generación y revocación de autenticación
- Nuevo AuthControllerBase para centralizar respuesta de token en cookies en Controlladores que manejen seguridad por medio de HTTP-Only
- Agregadas propiedades al csproj `UseAppHost=false` y `GenerateAssemblyInfo=true` para optimizar como librería

### Changed

- Mejoras del DefaultExceptionHandler y logging contextual
- Corrección en la obtención de estilos CSS para plantillas de correo
- Eliminación de launchSettings.json por ser innecesario en una librería

## [1.4.0] - 2025-04-29

### Added

- Servicio para autenticación multifactor con TOTP
- Plantilla para enviar códigos MFA por correo electrónico
- Convenciones API para estandarizar respuestas y documentación

### Changed

- Actualización a .NET 8
- Optimización de servicios y estructuras internas para soportar multifactor
- Mejorada la documentación y ejemplos de uso

## [1.3.0] - 2024-04-27

### Added

- Extensiones para colecciones que permiten:
  - Lanzar excepciones personalizadas cuando una consulta está vacía o no cumple condiciones
  - Paginación de resultados con límites de tamaño y número de página
- Generación de tokens RSA

## [1.2.1] - 2024-04-02

### Added

- Funcionalidad y plantilla de correo para notificaciones de rechazo de solicitudes

### Changed

- Mejorado el validador `DateRangeValidation.cs` para asegurar que la fecha de fin sea posterior a la fecha de inicio
- Actualizado el copyright de "Soluciones Modernas 10X" a "10X de Guatemala, S.A."

### Fixed

- Correcciones de reglas de Sonar y NuGet

## [1.1.0] - 2023-12-19

### Added

- Nuevos validadores:
  - Validación avanzada de correos electrónicos
  - Validación de rangos de fechas
  - Validación de extensiones de archivos
- Paquete 'Remote' con utilidad para obtener IP de cliente
- Clase inicial de gestión de caché

### Changed

- Mejorado el manejo de errores para alinearse con el estándar ProblemDetail
- Optimizadas las clases de manejo de correo y plantillas

## [1.0.0] - 2023-12-19

### Added

- Versión inicial de la librería con funcionalidades base:
  - Manejo de excepciones
  - Servicios de seguridad
  - Servicios de notificación
  - Utilidades comunes
  
[1.8.0]: https://github.com/10xGuatemala/DiezX.Api.Commons/releases/tag/v1.8.0
[1.7.1]: https://github.com/10xGuatemala/DiezX.Api.Commons/releases/tag/v1.7.1
[1.7.0]: https://github.com/10xGuatemala/DiezX.Api.Commons/releases/tag/v1.7.0
[1.3.0]: https://github.com/10xGuatemala/DiezX.Api.Commons/releases/tag/v1.3.0
[1.2.1]: https://github.com/10xGuatemala/DiezX.Api.Commons/releases/tag/v1.2.1
[1.1.0]: https://github.com/10xGuatemala/DiezX.Api.Commons/releases/tag/v1.1.0
[1.0.0]: https://github.com/10xGuatemala/DiezX.Api.Commons/releases/tag/v1.0.0
