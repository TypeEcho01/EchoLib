using EchoLib.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoLib.Extensions
{
    public static class ObjectExtensions
    {
        public static string Representation(this object? self)
        {
            if (self is null)
                return Internal.nullRepresentation;

            if (self is char c)
                return Extensions.CharExtensions.Representation(c);

            if (self is string s)
                return Extensions.StringExtensions.Representation(s);

            return Internal.Output.Get(self);
        }

        public static string FormatTypeName(this object? self) =>
            (self?.GetType().FormatName()) ?? (Internal.nullRepresentation);

        public static LiteralString ToLiteralString(this object? self)
        {
            ArgumentNullException.ThrowIfNull(self, nameof(self));

            string? toString = self.ToString();
            if (toString is null)
                throw new InvalidOperationException($"{self.FormatTypeName()}.ToString() returned null.");

            return new LiteralString(toString);
        }
    }
}
