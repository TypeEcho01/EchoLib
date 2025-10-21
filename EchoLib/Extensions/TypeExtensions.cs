using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoLib.Extensions
{
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, string> typeAliases = new()
        {
            { typeof(void),    "void"    },
            { typeof(bool),    "bool"    },
            { typeof(byte),    "byte"    },
            { typeof(sbyte),   "sbyte"   },
            { typeof(char),    "char"    },
            { typeof(decimal), "decimal" },
            { typeof(double),  "double"  },
            { typeof(float),   "float"   },
            { typeof(int),     "int"     },
            { typeof(uint),    "uint"    },
            { typeof(long),    "long"    },
            { typeof(ulong),   "ulong"   },
            { typeof(short),   "short"   },
            { typeof(ushort),  "ushort"  },
            { typeof(object),  "object"  },
            { typeof(string),  "string"  }
        };

        private static string GetFormattedTypeName(Type? type)
        {
            if (type is null)
                return "?";

            // Basic Type
            if (typeAliases.TryGetValue(type, out string? alias))
                return alias;

            // Nullable
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                string inner = GetFormattedTypeName(type.GetGenericArguments()[0]);

                return $"{inner}?";
            }

            // Array
            if (type.IsArray)
            {
                string elementType = GetFormattedTypeName(type.GetElementType());

                int rank = type.GetArrayRank();
                if (rank == 1)
                    return $"{elementType}[]";

                return $"{elementType}[{new string(',', rank - 1)}]";
            }

            // Fallback
            if (!type.IsGenericType)
                return type.Name;


            // Generic types (List, HashSet, Dictionary, Tuple, etc.)
            Type genericDef = type.GetGenericTypeDefinition();

            // List
            if (genericDef == typeof(List<>))
            {
                string arg = GetFormattedTypeName(type.GetGenericArguments()[0]);

                return $"List<{arg}>";
            }

            // HashSet
            if (genericDef == typeof(HashSet<>))
            {
                string arg = GetFormattedTypeName(type.GetGenericArguments()[0]);

                return $"HashSet<{arg}>";
            }

            // Dictionary
            if (genericDef == typeof(Dictionary<,>))
            {
                var args = type.GetGenericArguments().Select(GetFormattedTypeName);

                return $"Dictionary<{string.Join(", ", args)}>";
            }

            // Tuple
            if (type.FullName?.StartsWith("System.Tuple") == true)
            {
                string args = string.Join(", ", type.GetGenericArguments().Select(GetFormattedTypeName));

                return $"Tuple<{args}>";
            }

            // Generic Fallback: (MyType<T1, T2>)
            string name = type.Name;

            int tickIndex = name.IndexOf('`');
            if (tickIndex >= 0)
                name = name[..tickIndex];

            string genericArgs = string.Join(", ", type.GetGenericArguments().Select(GetFormattedTypeName));

            return $"{name}<{genericArgs}>";
        }

        public static string FormatName(this Type? self) =>
            GetFormattedTypeName(self);
    }
}
