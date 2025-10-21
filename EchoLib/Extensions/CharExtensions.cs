using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoLib.Extensions
{
    public static class CharExtensions
    {
        public static string Representation(this char? self) =>
            (self is not null) ? (Extensions.CharExtensions.FormatAsLiteral(self)) : (Internal.nullRepresentation);

        public static string FormatAsLiteral(this char? self)
        {
            ArgumentNullException.ThrowIfNull(self, nameof(self));

            return Internal.FormatCharAsLiteral((char)self);
        }
    }
}
