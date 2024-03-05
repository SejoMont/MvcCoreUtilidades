using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace MvcCoreUtilidades.Controllers
{
    public class CachingController : Controller
    {
        private IMemoryCache memoryCache;

        public CachingController(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public IActionResult MemoriaDistribuida()
        {
            string fecha = DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString();
            ViewData["FECHA"] = fecha;
            return View();
        }

        public IActionResult MemoriaPersonalizada(int? tiempo)
        {
            if (tiempo == null)
            {
                tiempo = 15;
            }
            string fecha = DateTime.Now.ToLongDateString() + " -- " + DateTime.Now.ToLongTimeString();

            // Vemos si existe algo en cache
            if (this.memoryCache.Get("FECHA") == null)
            {
                // No existe
                MemoryCacheEntryOptions options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(tiempo.Value));
                this.memoryCache.Set("FECHA", fecha, options);
                ViewData["MWNSAJE"] = "Almacenado en cache";
                ViewData["FECHA"] = this.memoryCache.Get("FECHA");
            }
            else
            {
                // Existe fecha en cache
                fecha = this.memoryCache.Get<string>("FECHA");
                ViewData["MWNSAJE"] = "Recuperando de cache";
                ViewData["FECHA"] = this.memoryCache.Get("FECHA");
            }
            return View();
        }
    }
}
