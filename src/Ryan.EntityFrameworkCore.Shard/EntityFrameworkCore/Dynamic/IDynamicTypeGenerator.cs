using System;

namespace Ryan.EntityFrameworkCore.Dynamic
{
    /// <summary>
    /// 动态类型生成器
    /// </summary>
    public interface IDynamicTypeGenerator
    {
        /// <summary>
        /// 创建动态类型
        /// </summary>
        Type Create(Type type);
    }
}
