using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MaybeType
{
    public readonly struct Maybe<T> : IEquatable<Maybe<T>>
    {
        private readonly T _value;

        public bool HasValue { get; }

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
            if (HasValue)
            {
                return _value.Equals(value);
            }
            else
            {
                return value == null;
            }
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

        public TOut Match<TOut>(Func<T, TOut> matchSome, Func<TOut> matchNone)
        {
            if (HasValue)
            {
                return matchSome(_value);
            }
            else
            {
                return matchNone();
            }
        }

        public void Match(Action<T> matchSome, Action matchNone)
        {
            if (HasValue)
            {
                matchSome(_value);
            }
            else
            {
                matchNone();
            }
        }

        public void MatchSome(Action<T> action)
        {
            if (HasValue)
            {
                action(_value);
            }
        }

        public void MatchNone(Action action)
        {
            if (!HasValue)
            {
                action();
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
