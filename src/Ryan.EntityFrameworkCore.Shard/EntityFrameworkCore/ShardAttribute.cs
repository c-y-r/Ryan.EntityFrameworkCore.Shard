namespace Ryan.EntityFrameworkCore
{
    /// <summary>
    /// 分表特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ShardAttribute : Attribute
    {
        private readonly Type[] _shardEntities;

        /// <summary>
        /// 创建分表特性
        /// </summary>
        public ShardAttribute(params Type[] shardEntities)
        {
            _shardEntities = shardEntities;
        }

        /// <summary>
        /// 获取分表实体
        /// </summary>
        public virtual Type[] GetShardEntities() => _shardEntities;
    }
}
