using System.Collections;

namespace EchoLib.Classes
{
    public abstract class String : 
        IEnumerable<char>, IEquatable<String>, IEquatable<string>, IComparable<String>, IComparable<string>
    {
        internal readonly string value;
        internal readonly string internalValue;

        public readonly int Length;

        public char this[int index] =>
            this.internalValue[index];

        public char this[Index index] =>
            this.internalValue[index];


        /*=============================================================================================================
        Constructors
        =============================================================================================================*/
        protected String(string value, string internalValue)
        {
            this.value = value;
            this.internalValue = internalValue;
            this.Length = this.internalValue.Length;
        }


        /*=============================================================================================================
        Implicit Conversions
        =============================================================================================================*/

        // String to string
        public static implicit operator string(String self) =>
            self.internalValue;


        /*=============================================================================================================
        Operators
        ===============================================================================================================
        ==
        -------------------------------------------------------------------------------------------------------------*/
        public static bool operator ==(String a, String b) =>
            (a.internalValue == b.internalValue);

        public static bool operator ==(String a, string b) =>
            (a.internalValue == b);

        public static bool operator ==(string a, String b) =>
            (a == b.internalValue);

        /*
        !=
        -------------------------------------------------------------------------------------------------------------*/
        public static bool operator !=(String a, String b) =>
            (!(a == b));

        public static bool operator !=(String a, string b) =>
            (!(a == b));

        public static bool operator !=(string a, String b) =>
            (!(a == b));


        /*=============================================================================================================
        Override Methods
        =============================================================================================================*/
        public override int GetHashCode() =>
            this.value.GetHashCode();

        public override string ToString() =>
            this.value;


        /*=============================================================================================================
        Methods
        ===============================================================================================================
        ToCSharpString
        -------------------------------------------------------------------------------------------------------------*/
        public string ToCSharpString() =>
            this.internalValue;


        /*=============================================================================================================
        Existing string Methods
        ===============================================================================================================
        Clone
        -------------------------------------------------------------------------------------------------------------*/
        public abstract object Clone();


        /*
        CompareTo
        -------------------------------------------------------------------------------------------------------------*/

        // Internal
        protected int InternalCompareTo(object? value) =>
            this.internalValue.CompareTo(value);

        protected int InternalCompareTo(string? strB) =>
            this.internalValue.CompareTo(strB);


        // External
        public int CompareTo(object? value) =>
            this.InternalCompareTo(value);

        public int CompareTo(string? strB) =>
            this.InternalCompareTo(strB);

        public int CompareTo(String? strB) =>
            this.InternalCompareTo(strB?.internalValue);

        /*
        Copy
        -------------------------------------------------------------------------------------------------------------*/
        public abstract String Copy();

        /*
        Equals
        -------------------------------------------------------------------------------------------------------------*/

        // Internal
        protected bool InternalEquals(object? obj) =>
            this.internalValue.Equals(obj);

        protected bool InternalEquals(string? value) =>
            this.internalValue.Equals(value);

        protected bool InternalEquals(string? value, StringComparison comparisonType) =>
            this.internalValue.Equals(value, comparisonType);

        protected static bool InternalEquals(string? a, string? b) =>
            string.Equals(a, b);

        protected static bool InternalEquals(string? a, string? b, StringComparison comparisonType) =>
            string.Equals(a, b, comparisonType);

        // External
        public override bool Equals(object? obj) =>
            this.InternalEquals(obj);

        public bool Equals(string? value) =>
            this.InternalEquals(value);

        public bool Equals(string? value, StringComparison comparisonType) =>
            this.InternalEquals(value, comparisonType);

        public bool Equals(String? value) =>
            this.InternalEquals(value?.internalValue);

        public bool Equals(String? value, StringComparison comparisonType) =>
            this.InternalEquals(value?.internalValue, comparisonType);

        public static bool Equals(string? a, string? b) =>
            String.InternalEquals(a, b);

        public static bool Equals(string? a, string? b, StringComparison comparisonType) =>
            String.InternalEquals(a, b,  comparisonType);

        public static bool Equals(string? a, String? b) =>
            String.InternalEquals(a, b?.internalValue);

        public static bool Equals(string? a, String? b, StringComparison comparisonType) =>
            String.InternalEquals(a, b?.internalValue, comparisonType);

        public static bool Equals(String? a, string? b) =>
            String.InternalEquals(a?.internalValue, b);

        public static bool Equals(String? a, string? b, StringComparison comparisonType) =>
            String.InternalEquals(a?.internalValue, b, comparisonType);

        public static bool Equals(String? a, String? b) =>
            String.InternalEquals(a?.internalValue, b?.internalValue);

        public static bool Equals(String? a, String? b, StringComparison comparisonType) =>
            String.InternalEquals(a?.internalValue, b?.internalValue, comparisonType);

        /*
        GetEnumerator
        -------------------------------------------------------------------------------------------------------------*/
        IEnumerator<char> IEnumerable<char>.GetEnumerator() =>
            this.internalValue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            this.internalValue.GetEnumerator();
    }
}
