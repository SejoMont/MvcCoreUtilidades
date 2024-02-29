using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace MvcCoreUtilidades.Helpers
{
    // Enumeración que representa las carpetas utilizadas por los controllers
    public enum Folders { Images = 0, Facturas = 1, Uploads = 2, Temporal = 3, Mails = 4 }

    // Clase que proporciona métodos para mapear rutas y URLs de archivos
    public class HelperPathProvider
    {
        private readonly IServer server;
        private readonly IWebHostEnvironment hostEnvironment;

        // Constructor que recibe el entorno de host web y el servidor
        public HelperPathProvider(IWebHostEnvironment hostEnvironment, IServer server)
        {
            this.server = server;
            this.hostEnvironment = hostEnvironment;
        }

        //Metodo que nos devuelve el nombre de la carpeta del folder
        private string GetFolderPath()
        {

        }


        // Método para mapear la ruta completa del archivo en el sistema de archivos del servidor
        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = GetFolderName(folder);
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }

        // Método para mapear la URL completa del archivo
        public string MapUrlPath(string fileName, Folders folder)
        {
            string carpeta = GetFolderName(folder);
            var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            string urlPath = $"{serverUrl}/{carpeta}/{fileName}";
            return urlPath;
        }

        // Método privado para obtener el nombre de la carpeta según la enumeración Folders
        private string GetFolderName(Folders folder)
        {
            return folder switch
            {
                Folders.Images => "images",
                Folders.Temporal => "temp",
                Folders.Facturas => "facturas",
                Folders.Uploads => "uploads",
                _ => throw new ArgumentOutOfRangeException(nameof(folder)),
            };
        }
    }
}