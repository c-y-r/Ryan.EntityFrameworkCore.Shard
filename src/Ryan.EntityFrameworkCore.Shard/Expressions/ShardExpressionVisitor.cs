using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ryan.Expressions
{
    /// <summary>
    /// 表达式访问器
    /// </summary>
    public class ShardExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// Lambda 表达式
        /// </summary>
        public LambdaExpression LambdaExpression { get; }

        /// <summary>
        /// 分表属性
        /// </summary>
        public MemberExpression MemberExpression { get; }

        /// <summary>
        /// 分表参数
        /// </summary>
        public HashSet<string> Values { get; }

        /// <summary>
        /// 创建分表表达式访问器
        /// </summary>
        public ShardExpressionVisitor(LambdaExpression lambdaExpression)
        {
            Values = new HashSet<string>();
            LambdaExpression = lambdaExpression;
            MemberExpression = lambdaExpression.Body as MemberExpression;
        }

        /// <inheritdoc/>
        public override Expression Visit(Expression node)
        {
            return base.Visit(node);
        }

        /// <summary>
        /// 访问实体
        /// </summary>
        /// <param name="entities">实体集合</param>
        public virtual void Visit(IEnumerable<object> entities)
        {
        }

        /// <inheritdoc/>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType != ExpressionType.Equal)
            {
                return base.VisitBinary(node);
            }

            // x =>
            // x.Member == Something
            if (node.Left is MemberExpression && node.Right is ConstantExpression)
            {
                VisitBinaryMemberEqualConstant(node.Left as MemberExpression, node.Right as ConstantExpression);
            }
            // Something == x.Member
            else if (node.Left is ConstantExpression && node.Right is MemberExpression)
            {
                VisitBinaryMemberEqualConstant(node.Right as MemberExpression, node.Left as ConstantExpression);
            }

            // var other = new Something();
            // x =>
            // x.Member == other.Something
            else if (node.Left is MemberExpression && node.Right is MemberExpression)
            {
                VisitBinaryMemberEqualMember(node.Left as MemberExpression, node.Right as MemberExpression);
            }

            return base.VisitBinary(node);
        }

        /// <summary>
        /// 对 BinaryExpression 中 Left 和 Right 进行取值判断是否满足条件，满足则获取对应值
        /// </summary>
        /// <param name="memberExpression">成员表达式</param>
        /// <param name="constantExpression">常量表达式</param>
        /// <returns>判断是否取值</returns>
        protected virtual bool VisitBinaryMemberEqualConstant(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            return false;
        }

        /// <summary>
        /// 对 BinaryExpression 中 Left 和 Right 进行取值判断是否满足条件，满足则获取对应值
        /// </summary>
        /// <param name="leftMemberExpression">左侧成员表达式</param>
        /// <param name="rightMemberExpression">右侧成员表达式</param>
        /// <returns>判断是否取值</returns>
        protected virtual bool VisitBinaryMemberEqualMember(MemberExpression leftMemberExpression, MemberExpression rightMemberExpression)
        {
            return false;
        }
    }
}
