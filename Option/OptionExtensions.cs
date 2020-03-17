using System;
using System.Collections.Generic;
using System.Text;

namespace OptionType
{
    public static class MaybeExtensions
    {
        public static Option<T> Some<T>(this T value) => new Option<T>(value);

        public static Option<T> None<T>(this T _) => new Option<T>();
    }
}
