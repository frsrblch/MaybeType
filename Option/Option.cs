using System;
using System.Collections.Generic;
using System.Linq;

namespace Option
{
    public readonly struct Option<TValue> : IEquatable<Option<TValue>>
    {
        public bool HasValue { get; }

        private readonly TValue _value;

        internal Option(TValue value = default)
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

        public static implicit operator Option<TValue>(TValue value)
        {
            return new Option<TValue>(value);
        }

        public bool Contains(TValue value)
        {
            return HasValue && _value.Equals(value);
        }

        public TOut Match<TOut>(Func<TValue, TOut> functionIfSome, Func<TOut> functionIfNone)
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

        public void Match(Action<TValue> actionIfSome, Action actionIfNone)
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

        public void MatchSome(Action<TValue> actionIfSome)
        {
            Match(actionIfSome, () => { });
        }

        public void MatchNone(Action actionIfNone)
        {
            Match(_ => { }, actionIfNone);
        }

        public Option<TOut> Map<TOut>(Func<TValue, TOut> mapFunction)
        {
            return Match(some => mapFunction(some), Option.None<TOut>);
        }

        public Option<TOut> Map<TOut>(Func<TValue, Option<TOut>> mapFunction)
        {
            return Match(
                mapFunction,
                Option.None<TOut>);
        }

        public Option<TValue> Filter(Predicate<TValue> predicate)
        {
            return Match(
                value => predicate(value) ? value : value.None(),
                Option.None<TValue>);
        }

        public TValue ValueOr(TValue other)
        {
            return Match(
                value => value,
                () => other);
        }

        public TValue ValueOr(Func<TValue> getOther)
        {
            return Match(
                value => value,
                getOther);
        }

        public TValue ValueOrThrow(string message = null)
        {
            return Match(
                value => value,
                () => throw new InvalidOperationException(message));
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            if (HasValue)
            {
                yield return _value;
            }
        }

        public IEnumerable<TValue> ToEnumerable()
        {
            if (HasValue)
            {
                yield return _value;
            }
        }

        public bool Equals(Option<TValue> other)
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
