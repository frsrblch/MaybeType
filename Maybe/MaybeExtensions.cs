using System;
using System.Collections.Generic;
using System.Text;

namespace MaybeType
{
    public static class MaybeExtensions
    {
        public static Maybe<T> Some<T>(this T value) => new Maybe<T>(value);

        public static Maybe<T> None<T>(this T _) => new Maybe<T>();
    }
}
