using System;

namespace Ryan.EntityFrameworkCore.Dynamic
{
    /// <summary>
    /// 源码生成器
    /// </summary>
    public interface IDynamicSourceCodeGenerator
    {
        /// <summary>
        /// 生成指定类型动态源码
        /// </summary>
        public string Create(Type type);
    }
}
