using System;
using System.Collections.Generic;
using System.Linq;

namespace Option
{
    public readonly struct Option<T> : IEquatable<Option<T>>
    {
        public bool HasValue { get; }

        private readonly T _value;

        internal Option(T value = default)
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

        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }

        public bool Contains(T value)
        {
            return HasValue && _value.Equals(value);
        }

        public T ValueOr(T other)
        {
            return Match(value => value, () => other);
        }

        public T ValueOr(Func<T> getOther)
        {
            return Match(value => value, getOther);
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
            Match(actionIfSome, () => { });
        }

        public void MatchNone(Action actionIfNone)
        {
            Match(_ => { }, actionIfNone);
        }

        public Option<TOut> Map<TOut>(Func<T, TOut> mapFunction)
        {
            return Match(some => mapFunction(some), Maybe.None<TOut>);
        }

        public Option<TOut> Map<TOut>(Func<T, Option<TOut>> mapFunction)
        {
            return Match(mapFunction, Maybe.None<TOut>);
        }

        public Option<T> SomeWhen(Predicate<T> predicate)
        {
            if (HasValue && predicate(_value))
            {
                return this;
            }

            return Maybe.None<T>();
        }

        public Option<T> NoneWhen(Predicate<T> predicate)
        {
            return SomeWhen(value => !predicate(value));
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

        public bool Equals(Option<T> other)
        {
            return Match(
                value => other.Contains(value),
                () => !other.HasValue);
        }

        public override string ToString()
        {
            return Match(
                value => $"Some({value})",
                () => "None");
        }
    }
}
