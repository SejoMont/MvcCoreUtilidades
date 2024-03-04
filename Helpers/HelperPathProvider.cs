using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace MvcCoreUtilidades.Helpers
{
    // Enumeración que define carpetas utilizadas por el sistema
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

        // Método privado que devuelve el nombre de la carpeta según el Folder especificado
        private string GetFolderPath(Folders folder)
        {
            string carpeta = "";
            if (folder == Folders.Images)
                carpeta = "images";
            else if (folder == Folders.Temporal)
                carpeta = "temp";
            else if (folder == Folders.Facturas)
                carpeta = "facturas";
            else if (folder == Folders.Uploads)
                carpeta = "uploads";
            else if (folder == Folders.Mails)
                carpeta = "mails";
            return carpeta;
        }

        // Método para mapear la ruta completa del archivo en el sistema de archivos del servidor
        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = this.GetFolderPath(folder);
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }

        // Método para mapear la URL completa del archivo
        public string MapUrlPath(string fileName, Folders folder)
        {
            string carpeta = this.GetFolderPath(folder);
            var addresses = server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            string urlPath = serverUrl + "/" + carpeta + "/" + fileName;
            return urlPath;
        }

    }
}