namespace PorExtenso
{
    static class ExtensionMethods
    {
        public static bool IsEmpty(this string source) => string.IsNullOrEmpty(source);
        public static bool IsNotEmpty(this string source) => !string.IsNullOrEmpty(source);
    }
}
