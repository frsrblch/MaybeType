using System;
using System.Collections.Generic;
using System.Text;

namespace OptionType
{
    public static class Option
    {
        public static Option<T> None<T>() => new Option<T>();

        public static Option<T> Some<T>(T value) => new Option<T>(value);
    }
}
