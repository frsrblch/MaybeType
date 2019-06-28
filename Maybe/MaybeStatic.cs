using System;
using System.Collections.Generic;
using System.Text;

namespace MaybeType
{
    public static class Maybe
    {
        public static Maybe<T> None<T>() => new Maybe<T>();

        public static Maybe<T> Some<T>(T value) => new Maybe<T>(value);
    }
}
