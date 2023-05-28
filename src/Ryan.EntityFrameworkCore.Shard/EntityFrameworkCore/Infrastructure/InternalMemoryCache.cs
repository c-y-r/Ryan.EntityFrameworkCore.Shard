using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Ryan.EntityFrameworkCore.Infrastructure
{
    /// <summary>
    /// 内部缓存
    /// </summary>
    public class InternalMemoryCache : MemoryCache, IMemoryCache
    {
        public InternalMemoryCache() : base(new MemoryCacheOptions() 
        {
            SizeLimit = 300
        })
        {
        }
    }
}
