using System.Text;

namespace Ryan.EntityFrameworkCore.Dynamic
{
    /// <inheritdoc cref="IDynamicSourceCodeGenerator"/>
    public class DynamicSourceCodeGenerator : IDynamicSourceCodeGenerator
    {
        /// <inheritdoc/>
        public virtual string Create(Type entityType)
        {
            var sourceCodeBuilder = new StringBuilder();
            sourceCodeBuilder.Append($"using {typeof(object).Namespace};");

            BuildNamespaceType(sourceCodeBuilder, entityType);

            return sourceCodeBuilder.ToString();
        }

        /// <summary>
        /// 构建命名空间以及类型
        /// </summary>
        public virtual void BuildNamespaceType(StringBuilder sourceCodeBuilder, Type entityType)
        {
            sourceCodeBuilder.Append($"namespace {entityType.Namespace} {{");

            var rd = Guid.NewGuid().ToString("N");
            var className = $"{entityType.Name}_{rd}";
            sourceCodeBuilder.Append($"public class {className}:{entityType.Name}{{public {className}():base(){{}}}}");

            sourceCodeBuilder.Append('}');
        }
    }
}
