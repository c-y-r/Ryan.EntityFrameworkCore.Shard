using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System;

namespace Ryan
{
    /// <summary>
    /// 工具类
    /// </summary>
    public static class ShardService
    {
        /// <summary>
        /// 检查是否继承自泛型类型
        /// </summary>
        internal static bool IsTypeImplementGenericType(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur)
                {
                    return true;
                }

                toCheck = toCheck.BaseType;
            }

            return false;
        }

        /// <summary>
        /// 获取泛型类型
        /// </summary>
        internal static Type GetTypeImplementGenericType(Type generic, Type toGet)
        {
            while (toGet != null && toGet != typeof(object))
            {
                var cur = toGet.IsGenericType ? toGet.GetGenericTypeDefinition() : toGet;
                if (generic == cur)
                {
                    return toGet;
                }

                toGet = toGet.BaseType;
            }

            return null;
        }

        /// <summary>
        /// 检查是否实现泛型接口
        /// </summary>
        internal static bool IsTypeImplementGenericInterface(Type generic, Type toCheck)
        {
            foreach (Type interfaceType in toCheck.GetInterfaces())
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == generic)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 获取泛型接口类型
        /// </summary>
        internal static IEnumerable<Type> GetTypeImplementGenericInterfaces(Type generic, Type toGet)
        {
            foreach (Type interfaceType in toGet.GetInterfaces())
            {
                if (interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == generic)
                {
                    yield return interfaceType;
                }
            }
        }

        /// <summary>
        /// MD5
        /// </summary>
        internal static string Hash(string input)
        {
            // 将输入字符串转换为字节数组
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // 创建 MD5 实例
            using MD5 md5 = MD5.Create();

            // 计算哈希值
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // 将字节数组转换为十六进制字符串
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }

            // 返回哈希值的字符串表示
            return sb.ToString();
        }
    }
}
