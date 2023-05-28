using Microsoft.EntityFrameworkCore;

namespace Ryan.EntityFrameworkCore.Proxy
{
    /// <summary>
    /// 实体代理
    /// </summary>
    public class EntityProxy
    {
        /// <summary>
        /// 实体
        /// </summary>
        public object Entity { get; }

        /// <summary>
        /// 实现
        /// </summary>
        public object Implementation { get; }

        /// <summary>
        /// 代理类型
        /// </summary>
        public EntityProxyType Type { get; }

        /// <summary>
        /// 上下文
        /// </summary>
        public DbContext DbContext { get; }

        /// <summary>
        /// 创建实体代理
        /// </summary>
        public EntityProxy(object entity, object implementation, EntityProxyType type, DbContext dbContext)
        {
            Entity = entity;
            Implementation = implementation;
            Type = type;
            DbContext = dbContext;
        }

        /// <summary>
        /// 是否已被状态管理
        /// </summary>
        public bool IsStated()
        {
            return DbContext.Entry(Implementation).State != EntityState.Detached;
        }

        /// <summary>
        /// 实体映射
        /// </summary>
        public void Changes()
        {
            if (Type == EntityProxyType.NonQuery)
            {
                EntityFromImplementation();
            }
        }

        /// <summary>
        /// 实体反向映射
        /// </summary>
        public void Changed()
        {
            if (Type == EntityProxyType.NonQuery)
            {
                EntityFromImplementationReverse();
            }
        }

        /// <summary>
        /// 实体从实现映射
        /// </summary>
        public void EntityFromImplementation()
        {
            foreach (var propertyInfo in Implementation.GetType().BaseType!.GetProperties())
            {
                var val = propertyInfo.GetValue(Entity, null);
                propertyInfo.SetValue(Implementation, val, null);
            }
        }

        /// <summary>
        /// 实现反向映射
        /// </summary>
        public void EntityFromImplementationReverse()
        {
            foreach (var propertyInfo in Implementation.GetType().BaseType!.GetProperties())
            {
                var val = propertyInfo.GetValue(Implementation, null);
                propertyInfo.SetValue(Entity, val, null);
            }
        }
    }
}
