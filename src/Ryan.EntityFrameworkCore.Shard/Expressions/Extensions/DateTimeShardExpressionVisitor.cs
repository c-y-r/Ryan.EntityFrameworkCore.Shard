using System;
using System.Linq.Expressions;

namespace Ryan.Expressions.Extensions
{
    /// <inheritdoc/>
    public class DateTimeShardExpressionVisitor : ShardExpressionVisitor<DateTime>
    {
        /// <summary>
        /// DateTime Format
        /// </summary>
        public string Format { get; }

        /// <inheritdoc/>
        public DateTimeShardExpressionVisitor(LambdaExpression lambdaExpression, string format) : base(lambdaExpression)
        {
            Format = format ?? "yyyyMMdd";
        }

        /// <inheritdoc/>
        protected override bool AddExpressionValue(DateTime value)
        {
            return Values.Add(value.ToString(Format));
        }
    }
}
