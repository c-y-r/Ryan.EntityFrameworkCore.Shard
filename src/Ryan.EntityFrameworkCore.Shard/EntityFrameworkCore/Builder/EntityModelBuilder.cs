using Microsoft.EntityFrameworkCore;
using Ryan.EntityFrameworkCore.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Ryan.EntityFrameworkCore.Builder
{
    /// <summary>
    /// 实体模型构建器接口
    /// </summary>
    public interface IEntityModelBuilder
    {
        /// <summary>
        /// 获取访问器
        /// </summary>
        /// <returns></returns>
        IEnumerable<EntityExpressionVisitor> GetExpressionVisitors();

        /// <summary>
        /// 获取表名
        /// </summary>
        string GetTableName(Dictionary<string, string> value);
    }

    /// <summary>
    /// 实体模型构造器
    /// </summary>
    public abstract class EntityModelBuilder<TEntity> : IEntityModelBuilder where TEntity : class
    {
        /// <summary>
        /// 访问器
        /// </summary>
        private List<Func<EntityExpressionVisitor>> Visitors { get; } = new List<Func<EntityExpressionVisitor>>();

        /// <summary>
        /// 创建实体模型构建器
        /// </summary>
        public EntityModelBuilder()
        {
            EntityConfiguration();
        }

        /// <summary>
        /// 实体配置
        /// </summary>
        protected abstract void EntityConfiguration();

        /// <summary>
        /// 应用分表
        /// </summary>
        protected void Apply<TMember>(Expression<Func<TEntity, TMember>> expression, Func<TMember, string> converter = null)
        {
            Visitors.Add(() => new EntityExpressionVisitor<TMember>(expression, converter));
        }

        /// <inheritdoc/>
        public virtual IEnumerable<EntityExpressionVisitor> GetExpressionVisitors()
        {
            return Visitors.Select(x => x());
        }

        /// <inheritdoc/>
        public abstract string GetTableName(Dictionary<string, string> value);

        /// <summary>
        /// 构建 <typeparamref name="TImplementation"/> 类型 Model
        /// </summary>
        public abstract void Build<TImplementation>(ModelBuilder modelBuilder, string tableName) where TImplementation : TEntity;
    }
}
