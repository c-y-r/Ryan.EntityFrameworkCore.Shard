using System;

namespace Ryan.DependencyInjection
{
    /// <summary>
    /// RyanService 提供依赖注入服务
    /// </summary>
    public class RyanService
    {
        /// <inheritdoc cref="IServiceProvider"/>
        public static IServiceProvider ServiceProvider { get; internal set; }

        /// <summary>
        /// 替换依赖注入服务
        /// </summary>
        public static void Replace(IServiceProvider newServiceProvider)
        {
            ServiceProvider = newServiceProvider;
        }
    }
}
