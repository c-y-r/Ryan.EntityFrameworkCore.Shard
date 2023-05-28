using System;
using System.Linq.Expressions;

namespace Ryan.EntityFrameworkCore.Expressions
{
    /// <inheritdoc/>
    public class EntityExpressionVisitor<TValue> : EntityExpressionVisitor
    {
        /// <summary>
        /// 格式化
        /// </summary>
        public Func<TValue, string> Converter { get; }

        /// <summary>
        /// 创建实体访问器
        /// </summary>
        public EntityExpressionVisitor(LambdaExpression lambdaExpression, Func<TValue, string> converter) : base(lambdaExpression)
        {
            Converter = converter;
        }

        /// <inheritdoc/>
        public override void Visit(object entity)
        {
            var func = LambdaCache<TValue>.GetFunc(LambdaExpression);
            AddExpressionValue(func(entity));
        }

        /// <inheritdoc/>
        protected override bool VisitBinaryMemberEqualConstant(MemberExpression memberExpression, ConstantExpression constantExpression)
        {
            if (memberExpression.Member != MemberExpression.Member)
            {
                return false;
            }

            return AddExpressionValue((TValue)constantExpression.Value!);
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
