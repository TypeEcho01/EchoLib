using EchoLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EchoLib.Extensions
{
    public static partial class StringExtensions
    {
        public static string Representation(this string? self) =>
            (self is not null) ? (Extensions.StringExtensions.FormatAsLiteral(self)) : (Internal.nullRepresentation);

        public static string FormatAsLiteral(this string? self)
        {
            ArgumentNullException.ThrowIfNull(self, nameof(self));

            return Internal.FormatStringAsLiteral(self);
        }

        public static LiteralString ToLiteralString(this string? self)
        {
            if (self is null)
                ArgumentNullException.ThrowIfNull(self);

            return new LiteralString(self);
        }

        public static int CompareTo(this string? self, EchoLib.Classes.String? strB)
        {
            ArgumentNullException.ThrowIfNull(self, nameof(self));

            return self.CompareTo(strB?.internalValue);
        }

        public static bool IsValidIdentifier(this string? self)
        {
            ArgumentNullException.ThrowIfNull(self, nameof(self));

            return IdentifierRegex().IsMatch(self);
        }

        // Regex ensures:
        // 1. First char is a letter
        // 2. Remaining chars are only letters or digits
        [GeneratedRegex(@"^[A-Za-z][A-Za-z0-9]*$")]
        private static partial Regex IdentifierRegex();
    }
}
