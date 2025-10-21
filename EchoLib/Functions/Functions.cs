using EchoLib.Extensions;
using System.ComponentModel;
using System.Text;

namespace EchoLib
{
    public static class Functions
    {
        /*=============================================================================================================
        Enumerate
        =============================================================================================================*/
        public static IEnumerable<(int, T)> Enumerate<T>(IEnumerable<T> enumerable) =>
            Functions.Enumerate<T>
            (
                enumerable: enumerable, 
                start: Internal.Functions.Enumerate.defaultStart, 
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T)> Enumerate<T>(IEnumerable<T> enumerable, int start) =>
            Functions.Enumerate<T>
            (
                enumerable: enumerable,
                start: start, 
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T)> Enumerate<T>(IEnumerable<T> enumerable, int start, int step)
        {
            int count = start;

            foreach (T element in enumerable)
            {
                yield return (count, element);

                count += step;
            }
        }


        /*=============================================================================================================
        ForEach
        =============================================================================================================*/
        public static void ForEach<T>(IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T element in enumerable)
                action(element);
        }

        public static void ForEach<T>(IEnumerable<T> enumerable, Action<T, Index> action)
        {
            foreach (var (i, element) in Functions.Enumerate(enumerable))
                action(element, i);
        }


        /*=============================================================================================================
        Input
        =============================================================================================================*/
        public static string Input() =>
            (Console.ReadLine()) ?? (string.Empty);

        public static string Input(string prompt)
        {
            Console.Write(prompt);

            return Input();
        }

        public static T? Input<T>()
        {
            string input = Input();

            try
            {
                return (T)Convert.ChangeType(input, typeof(T));
            }
            catch (Exception ex) when ((ex is InvalidCastException) || (ex is FormatException))
            {
                return default;
            }
        }

        public static T? Input<T>(string prompt)
        {
            Console.Write(prompt);

            return Input<T>();
        }


        /*=============================================================================================================
        Print
        =============================================================================================================*/
        public static void Print() =>
            Internal.Functions.Print.Output();

        public static void Print(KeywordArguments? kwargs)
        {
            if (kwargs is null)
            {
                Internal.Functions.Print.Output(Internal.nullRepresentation);
                return;
            }

            var endKwarg = kwargs.Get<string>("end");
            if (endKwarg is not null)
            {
                Internal.Functions.Print.Write(endKwarg.Value);
                return;
            }

            Internal.Functions.Print.Output();
        }

        public static void Print(object? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print(string? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print(Classes.String? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print(int? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print(double? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print(char? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print(bool? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print<T>(T[]? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print<T>(T[,]? arg) =>
            Internal.Functions.Print.Output(arg);

        public static void Print(params object?[]? args)
        {
            if (args is null)
            {
                Internal.Functions.Print.Output(Internal.nullRepresentation);
                return;
            }

            int argc = args.Length;
            if (argc == 0)
            {
                Internal.Functions.Print.Output();
                return;
            }

            string sep = Internal.Functions.Print.defaultSep;
            string end = Internal.Functions.Print.defaultEnd;

            int lastIndex = argc - 1;
            if (args[lastIndex] is KeywordArguments kwargs)
            {
                var sepKwarg = kwargs.Get<string>("sep");
                if (sepKwarg is not null)
                    sep = sepKwarg.Value;

                var endKwarg = kwargs.Get<string>("end");
                if (endKwarg is not null)
                    end = endKwarg.Value;

                args[lastIndex] = null;
                lastIndex--;
            }

            var builder = new StringBuilder();

            foreach (var (i, value) in Functions.Enumerate(args))
            {
                builder.Append(Internal.Output.Get(value));

                if (i == lastIndex)
                    break;

                builder.Append(sep);
            }

            builder.Append(end);

            Internal.Functions.Print.Output(builder);
        }

        /*=============================================================================================================
        Zip
        =============================================================================================================*/
        public static IEnumerable<(T1, T2)> Zip<T1, T2>
        (IList<T1> a, IList<T2> b)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b);

            for (int i = 0; i < length; i++)
                yield return (a[i], b[i]);
        }

        public static IEnumerable<(T1, T2, T3)> Zip<T1, T2, T3>
        (IList<T1> a, IList<T2> b, IList<T3> c)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b, c);

            for (int i = 0; i < length; i++)
                yield return (a[i], b[i], c[i]);
        }

        public static IEnumerable<(T1, T2, T3, T4)> Zip<T1, T2, T3, T4>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b, c, d);

            for (int i = 0; i < length; i++)
                yield return (a[i], b[i], c[i], d[i]);
        }

        public static IEnumerable<(T1, T2, T3, T4, T5)> Zip<T1, T2, T3, T4, T5>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b, c, d, e);

            for (int i = 0; i < length; i++)
                yield return (a[i], b[i], c[i], d[i], e[i]);
        }

        public static IEnumerable<(T1, T2, T3, T4, T5, T6)> Zip<T1, T2, T3, T4, T5, T6>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e, IList<T6> f)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b, c, d, e, f);

            for (int i = 0; i < length; i++)
                yield return (a[i], b[i], c[i], d[i], e[i], f[i]);
        }

        /*=============================================================================================================
        ZipEnumerate
        =============================================================================================================*/
        public static IEnumerable<(int, T1, T2)> ZipEnumerate<T1, T2>
        (IList<T1> a, IList<T2> b) =>
            Functions.ZipEnumerate
            (
                a, b,

                start: Internal.Functions.Enumerate.defaultStart,
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T1, T2)> ZipEnumerate<T1, T2>
        (IList<T1> a, IList<T2> b, int start) =>
            Functions.ZipEnumerate
            (
                a, b, 

                start: start,
                step: Internal.Functions.Enumerate.defaultStep
            );


        public static IEnumerable<(int, T1, T2)> ZipEnumerate<T1, T2>
        (IList<T1> a, IList<T2> b, int start, int step)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b);

            int count = start;

            for (int i = 0; i < length; i++)
            {
                yield return (count, a[i], b[i]);

                count += step;
            }
        }

        public static IEnumerable<(int, T1, T2, T3)> ZipEnumerate<T1, T2, T3>
        (IList<T1> a, IList<T2> b, IList<T3> c) =>
            Functions.ZipEnumerate
            (
                a, b, c,

                start: Internal.Functions.Enumerate.defaultStart,
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T1, T2, T3)> ZipEnumerate<T1, T2, T3>
        (IList<T1> a, IList<T2> b, IList<T3> c, int start) =>
            Functions.ZipEnumerate
            (
                a, b, c,

                start: start,
                step: Internal.Functions.Enumerate.defaultStep
            );


        public static IEnumerable<(int, T1, T2, T3)> ZipEnumerate<T1, T2, T3>
        (IList<T1> a, IList<T2> b, IList<T3> c, int start, int step)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b, c);

            int count = start;

            for (int i = 0; i < length; i++)
            {
                yield return (count, a[i], b[i], c[i]);

                count += step;
            }
        }

        public static IEnumerable<(int, T1, T2, T3, T4)> ZipEnumerate<T1, T2, T3, T4>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d) =>
            Functions.ZipEnumerate
            (
                a, b, c, d,

                start: Internal.Functions.Enumerate.defaultStart,
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T1, T2, T3, T4)> ZipEnumerate<T1, T2, T3, T4>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, int start) =>
            Functions.ZipEnumerate
            (
                a, b, c, d,

                start: start,
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T1, T2, T3, T4)> ZipEnumerate<T1, T2, T3, T4>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, int start, int step)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b, c, d);

            int count = start;

            for (int i = 0; i < length; i++)
            {
                yield return (count, a[i], b[i], c[i], d[i]);

                count += step;
            }
        }

        public static IEnumerable<(int, T1, T2, T3, T4, T5)> ZipEnumerate<T1, T2, T3, T4, T5>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e) =>
            Functions.ZipEnumerate
            (
                a, b, c, d, e,

                start: Internal.Functions.Enumerate.defaultStart,
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T1, T2, T3, T4, T5)> ZipEnumerate<T1, T2, T3, T4, T5>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e, int start) =>
            Functions.ZipEnumerate
            (
                a, b, c, d, e,

                start: start,
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T1, T2, T3, T4, T5)> ZipEnumerate<T1, T2, T3, T4, T5>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e, int start, int step)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b, c, d, e);

            int count = start;

            for (int i = 0; i < length; i++)
            {
                yield return (count, a[i], b[i], c[i], d[i], e[i]);

                count += step;
            }
        }

        public static IEnumerable<(int, T1, T2, T3, T4, T5, T6)> ZipEnumerate<T1, T2, T3, T4, T5, T6>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e, IList<T6> f) =>
            Functions.ZipEnumerate
            (
                a, b, c, d, e, f,

                start: Internal.Functions.Enumerate.defaultStart,
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T1, T2, T3, T4, T5, T6)> ZipEnumerate<T1, T2, T3, T4, T5, T6>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e, IList<T6> f, int start) =>
            Functions.ZipEnumerate
            (
                a, b, c, d, e, f,

                start: start,
                step: Internal.Functions.Enumerate.defaultStep
            );

        public static IEnumerable<(int, T1, T2, T3, T4, T5, T6)> ZipEnumerate<T1, T2, T3, T4, T5, T6>
        (IList<T1> a, IList<T2> b, IList<T3> c, IList<T4> d, IList<T5> e, IList<T6> f, int start, int step)
        {
            int length = Internal.Functions.Zip.ValidateListLengths(a, b, c, d, e, f);

            int count = start;

            for (int i = 0; i < length; i++)
            {
                yield return (count, a[i], b[i], c[i], d[i], e[i], f[i]);

                count += step;
            }
        }
    }
}
