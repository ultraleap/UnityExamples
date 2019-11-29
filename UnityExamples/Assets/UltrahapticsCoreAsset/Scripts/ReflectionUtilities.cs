using System;
using System.Linq;
using System.Collections.Generic;

namespace UltrahapticsCoreAsset
{
    public static class ReflectionUtilities
    {
        public static Type GetType(string typeName, bool usingTestAssembly = false)
        {
            var types = GetExportedTypesFromAssemblies().Where(t => t.Name == typeName);

            if (usingTestAssembly)
            {
                return types.FirstOrDefault(t => t.FullName.Contains("Test"));
            }
            else
            {
                return types.FirstOrDefault(t => !t.FullName.Contains("Test"));
            }
        }

        public static IEnumerable<Type> GetExportedTypesFromAssemblies()
        {
            #if NET_STANDARD_2_0
                return AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !(a.IsDynamic))
                    .SelectMany(a => a.GetExportedTypes());
            #else
                return AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !(a.ManifestModule is System.Reflection.Emit.ModuleBuilder))
                    .SelectMany(a => a.GetExportedTypes());
            #endif
        }

    }
}
