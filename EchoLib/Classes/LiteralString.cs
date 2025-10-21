using EchoLib.Extensions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EchoLib.Classes
{
    public class LiteralString : String
    {
        public static readonly LiteralString Empty = new LiteralString("");

        /*=============================================================================================================
        Constructors
        =============================================================================================================*/
        public LiteralString() : base
        (
            value: LiteralString.Empty.value,
            internalValue: LiteralString.Empty.internalValue
        )
        { }

        public LiteralString(string value) : base
        (
            value: Internal.FormatStringAsLiteral(value),
            internalValue: value
        )
        { }

        public LiteralString(EchoLib.Classes.String value) : base
        (
            value: Internal.FormatStringAsLiteral(value.value),
            internalValue: value.value
        )
        { }

        public LiteralString(LiteralString value) : base
        (
            value: value.value,
            internalValue: value.internalValue
        )
        { }


        /*=============================================================================================================
        Implicit Conversions
        =============================================================================================================*/

        // string to LiteralString
        public static implicit operator LiteralString(string self) => 
            new LiteralString(self);


        /*=============================================================================================================
        Operators
        ===============================================================================================================
        +
        -------------------------------------------------------------------------------------------------------------*/
        public static LiteralString operator +(LiteralString a, LiteralString b) =>
            LiteralString.Concat(a, b);

        public static LiteralString operator +(LiteralString a, String b) =>
            LiteralString.Concat(a, b);

        public static LiteralString operator +(String a, LiteralString b) =>
            LiteralString.Concat(a, b);

        public static LiteralString operator +(LiteralString a, string b) =>
            LiteralString.Concat(a, b);

        public static LiteralString operator +(string a, LiteralString b) =>
            LiteralString.Concat(a, b);

        /*=============================================================================================================
        Override Methods
        =============================================================================================================*/
        public override int GetHashCode() =>
            this.value.GetHashCode();


        /*=============================================================================================================
        Existing string Methods
        ===============================================================================================================
        Clone
        -------------------------------------------------------------------------------------------------------------*/
        public override object Clone() =>
            this;


        /*
        Concat
        -------------------------------------------------------------------------------------------------------------*/

        // Internal
        private static LiteralString ConcatInternal(string? str0, string? str1) => 
            new LiteralString(string.Concat(str0, str1));


        // External
        public static LiteralString Concat(string? str0, string? str1) => 
            ConcatInternal(str0, str1);

        public static LiteralString Concat(string? str0, String? str1) =>
            ConcatInternal(str0, str1?.internalValue);

        public static LiteralString Concat(String? str0, string? str1) => 
            ConcatInternal(str0?.internalValue, str1);

        public static LiteralString Concat(String? str0, String? str1) =>
            ConcatInternal(str0?.internalValue, str1?.internalValue);


        /*
        Contains
        -------------------------------------------------------------------------------------------------------------*/

        // Internal
        private bool InternalContains(char value) =>
            this.internalValue.Contains(value);

        private bool InternalContains(char value, StringComparison comparisonType) =>
            this.internalValue.Contains(value, comparisonType);

        private bool InternalContains(string value) =>
            this.internalValue.Contains(value);

        private bool InternalContains(string value, StringComparison comparisonType) =>
            this.internalValue.Contains(value, comparisonType);


        // External
        public bool Contains(char value) =>
            this.InternalContains(value);

        public bool Contains(char value, StringComparison comparisonType) =>
            this.InternalContains(value, comparisonType);

        public bool Contains(string value) =>
            this.InternalContains(value);

        public bool Contains(string value, StringComparison comparisonType) =>
            this.InternalContains(value, comparisonType);

        public bool Contains(String value) =>
            this.InternalContains(value.internalValue);

        public bool Contains(String value, StringComparison comparisonType) =>
            this.InternalContains(value.internalValue, comparisonType);


        /*
        Copy
        -------------------------------------------------------------------------------------------------------------*/
        public override LiteralString Copy() =>
            new LiteralString(this);
    }
}