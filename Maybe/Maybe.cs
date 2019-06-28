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

        public bool Contains(T value)
        {
            return Match(some => some.Equals(value), () => value == null);
        }

        public T ValueOr(T other)
        {
            return Match(some => some, () => other);
        }

        public T ValueOr(Func<T> getOther)
        {
            return Match(some => some, getOther);
        }

        public void MatchSome(Action<T> action)
        {
            Match(action, () => { });
        }

        public void MatchNone(Action action)
        {
            Match(_ => { }, action);
        }

        public Maybe<TOut> Map<TOut>(Func<T, TOut> mapFunction)
        {
            return Match(some => mapFunction(some), () => Maybe.None<TOut>());
        }

        public Maybe<TOut> Map<TOut>(Func<T, Maybe<TOut>> mapFunction)
        {
            return Match(mapFunction, Maybe.None<TOut>);
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
            return SomeWhen(some => !predicate(some));
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
                yield return _value;
            }
        }

        public bool Equals(Maybe<T> other)
        {
            return Match(some => other.Contains(some), () => !other.HasValue);
        }

        public override string ToString()
        {
            return Match(
                value => $"Some({value.ToString()})",
                () => "None");
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
