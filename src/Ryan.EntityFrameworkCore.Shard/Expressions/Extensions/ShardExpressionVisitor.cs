using Ryan.Expressions.Caches;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ryan.Expressions.Extensions
{
    /// <inheritdoc/>
    public class ShardExpressionVisitor<TValue> : ShardExpressionVisitor
    {
        public ShardExpressionVisitor(LambdaExpression lambdaExpression) : base(lambdaExpression)
        {
        }

        /// <inheritdoc/>
        public override void Visit(IEnumerable<object> entities)
        {
            var func = LambdaCache<TValue>.GetFunc(LambdaExpression);
            foreach (var e in entities)
            {
                AddExpressionValue(func(e));
            }
        }

        /// <inheritdoc/>
        protected override bool VisitBinaryMemberEqualConstant(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            if (memberExpression.Member != MemberExpression.Member)
            {
                return false;
            }

            return AddExpressionValue((TValue)constantExpression.Value);
        }

        /// <inheritdoc/>
        protected override bool VisitBinaryMemberEqualMember(MemberExpression leftMemberExpression, MemberExpression rightMemberExpression)
        {
            if (leftMemberExpression.Member == rightMemberExpression.Member)
            {
                return false;
            }

            var expressionOrdered = leftMemberExpression.Member == MemberExpression.Member
                ? (leftMemberExpression, rightMemberExpression)
                : (rightMemberExpression, leftMemberExpression);

            // var other == new Something();
            // x =>
            // x.Member == other.Something
            var value = MemberCache<TValue>.GetFunc(expressionOrdered.Item2)();
            return AddExpressionValue(value);
        }

        /// <summary>
        /// 增加表达式值
        /// </summary>
        /// <param name="value">表达式值</param>
        /// <returns>是否添加成功</returns>
        protected virtual bool AddExpressionValue(TValue value)
        {
            return Values.Add(value.ToString());
        }
    }
}
