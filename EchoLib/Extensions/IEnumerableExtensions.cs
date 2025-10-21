using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoLib.Extensions
{
    public static class IEnumerableExtensions
    {
        /*=============================================================================================================
        Enumerate
        =============================================================================================================*/
        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T>? self)
        {
            if (self is null)
                ArgumentNullException.ThrowIfNull(self, nameof(self));

            return Functions.Enumerate(self);
        }

        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T>? self, int start)
        {
            if (self is null)
                ArgumentNullException.ThrowIfNull(self, nameof(self));

            return Functions.Enumerate(self, start);
        }

        public static IEnumerable<(int, T)> Enumerate<T>(this IEnumerable<T>? self, int start, int step)
        {
            if (self is null)
                ArgumentNullException.ThrowIfNull(self, nameof(self));

            return Functions.Enumerate(self, start, step);
        }


        /*=============================================================================================================
        ForEach
        =============================================================================================================*/
        public static void ForEach<T>(this IEnumerable<T>? self, Action<T> action)
        {
            if (self is null)
                ArgumentNullException.ThrowIfNull(self, nameof(self));

            Functions.ForEach(self, action);
        }

        public static void ForEach<T>(this IEnumerable<T>? self, Action<T, Index> action)
        {
            if (self is null)
                ArgumentNullException.ThrowIfNull(self, nameof(self));

            Functions.ForEach(self, action);
        }
    }
}
