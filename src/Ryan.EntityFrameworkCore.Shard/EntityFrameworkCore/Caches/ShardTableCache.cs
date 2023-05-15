using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Ryan.EntityFrameworkCore.Caches
{
    /// <summary>
    /// 分表缓存
    /// </summary>
    public class ShardTableCache
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; protected set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Type TableType { get; protected set; }

        /// <summary>
        /// 从表名生成实例
        /// </summary>
        public static ShardTableCache FromTableName(string tableName, Type baseType)
        {
            var typeName = $"SHARD_{ShardService.Hash(tableName)}";
            var template = $@"
namespace {baseType.Namespace};

public class {typeName} : {baseType.Name} {{ }}
";

            // 解析语法树
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(template);

            // 获取临时文件
            var assemblyName = Path.GetRandomFileName();
            var references = baseType.Assembly.GetReferencedAssemblies()
                .Select(x => MetadataReference.CreateFromFile(Assembly.Load(x).Location))
                .Union(new MetadataReference[]
                {
                    MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                    MetadataReference.CreateFromFile(baseType.Assembly.Location)
                });

            // 编译器
            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // 获取内容
            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);

            if (!result.Success)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics.Select(x => x.GetMessage())));
            }

            // 获取类型
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
            var type = assembly.GetType($"{baseType.Namespace}.{typeName}");

            return new ShardTableCache()
            {
                TableName = tableName,
                TableType = type
            };
        }
    }
}
