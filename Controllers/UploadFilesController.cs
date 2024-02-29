using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using MvcCoreUtilidades.Helpers;

public class UploadFilesController : Controller
{
    private HelperPathProvider helperPathProvider;

    public UploadFilesController(HelperPathProvider helperPathProvider)
    {
        this.helperPathProvider = helperPathProvider;
    }

    // Vista para subir un archivo
    public IActionResult SubirFichero()
    {
        return View();
    }

    // Método para manejar la subida de archivos mediante POST
    [HttpPost]
    public async Task<IActionResult> SubirFichero(IFormFile fichero)
    {
        // Obtener la ruta completa del archivo utilizando HelperPathProvider
        string path = this.helperPathProvider.MapPath(fichero.FileName, Folders.Uploads);

        // Subir el archivo utilizando Stream
        using (Stream stream = new FileStream(path, FileMode.Create))
        {
            // Copiar el contenido del archivo al stream
            await fichero.CopyToAsync(stream);
        }

        // Mensajes de éxito en la vista
        ViewData["MENSAJE"] = "Fichero subido a " + path;

        // Obtener y mostrar la URL base del servidor
        string urlServer = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
        ViewData["TEST"] = urlServer;

        // Obtener y mostrar la URL completa del archivo
        string urlPath = this.helperPathProvider.MapUrlPath(fichero.FileName, Folders.Uploads);
        ViewData["URL"] = urlPath;

        return View();
    }

}
