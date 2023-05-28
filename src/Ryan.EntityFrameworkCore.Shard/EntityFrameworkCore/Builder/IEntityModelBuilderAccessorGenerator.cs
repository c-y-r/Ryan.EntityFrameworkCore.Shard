using System;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <summary>
    /// 实体模型构建访问器
    /// </summary>
    public interface IEntityModelBuilderAccessorGenerator
    {
        /// <summary>
        /// 创建实体模型构建访问器
        /// </summary>
        EntityModelBuilderAccessor Create(Type entityType);
    }
}
