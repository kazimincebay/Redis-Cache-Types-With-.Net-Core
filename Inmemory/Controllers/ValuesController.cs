using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Cryptography.X509Certificates;

namespace Inmemory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        readonly IMemoryCache _memoryCache;

        public ValuesController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet("SetName/{name}")]
        public void SetName(string name)
        {
            _memoryCache.Set("name", name);
            _memoryCache.Set("name", name);
        }

        [HttpGet("GetName")]
        public string GetName()
        {
            /* cachlenen veri üzerinde işlem yapılıyorsa ve bu veri geri dönülüyorsa cachlenmediği durumlarda runtime hatalarına neden olabilir.
             bu nedenle TryGetValue() fonksiyonu kullanılması runtime hatalarının önüne geçilmesini sağlar */
            //return _memoryCache.Get<string>("name");
            if (_memoryCache.TryGetValue<string>("name",out string name))
            {
                return _memoryCache.Get<string>("name");
            }
            

            return "Veri Cachlenmemiş";
        }

        [HttpGet("DeleteName")]
        public string DeleteName()
        {
            _memoryCache.Remove("name");
            return "Cachlenen veri başarılı bir şekilde silinmiştir.";
        }


        [HttpGet("SetDate")]
        public void SetDate()
        {
            _memoryCache.Set<DateTime>("date", DateTime.Now, options: new()
            {
                AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                SlidingExpiration=TimeSpan.FromSeconds(5)
            });
            
        }

        [HttpGet("GetDate")]
        public DateTime GetDate()
        {
            return _memoryCache.Get<DateTime>("date");
        }

    }
}
