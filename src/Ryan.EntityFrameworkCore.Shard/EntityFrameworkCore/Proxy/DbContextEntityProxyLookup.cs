using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <summary>
    /// 上下文实体代理字典
    /// </summary>
    public class DbContextEntityProxyLookup : ConcurrentDictionary<Type, DbContextEntityProxy>
    {
        private readonly IDbContextEntityProxyGenerator _dbContextEntityProxyGenerator;

        public DbContextEntityProxyLookup(IDbContextEntityProxyGenerator dbContextEntityProxyGenerator)
        {
            _dbContextEntityProxyGenerator = dbContextEntityProxyGenerator;
        }

        public DbContextEntityProxy GetOrDefault(Type entityType, DbContext context)
        {
            return GetOrAdd(entityType, _dbContextEntityProxyGenerator.Create(context));
        }

        public void Changes()
        {
            foreach (var context in Values)
            {
                for (int i = context.EntityProxies.Count - 1; i >= 0; i--)
                {
                    if (!context.EntityProxies[i].IsStated())
                    {
                        context.EntityProxies.RemoveAt(i);
                        continue;
                    }

                    context.EntityProxies[i].Changes();
                }
            }
        }

        public void Changed()
        {
            foreach (var context in Values)
            {
                for (int i = context.EntityProxies.Count - 1; i >= 0; i--)
                {
                    context.EntityProxies[i].Changed();
                }
            }
        }
    }
}
