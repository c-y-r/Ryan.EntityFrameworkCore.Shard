using System;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <summary>
    /// 实体模型构建对象生成器
    /// </summary>
    public interface IEntityModelBuilderGenerator
    {
        /// <summary>
        /// 创建 <paramref name="entityType"/> 类型的实体模型构建对象
        /// </summary>
        public object Create(Type entityType);
    }
}
