using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime.CompilerServices;

namespace EchoLib.Extensions
{
    public static class ITupleExtensions
    {
        public static IEnumerable ToEnumerator(this ITuple tuple) =>
            Internal.TupleToEnumerable(tuple);
    }
}
