using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ryan.EntityFrameworkCore.Dynamic
{
    /// <inheritdoc cref="IDynamicTypeGenerator"/>
    public class DynamicTypeGenerator : IDynamicTypeGenerator
    {
        /// <summary>
        /// 动态源码生成器
        /// </summary>
        public IDynamicSourceCodeGenerator DynamicSourceCodeGenerator { get; }

        /// <summary>
        /// 创建动态类型生成器
        /// </summary>
        public DynamicTypeGenerator(IDynamicSourceCodeGenerator dynamicSourceCodeGenerator)
        {
            DynamicSourceCodeGenerator = dynamicSourceCodeGenerator;
        }

        /// <inheritdoc/>
        public virtual Type Create(Type entityType)
        {
            // 创建编译选项
            var options = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);

            // 创建语法树
            var syntaxTree = CSharpSyntaxTree.ParseText(DynamicSourceCodeGenerator.Create(entityType));

            // 创建编译上下文
            var assemblyName = Guid.NewGuid().ToString();
            var references = GetReferencedAssemblies(entityType).Select(x => MetadataReference.CreateFromFile(x.Location));

            // 编译
            var compilation = CSharpCompilation.Create(assemblyName)
                .WithOptions(options)
                .AddReferences(references)
                .AddSyntaxTrees(syntaxTree);

            // 获取编译结果
            using var ms = new MemoryStream();

            // 编译
            var result = compilation.Emit(ms);
            if (!result.Success)
            {
                throw new InvalidOperationException(string.Join(Environment.NewLine, result.Diagnostics));
            }

            // 加载程序集
            ms.Seek(0, SeekOrigin.Begin);
            var assembly = Assembly.Load(ms.ToArray());

            // 返回实体类型
            return assembly.GetTypes().First(x => x.BaseType == entityType);
        }

        /// <summary>
        /// 获取 <paramref name="entityType"/> 所有引用的程序集
        /// </summary>
        protected virtual IEnumerable<Assembly> GetReferencedAssemblies(Type entityType)
        {
            var assemblies = new HashSet<Assembly>();

            // 获取类型所在的程序集
            var typeAssembly = Assembly.GetAssembly(entityType)!;
            if (!assemblies.Contains(typeAssembly))
            {
                GetReferencedAssembliesRecursive(typeAssembly, assemblies);
            }

            return assemblies;
        }

        /// <summary>
        /// 递归获取所有引用程序集
        /// </summary>
        private void GetReferencedAssembliesRecursive(Assembly assembly, HashSet<Assembly> assemblies)
        {
            if (!assemblies.Contains(assembly))
            {
                assemblies.Add(assembly);

                // 获取程序集引用的其他程序集
                var referencedAssemblyNames = assembly.GetReferencedAssemblies();

                foreach (AssemblyName referencedAssemblyName in referencedAssemblyNames)
                {
                    // 加载被引用的程序集
                    var referencedAssembly = Assembly.Load(referencedAssemblyName);

                    // 递归获取被引用程序集的引用
                    GetReferencedAssembliesRecursive(referencedAssembly, assemblies);
                }
            }
        }
    }
}
