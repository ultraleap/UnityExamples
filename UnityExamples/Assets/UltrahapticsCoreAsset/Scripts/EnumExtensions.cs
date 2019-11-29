using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace UltrahapticsCoreAsset
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum source)
        {
            Type type = source.GetType();
            string name = Enum.GetName(type, source);
            if (name != null)
            {
                FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr =
                        Attribute.GetCustomAttribute(field,
                            typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<T> Values<T>()
            where T : struct, IComparable, IFormattable, IConvertible
        //    where T : System.Enum C# 7.3
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }
    }


}
