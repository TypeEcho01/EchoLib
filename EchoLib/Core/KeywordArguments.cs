using EchoLib.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Keyword = System.String;
using Argument = System.Object;
using KeywordArgumentPair = System.Collections.Generic.KeyValuePair<string, object?>;
using KeywordArgumentPairs = System.Collections.Generic.Dictionary<string, object?>;


namespace EchoLib
{
    public class KeywordArguments
    {
        public int Count => this.keywordArgumentPairs.Count;

        public static readonly KeywordArgumentsGenerator Kwargs = new KeywordArgumentsGenerator();

        private readonly KeywordArgumentPairs keywordArgumentPairs;

        internal KeywordArguments(KeywordArgumentPair keywordArgumentPair)
        {
            this.keywordArgumentPairs = new KeywordArgumentPairs()
            {
                { keywordArgumentPair.Key, keywordArgumentPair.Value }
            };
        }

        internal KeywordArguments(Keyword keyword, Argument? argument)
        {
            this.keywordArgumentPairs = new KeywordArgumentPairs()
            {
                { keyword, argument }
            };
        }

        internal KeywordArguments((Keyword keyword, Argument? argument) keywordArgumentPair)
        {
            this.keywordArgumentPairs = new KeywordArgumentPairs()
            {
                { keywordArgumentPair.keyword, keywordArgumentPair.argument }
            };
        }

        internal KeywordArguments(params (Keyword keyword, Argument? argument)[] keywordArgumentPairs)
        {
            this.keywordArgumentPairs = keywordArgumentPairs.ToDictionary();
        }

        internal KeywordArguments(KeywordArgumentPairs keywordArgumentPairs)
        {
            this.keywordArgumentPairs = new KeywordArgumentPairs(keywordArgumentPairs);
        }

        private static string GetPairString(Keyword keyword, Argument? argument)
        {
            string type;
            if (argument is null)
                type = "object?";
            else
                type = argument.FormatTypeName();

            string value = Internal.Output.BuildNestedString(argument);

            if (keyword.IsValidIdentifier())
                return $"{type} {keyword} = {value}";

            return $"{type} ({Internal.FormatStringAsLiteral(keyword)}) = {value}";
        }

        public override string ToString()
        {
            int length = this.Count;

            if (length == 0)
                return "Kwargs[]";

            var builder = new StringBuilder();
            int lastIndex = length - 1;

            foreach (var (i, pair) in Functions.Enumerate(this.keywordArgumentPairs))
            {
                builder.Append(KeywordArguments.GetPairString(pair.Key, pair.Value));

                if (i != lastIndex)
                    builder.Append(", \n\t");
            }

            if (length == 1)
                return $"Kwargs[{builder}]";

            return $"Kwargs\n[\n\t{builder}\n]";
        }

        public KeywordArgument<T>? Get<T>(Keyword keyword)
        {
            if (!this.keywordArgumentPairs.TryGetValue(keyword, out Argument? argument))
                return null;

            if (argument is T value)
                return new KeywordArgument<T>(value);

            return null;
        }
        
        public NullableKeywordArgument<T>? GetNullable<T>(Keyword keyword)
        {
            if (!this.keywordArgumentPairs.TryGetValue(keyword, out Argument? argument))
                return null;

            if (argument is null)
                return new NullableKeywordArgument<T>(null);

            if (argument is T value)
                return new NullableKeywordArgument<T>(value);

            return null;
        }
    }

    public class KeywordArgumentsGenerator
    {
        internal KeywordArgumentsGenerator() { }

        public KeywordArguments this[KeywordArgumentPair keywordArgumentPair] =>
            new KeywordArguments(keywordArgumentPair);

        public KeywordArguments this[Keyword keyword, Argument? argument] => 
            new KeywordArguments(keyword, argument);

        public KeywordArguments this[(Keyword keyword, Argument? argument) keywordArgumentPair] => 
            new KeywordArguments(keywordArgumentPair);

        public KeywordArguments this[params (Keyword keyword, Argument? argument)[] keywordArgumentPairs] => 
            new KeywordArguments(keywordArgumentPairs);

        public KeywordArguments this[KeywordArgumentPairs keywordArgumentPairs] => 
            new KeywordArguments(keywordArgumentPairs);

        public override string ToString() =>
            string.Empty;
    }

    public class KeywordArgument<T>
    {
        public readonly T Value;

        public KeywordArgument(T value)
        {
            this.Value = value;
        }
    }

    public class NullableKeywordArgument<T>
    {
        private readonly Argument? value;

        public T Value
        {
            get
            {
                if (this.value is null)
                    throw new InvalidOperationException("NullableKeywordArgument object must have a value.");

                return (T)this.value;
            }
        }

        public bool IsNull => (value is null);

        public NullableKeywordArgument(T? value)
        {
            this.value = value;
        }

        public NullableKeywordArgument(Argument? value)
        {
            this.value = value;
        }
    }
}
