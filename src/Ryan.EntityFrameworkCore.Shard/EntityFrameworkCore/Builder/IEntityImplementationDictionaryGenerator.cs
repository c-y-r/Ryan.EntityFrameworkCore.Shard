namespace Ryan.EntityFrameworkCore.Builder
{
    /// <summary>
    /// 实体实现字典生成器
    /// </summary>
    public interface IEntityImplementationDictionaryGenerator
    {
        /// <summary>
        /// 创建 实体实现字典
        /// </summary>
        EntityImplementationDictionary Create(Type entityType);
    }
}
