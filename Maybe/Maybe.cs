using System;
using System.Collections.Generic;
using System.Linq;

namespace MaybeType
{
    public readonly struct Maybe<T> : IEquatable<Maybe<T>>
    {
        public bool HasValue { get; }

        private readonly T _value;

        internal Maybe(T value = default)
        {
            if (value == null)
            {
                _value = default;
                HasValue = false;
            }
            else
            {
                _value = value;
                HasValue = true;
            }
        }

        public static implicit operator Maybe<T>(T value)
        {
            return new Maybe<T>(value);
        }

        public bool Contains(T value)
        {
            return HasValue && _value.Equals(value);
        }

        public T ValueOr(T other)
        {
            if (HasValue)
            {
                return _value;
            }

            return other;
        }

        public T ValueOr(Func<T> getOther)
        {
            if (HasValue)
            {
                return _value;
            }

            return getOther();
        }

        public TOut Match<TOut>(Func<T, TOut> functionIfSome, Func<TOut> functionIfNone)
        {
            if (HasValue)
            {
                return functionIfSome(_value);
            }
            else
            {
                return functionIfNone();
            }
        }

        public void Match(Action<T> actionIfSome, Action actionIfNone)
        {
            if (HasValue)
            {
                actionIfSome(_value);
            }
            else
            {
                actionIfNone();
            }
        }

        public void MatchSome(Action<T> actionIfSome)
        {
            if (HasValue)
            {
                actionIfSome(_value);
            }
        }

        public void MatchNone(Action actionIfNone)
        {
            if (!HasValue)
            {
                actionIfNone();
            }
        }

        public Maybe<TOut> Map<TOut>(Func<T, TOut> mapFunction)
        {
            if (HasValue)
            {
                return mapFunction(_value);
            }
            else
            {
                return Maybe.None<TOut>();
            }
        }

        public Maybe<TOut> Map<TOut>(Func<T, Maybe<TOut>> mapFunction)
        {
            if (HasValue)
            {
                return mapFunction(_value);
            }
            else
            {
                return Maybe.None<TOut>();
            }
        }

        public Maybe<T> SomeWhen(Predicate<T> predicate)
        {
            if (HasValue && predicate(_value))
            {
                return this;
            }

            return Maybe.None<T>();
        }

        public Maybe<T> NoneWhen(Predicate<T> predicate)
        {
            if (HasValue && !predicate(_value))
            {
                return this;
            }

            return Maybe.None<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (HasValue)
            {
                yield return _value;
            }
        }

        public IEnumerable<T> ToEnumerable()
        {
            if (HasValue)
            {
                return new T[] { _value };
            }
            return Enumerable.Empty<T>();
        }

        public bool Equals(Maybe<T> other)
        {
            if (HasValue && other.HasValue)
            {
                return _value.Equals(other._value);
            }

            if (!HasValue && !other.HasValue)
            {
                return true;
            }

            return false;
        }
    }

    public static class Maybe
    {
        public static Maybe<T> None<T>() => new Maybe<T>();

        public static Maybe<T> Some<T>(T value) => new Maybe<T>(value);
    }

    public static class MaybeExtensions
    {
        public static Maybe<T> Some<T>(this T value) => new Maybe<T>(value);

        public static Maybe<T> None<T>(this T _) => new Maybe<T>();
    }
}
