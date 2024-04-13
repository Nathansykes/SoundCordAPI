using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Project.Generic;
public static class Extensions
{
    public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2>? kvp, out T1 key, out T2 value)
    {
        if (kvp is null)
        {
            key = default!;
            value = default!;
            return;
        }
        key = kvp.Value.Key;
        value = kvp.Value.Value;
    }

    public static void Clear(this IMemoryCache cache) => (cache as MemoryCache)?.Clear();

    public static string[] SplitValuesToArray(this StringValues values, string delimiter = ",")
    {
        return values
            .SelectMany(x => x?.Split(delimiter, StringSplitOptions.RemoveEmptyEntries) ?? [])
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .ToArray();
    }

    public static TimeSpan Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, TimeSpan> selector)
    {
        return source.Select(selector).Aggregate(TimeSpan.Zero, (t1, t2) => t1 + t2);
    }


    public static void AddClassesAsImplementedInterface(
            this IServiceCollection services,
            IEnumerable<Assembly> assemblies,
            Type compareType,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        foreach (var assembly in assemblies)
        {
            services.AddClassesAsImplementedInterface(assembly, compareType, lifetime);
        }
    }

    public static void AddClassesAsImplementedInterface(
            this IServiceCollection services,
            Assembly assembly,
            Type compareType,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        assembly.GetTypesAssignableTo(compareType).ForEach((type) =>
        {
            foreach (var implementedInterface in type.ImplementedInterfaces)
            {
                switch (lifetime)
                {
                    case ServiceLifetime.Scoped:
                        services.AddScoped(implementedInterface, type);
                        break;
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(implementedInterface, type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(implementedInterface, type);
                        break;
                }
            }
        });
    }

    public static List<TypeInfo> GetTypesAssignableTo(this Assembly assembly, Type compareType)
    {
        var typeInfoList = assembly.DefinedTypes.Where(x =>
        {
            return x.IsClass && !x.IsAbstract && x != compareType
                    && x.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == compareType);
        }).ToList();

        return typeInfoList;
    }

    public static string GetBytesAsString(this byte[] value) => value.GetBytesAsString(Encoding.UTF8);
    public static string GetBytesAsString(this byte[] value, Encoding encoding) => encoding.GetString(value);
    public static byte[] GetStringAsBytes(this string value) => value.GetStringAsBytes(Encoding.UTF8);
    public static byte[] GetStringAsBytes(this string value, Encoding encoding) => encoding.GetBytes(value);
    public static string GetBytesAsHexString(this byte[] bytes, bool upperCase = true)
    {
        StringBuilder result = new StringBuilder(bytes.Length * 2);

        for (int i = 0; i < bytes.Length; i++)
            result.Append(bytes[i].ToString(upperCase ? "X2" : "x2"));

        return result.ToString();
    }
    public static byte[] ComputeMD5Hash(this string value) => value.ComputeMD5Hash(Encoding.UTF8);
    public static byte[] ComputeMD5Hash(this string value, Encoding encoding)
    {
        return value.GetStringAsBytes(encoding).ComputeMD5Hash();
    }
    public static byte[] ComputeMD5Hash(this byte[] bytes)
    {
        return MD5.HashData(bytes);
    }
}
