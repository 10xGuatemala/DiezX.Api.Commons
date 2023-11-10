//
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
using System.Diagnostics;
using HeyRed.Mime;

namespace DiezX.Api.Commons.Utils {
	/// <summary>
	/// Utilidad para el manejo de operaciones de archivos en el sistema de archivos.
	/// Este archivo necesita las propiedades StaticFiles del appsettings.json
	/// </summary>
	public static class StaticFileUtil {

		public static string DirectoryPath { get; private set; } = "";
		public static string RequestPath { get; private set; } = "";

		/// <summary>
		/// Inicializa la utilidad del sistema de archivos con el directorio de destino configurado.
		/// </summary>
		/// <param name="directoryPath">La ruta del directorio de destino.</param>
		/// <param name="requestPath">La ruta para servir las imagenes</param>
		public static void Initialize (string directoryPath,
			string requestPath)
		{
			DirectoryPath = directoryPath ?? throw new ArgumentNullException (nameof (directoryPath));
			RequestPath = requestPath ?? throw new ArgumentNullException (nameof (requestPath));
		}

		/// <summary>
		/// Guarda un archivo en el sistema de archivos en la ruta especificada en la configuración.
		/// </summary>
		/// <param name="file">El archivo a guardar.</param>
		/// <returns>Una tarea que representa la operación asincrónica.</returns>
		public static async Task<string> SaveFileToDirectory (IFormFile file)
		{
			if (string.IsNullOrEmpty (DirectoryPath)) {
				throw new InvalidOperationException ("El directorio de destino no ha sido configurado.");
			}

			string fileName = Guid.NewGuid ().ToString () + Path.GetExtension (file.FileName);
			string filePath = Path.Combine (DirectoryPath, fileName);

			using (FileStream fileStream = new (filePath, FileMode.Create)) {
				await file.CopyToAsync (fileStream);
			}

			Trace.WriteLine ($"Archivo guardado en el sistema de archivos: {fileName}");

			return fileName;
		}

		/// <summary>
		/// Lee un archivo del sistema de archivos en la ruta especificada en la configuración y lo devuelve como un arreglo de bytes.
		/// </summary>
		/// <param name="fileName">El nombre del archivo a leer.</param>
		/// <returns>Los bytes del archivo leído.</returns>
		public static byte [] ReadFileToByte (string fileName)
		{
			if (string.IsNullOrEmpty (DirectoryPath)) {
				throw new InvalidOperationException ("El directorio de destino no ha sido configurado.");
			}

			string filePath = Path.Combine (DirectoryPath, fileName);
			byte [] fileBytes = File.ReadAllBytes (filePath);

			Trace.WriteLine ($"Archivo leído del sistema de archivos: {fileName}");

			return fileBytes;
		}


		/// <summary>
		/// Devuelve la ruta relativa hacia el archivo estatico 
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>la ruta relativa hacia el archivo estatico</returns>
		public static string GetStaticFilePath (string fileName)
		{
			return $"{RequestPath}/{fileName}";
		}

		/// <summary>
		/// Devuelve el content-type basado en la extensión del archivo (filename)
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns>Content-Type</returns>
		public static string GetContentType (string fileName)
		{
			string extension = Path.GetExtension (fileName);
			string contentType = MimeTypesMap.GetMimeType (extension);
			Trace.WriteLine ($"Content-type: {contentType}");

			return contentType;
		}
	}

}

