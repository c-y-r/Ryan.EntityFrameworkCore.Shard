using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <summary>
    /// 实体模型构建访问器
    /// </summary>
    public class EntityModelBuilderAccessor
    {
        /// <summary>
        /// 实体类型
        /// </summary>
        public Type EntityType { get; }

        /// <summary>
        /// 实体实现字典
        /// </summary>
        public EntityImplementationDictionary Dictionary { get; }

        /// <summary>
        /// 实体模型构建
        /// </summary>
        public object EntityModelBuilder { get; }

        /// <summary>
        /// 访问器
        /// </summary>
        public Action<Type, ModelBuilder> Accessor { get; protected set; }

        /// <summary>
        /// 创建实体模型构建访问器
        /// </summary>
        public EntityModelBuilderAccessor(Type entityType, EntityImplementationDictionary dictionary, object entityModelBuilder)
        {
            EntityType = entityType;
            Dictionary = dictionary;
            EntityModelBuilder = entityModelBuilder;

            // 获取构建方法
            var buildMethodInfo = EntityModelBuilder.GetType().GetMethod("Build")!;

            // 创建委托 (如果不大量动态建表，效率不会太低)
            Accessor = (t, m) =>
            {
                foreach (var entityImplementation in Dictionary.Values.Where(x => x.EntityType == t))
                {
                    buildMethodInfo
                        .MakeGenericMethod(entityImplementation.ImplementationType)
                        .Invoke(entityModelBuilder, new object[] { m, entityImplementation.TableName });
                }
            };
        }
    }
}
