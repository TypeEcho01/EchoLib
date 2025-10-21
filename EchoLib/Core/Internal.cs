using EchoLib.Classes;
using EchoLib.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static EchoLib.Internal.Functions;

namespace EchoLib
{
    internal class Internal
    {
        internal static readonly string nullRepresentation = "null";

        internal static readonly Dictionary<char, string> escapeSequenceReplacements = new Dictionary<char, string>
        {
            { '\a',  "\\a"  }, // Bell
            { '\b',  "\\b"  }, // Backspace
            { '\f',  "\\f"  }, // Formfeed
            { '\n',  "\\n"  }, // Newline
            { '\r',  "\\r"  }, // Carriage return
            { '\t',  "\\t"  }, // Horizontal tab
            { '\v',  "\\v"  }, // Vertical tab
            { '\\',  "\\\\" }, // Backslash
            { '\'',  "\\'"  }, // Single quote
            { '\"',  "\\\"" }, // Double quote
            { '\0',  "\\0" }   // Null terminator
        };

        internal static readonly HashSet<char> escapeSequences =
        [
            '\a', '\b', '\f', '\n', '\r', '\t', '\v', '\\', '\'', '\"', '\0'
        ];

        private static bool CharIsNotPrintable(char c) =>
            ((c < 32) || (c > 126));

        private static string GetNonPrintableStringRepresentation(int c) =>
            $"\\u{c:X4}";

        internal static IEnumerable TupleToEnumerable(ITuple tuple)
        {
            Type type = tuple.GetType();

            if (type.IsValueType)
                return Internal.GetValueTupleEnumerator(tuple, type);

            return Internal.GetTupleEnumerator(tuple, type);
        }

        private static IEnumerable GetTupleEnumerator(ITuple tuple, Type type)
        {
            for (int i = 1; ; i++)
            {
                var property = type.GetProperty($"Item{i}");

                if (property is null)
                    yield break;

                yield return property.GetValue(tuple);
            }
        }

        private static IEnumerable GetValueTupleEnumerator(ITuple tuple, Type type)
        {
            for (int i = 1; ; i++)
            {
                var field = type.GetField($"Item{i}");

                if (field is null)
                    yield break;

                yield return field.GetValue(tuple);
            }
        }

        internal static string FormatStringAsLiteral(string s)
        {
            var builder = new StringBuilder("\"");

            foreach (char c in s)
            {
                // Replace escape sequences with literal version
                if (Internal.escapeSequences.Contains(c))
                {
                    builder.Append(Internal.escapeSequenceReplacements[c]);
                    continue;
                }

                // Replace non-printable characters with unicode escape
                if (Internal.CharIsNotPrintable(c))
                {
                    builder.Append(Internal.GetNonPrintableStringRepresentation(c));
                    continue;
                }

                builder.Append(c);
            }

            builder.Append('\"');

            return builder.ToString();
        }

        internal static string FormatCharAsLiteral(char c)
        {
            string charRepresentation;

            if (Internal.escapeSequences.Contains(c))
                charRepresentation = Internal.escapeSequenceReplacements[c];
            else if (Internal.CharIsNotPrintable(c))
                charRepresentation = Internal.GetNonPrintableStringRepresentation(c);
            else
                charRepresentation = c.ToString();

            return $"\'{charRepresentation}\'";
        }

        internal static class Output
        {
            internal static string Get(object? obj) =>
                Output.BuildString(obj);

            private static string BuildString(object? obj)
            {
                if (obj is string str0)
                    return str0;

                if (obj is Classes.String str1)
                    return str1.value;

                if (obj is ITuple tuple)
                {
                    Type type = tuple.GetType();

                    if (type.IsValueType)
                        return Output.BuildValueTupleString(tuple, type);

                    return Output.BuildTupleString(tuple, type);
                }

                if (obj is IEnumerable enumerable)
                {
                    // if is multidimensional array
                    if ((enumerable is Array array) && (array.Rank > 1))
                        return Output.BuildMultidimensionalArrayString(array, 0, new int[array.Rank]);

                    return Output.BuildEnumerableString(enumerable);
                }

                return (obj?.ToString()) ?? (Internal.nullRepresentation);
            }

            internal static string BuildNestedString(object? obj)
            {
                if (obj is char c)
                    return Internal.FormatCharAsLiteral(c);

                if (obj is string str0)
                    return Internal.FormatStringAsLiteral(str0);

                if (obj is Classes.String str1)
                    return Internal.FormatStringAsLiteral(str1.value);

                if (obj is ITuple tuple)
                {
                    Type type = tuple.GetType();

                    if (type.IsValueType)
                        return Output.BuildNestedValueTupleString(tuple, type);

                    return Output.BuildNestedTupleString(tuple, type);
                }

                if (obj is IEnumerable enumerable)
                {
                    // if is multidimensional array
                    if ((enumerable is Array array) && (array.Rank > 1))
                        return Output.BuildNestedMultidimensionalArrayString(array, 0, new int[array.Rank]);

                    return Output.BuildNestedEnumerableString(enumerable);
                }

                return obj?.ToString() ?? Internal.nullRepresentation;
            }

            internal static string BuildTupleString(ITuple tuple, Type type)
            {
                var builder = new StringBuilder($"{type.FormatName()} (");

                bool enteredLoop = false;
                foreach (var element in Internal.GetTupleEnumerator(tuple, type))
                {
                    // Avoids placing a separator before the first element
                    if (enteredLoop)
                        builder.Append(", ");

                    builder.Append(Internal.Output.BuildNestedString(element));

                    enteredLoop = true;
                }

                builder.Append(')');

                return builder.ToString();
            }

            internal static string BuildNestedTupleString(ITuple tuple, Type type)
            {
                var builder = new StringBuilder("(");

                bool enteredLoop = false;
                foreach (var element in Internal.GetTupleEnumerator(tuple, type))
                {
                    // Avoids placing a separator before the first element
                    if (enteredLoop)
                        builder.Append(", ");

                    builder.Append(Internal.Output.BuildNestedString(element));

                    enteredLoop = true;
                }

                builder.Append(')');

                return builder.ToString();
            }

            internal static string BuildValueTupleString(ITuple tuple, Type type)
            {
                var builder = new StringBuilder("(");

                bool enteredLoop = false;
                foreach (var element in Internal.GetValueTupleEnumerator(tuple, type))
                {
                    // Avoids placing a separator before the first element
                    if (enteredLoop)
                        builder.Append(", ");

                    builder.Append(Internal.Output.BuildNestedString(element));

                    enteredLoop = true;
                }

                builder.Append(')');

                return builder.ToString();
            }

            internal static string BuildNestedValueTupleString(ITuple tuple, Type type) =>
                Internal.Output.BuildValueTupleString(tuple, type);

            internal static string BuildEnumerableString(IEnumerable enumerable)
            {
                var builder = new StringBuilder($"{enumerable.FormatTypeName()} {{ ");

                bool enteredLoop = false;
                foreach (var element in enumerable)
                {
                    // Avoids placing a separator before the first element
                    if (enteredLoop)
                        builder.Append(", ");

                    builder.Append(Internal.Output.BuildNestedString(element));

                    enteredLoop = true;
                }

                if (enteredLoop)          // Only add a space before the '}' if the loop was entered
                    builder.Append(" }");
                else                      // Otherwise, do not add a space so that the output is "{ }" and not "{  }"
                    builder.Append('}');

                return builder.ToString();
            }

            internal static string BuildNestedEnumerableString(IEnumerable enumerable)
            {
                var builder = new StringBuilder("{");

                bool enteredLoop = false;
                foreach (var element in enumerable)
                {
                    // Avoids placing a separator before the first element
                    if (enteredLoop)
                        builder.Append(", ");

                    builder.Append(Internal.Output.BuildNestedString(element));

                    enteredLoop = true;
                }

                builder.Append('}');

                return builder.ToString();
            }

            private static string BuildMultidimensionalArrayString(Array array, int dimension, int[] indices)
            {
                var builder = new StringBuilder($"{array.FormatTypeName()} {{ ");

                int length = array.GetLength(dimension);

                bool enteredLoop = false;
                for (int i = 0; i < length; i++)
                {
                    enteredLoop = true;

                    indices[dimension] = i;

                    if (i > 0)
                        builder.Append(", ");

                    if (dimension < array.Rank - 1)
                    {
                        // Recurse into next dimension
                        builder.Append(BuildNestedMultidimensionalArrayString(array, dimension + 1, indices));
                    }
                    else
                    {
                        // Base case: last dimension — print the actual element
                        object? value = array.GetValue(indices);
                        builder.Append(Internal.Output.BuildNestedString(value));
                    }
                }

                if (enteredLoop)          // Only add a space before the '}' if the loop was entered
                    builder.Append(" }");
                else                      // Otherwise, do not add a space so that the output is "{ }" and not "{  }"
                    builder.Append('}');
                return builder.ToString();
            }

            private static string BuildNestedMultidimensionalArrayString(Array array, int dimension, int[] indices)
            {
                var builder = new StringBuilder("{");

                int length = array.GetLength(dimension);

                for (int i = 0; i < length; i++)
                {
                    indices[dimension] = i;

                    if (i > 0)
                        builder.Append(", ");

                    if (dimension < array.Rank - 1)
                    {
                        // Recurse into next dimension
                        builder.Append(BuildNestedMultidimensionalArrayString(array, dimension + 1, indices));
                    }
                    else
                    {
                        // Base case: last dimension — print the actual element
                        object? value = array.GetValue(indices);
                        builder.Append(Internal.Output.BuildNestedString(value));
                    }
                }

                builder.Append('}');
                return builder.ToString();
            }
        }

        internal static class Functions
        {
            internal static class Enumerate
            {
                internal static readonly int defaultStart = 0;
                internal static readonly int defaultStep = 1;
            }

            internal static class Print
            {
                internal static readonly string defaultSep = " ";
                internal static readonly string defaultEnd = "\n";

                private static void WriteAndEnd() =>
                    Print.Write(Print.defaultEnd);

                private static void WriteAndEnd(string output) =>
                    Print.Write($"{output}{Print.defaultEnd}");

                internal static void Write(string output) =>
                    Console.Write(output);

                internal static void Output() =>
                    Print.WriteAndEnd();

                internal static void Output(object? obj) =>
                    Print.WriteAndEnd(Internal.Output.Get(obj));

                internal static void Output(Classes.String? s) =>
                    Print.WriteAndEnd((s?.value) ?? (Internal.nullRepresentation));

                internal static void Output(string? s) =>
                    Print.WriteAndEnd((s) ?? (Internal.nullRepresentation));

                internal static void Output(StringBuilder? builder) =>
                    Print.Write((builder?.ToString()) ?? (Internal.nullRepresentation));
            }

            internal static class Zip
            {
                private static void ListLengthDiscrepancy(int numberOfLists, params int[] lengths)
                {
                    string joined = string.Join(", ", lengths.Take(numberOfLists - 1)) + $" and {lengths[^1]}";

                    throw new ArgumentException
                    (
                        $"All lists must have the same length, but got {numberOfLists} lists with lengths of {joined}."
                    );
                }

                private static int ValidateAllListLengths(params System.Collections.ICollection[] lists)
                {
                    ArgumentNullException.ThrowIfNull(lists, nameof(lists));

                    int numberOfLists = lists.Length;

                    if (numberOfLists < 2)
                        throw new ArgumentOutOfRangeException
                        (
                            nameof(lists), numberOfLists, "At least two lists are required."
                        );

                    if (lists.Any(list => list is null))
                        throw new ArgumentException("None of the provided lists can be null.", nameof(lists));

                    int[] lengths = lists.Select(l => l.Count).ToArray();
                    int expectedLength = lengths[0];

                    for (int i = 1; i < numberOfLists; i++)
                    {
                        if (lengths[i] != expectedLength)
                            Zip.ListLengthDiscrepancy(numberOfLists, lengths);
                    }

                    return expectedLength;
                }

                internal static int ValidateListLengths<T1, T2>(ICollection<T1> a, ICollection<T2> b)
                {
                    ArgumentNullException.ThrowIfNull(a, nameof(a));
                    ArgumentNullException.ThrowIfNull(b, nameof(b));

                    int aLength = a.Count;
                    int bLength = b.Count;

                    if (aLength != bLength)
                        Zip.ListLengthDiscrepancy(2, aLength, bLength);

                    return aLength;
                }

                internal static int ValidateListLengths<T1, T2, T3>
                (ICollection<T1> a, ICollection<T2> b, ICollection<T3> c) =>
                    ValidateAllListLengths
                    (
                        (System.Collections.ICollection)a,
                        (System.Collections.ICollection)b,
                        (System.Collections.ICollection)c
                    );

                internal static int ValidateListLengths<T1, T2, T3, T4>
                (ICollection<T1> a, ICollection<T2> b, ICollection<T3> c, ICollection<T4> d) =>
                    ValidateAllListLengths
                    (
                        (System.Collections.ICollection)a,
                        (System.Collections.ICollection)b,
                        (System.Collections.ICollection)c, 
                        (System.Collections.ICollection)d
                    );

                internal static int ValidateListLengths<T1, T2, T3, T4, T5>
                (ICollection<T1> a, ICollection<T2> b, ICollection<T3> c, ICollection<T4> d, ICollection<T5> e) =>
                    ValidateAllListLengths
                    (
                        (System.Collections.ICollection)a,
                        (System.Collections.ICollection)b,
                        (System.Collections.ICollection)c,
                        (System.Collections.ICollection)d, 
                        (System.Collections.ICollection)e
                    );

                internal static int ValidateListLengths<T1, T2, T3, T4, T5, T6>
                (
                    ICollection<T1> a, ICollection<T2> b, ICollection<T3> c, 
                    ICollection<T4> d, ICollection<T5> e, ICollection<T6> f
                ) =>
                    ValidateAllListLengths
                    (
                        (System.Collections.ICollection)a,
                        (System.Collections.ICollection)b,
                        (System.Collections.ICollection)c,
                        (System.Collections.ICollection)d,
                        (System.Collections.ICollection)e, 
                        (System.Collections.ICollection)f
                    );
            }
        }
    }
}
