using ResultType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OptionType
{
    public readonly struct Option<TValue> : IEquatable<Option<TValue>>
    {
        public bool IsSome { get; }

        public bool IsNone => !IsSome;

        private readonly TValue _value;

        internal Option(TValue value = default)
        {
            if (value == null)
            {
                _value = default;
                IsSome = false;
            }
            else
            {
                _value = value;
                IsSome = true;
            }
        }

        public static implicit operator Option<TValue>(TValue value)
        {
            return new Option<TValue>(value);
        }

        public bool Contains(TValue value)
        {
            return IsSome && _value.Equals(value);
        }

        public TOut Match<TOut>(Func<TValue, TOut> functionIfSome, Func<TOut> functionIfNone)
        {
            if (IsSome)
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
            if (IsSome)
            {
                actionIfSome(_value);
            }
            else
            {
                actionIfNone();
            }
        }

        public void AndThen(Action<TValue> actionIfSome)
        {
            Match(actionIfSome, () => { });
        }

        public Option<UValue> AndThen<UValue>(Func<TValue, Option<UValue>> function)
        {
            return Match(function, Option.None<UValue>);
        }

        public Option<UValue> AndThen<UValue>(Func<TValue, UValue> function)
        {
            return Match(value => function(value).Some(), Option.None<UValue>);
        }

        public void OrElse(Action actionIfNone)
        {
            Match(_ => { }, actionIfNone);
        }

        public Option<TValue> Or(Option<TValue> alternative)
        {
            return Match(
                value => value.Some(),
                () => alternative);
        }

        public Option<TValue> OrElse(Func<TValue> function)
        {
            return Match(
                value => value.Some(),
                () => function().Some());
        }

        public Option<TValue> OrElse(Func<Option<TValue>> function)
        {
            return Match(
                value => value.Some(),
                () => function());
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

        public Result<TValue, TError> OkayOr<TError>(TError error)
        {
            return Match(
                value => Result.Okay<TValue, TError>(value),
                () => Result.Error<TValue, TError>(error));
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            if (IsSome)
            {
                yield return _value;
            }
        }

        public IEnumerable<TValue> ToEnumerable()
        {
            if (IsSome)
            {
                yield return _value;
            }
        }

        public bool Equals(Option<TValue> other)
        {
            return Match(
                value => other.Contains(value),
                () => !other.IsSome);
        }

        public override bool Equals(object obj)
        {
            return obj is Option<TValue> other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 281;
                if (IsSome)
                {
                    hash = (hash * 251) + _value.GetHashCode();
                }
                return hash;
            }
        }

        public override string ToString()
        {
            return Match(
                value => $"Some({value})",
                () => "None");
        }
    }
}
