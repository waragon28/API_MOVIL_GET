using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebApiNetCore.DAL;

namespace WebApiNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompraController : ControllerBase
    {
        private IMemoryCache _memoryCache;
        public CompraController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("Photo")]
        public RedirectResult GetPhoto(string Name)
        {
            CompraDAL compraDAL = new(_memoryCache);
            return Redirect(compraDAL.getUrlImagen(Name));
        }

        
    }
}
