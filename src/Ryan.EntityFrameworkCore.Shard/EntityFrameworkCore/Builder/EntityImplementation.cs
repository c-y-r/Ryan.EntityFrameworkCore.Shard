namespace Ryan.EntityFrameworkCore.Builder
{
    /// <summary>
    /// 实体实现
    /// </summary>
    public class EntityImplementation
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 实现类型
        /// </summary>
        public Type ImplementationType { get; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// 创建实体实现
        /// </summary>
        public EntityImplementation(Type entityType, Type implementationType, string tableName)
        {
            EntityType = entityType;
            ImplementationType = implementationType;
            TableName = tableName;
        }
    }
}
