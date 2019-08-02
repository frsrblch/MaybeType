using Option;
using System;
using System.Collections.Generic;
using Xunit;

namespace MaybeTests
{
    public class MaybeTests
    {
        [Fact]
        public void GivenNull_ReturnsEmpty()
        {
            Option<object> none = Option.Option.Some<object>(null);

            Assert.False(none.HasValue);
        }
        [Fact]
        public void Default_IsEmpty()
        {
            Option<object> none = default;

            Assert.False(none.HasValue);
        }

        [Fact]
        public void GivenObject_ReturnsWithValue()
        {
            object value = new object();

            Option<object> some = Option.Option.Some(value);

            Assert.True(some.HasValue);
            Assert.True(some.Contains(value));
        }

        [Fact]
        public void GivenNothing_ReturnsEmpty()
        {
            Option<object> none = new Option<object>();

            Assert.False(none.HasValue);
        }

        [Fact]
        public void ImplicitCast_GivenObject_ReturnsWithValue()
        {
            object value = new object();
            Option<object> some = value;

            Assert.True(some.HasValue);
            Assert.True(some.Contains(value));
        }

        [Fact]
        public void ImplicitCast_GivenNull_ReturnsEmpty()
        {
            Option<object> none = null;

            Assert.False(none.HasValue);
        }

        [Fact]
        public void Contains_NoneContainsNull_ReturnsTrue()
        {
            Option<object> none = null;

            Assert.False(none.Contains(null));
        }

        [Fact]
        public void Contains_SomeContainsNull_ReturnsFalse()
        {
            Option<object> some = new object();

            Assert.False(some.Contains(null));
        }

        [Fact]
        public void Contains_NoneContainsValue_ReturnsFalse()
        {
            Option<object> none = null;

            Assert.False(none.Contains(new object()));
        }

        [Fact]
        public void Contains_SomeContainsValue_ReturnsFalse()
        {
            object value = new object();
            Option<object> some = value;

            Assert.True(some.Contains(value));
        }

        [Fact]
        public void Contains_SomeContainsOtherValue_ReturnsFalse()
        {
            object value = new object();
            object otherValue = new object();
            Option<object> some = value;

            Assert.False(some.Contains(otherValue));
        }

        [Fact]
        public void Maybe_None_ReturnsEmpty()
        {
            Option<object> none = Option.Option.None<object>();

            Assert.False(none.HasValue);
        }

        [Fact]
        public void SomeExtension_GivenValue_ReturnsWithValue()
        {
            object value = new object();
            Option<object> some = value.Some();

            Assert.True(some.HasValue);
            Assert.True(some.Contains(value));
        }

        [Fact]
        public void SomeExtension_GivenNull_ReturnsEmpty()
        {
            object noValue = null;
            Option<object> none = noValue.Some();

            Assert.False(none.HasValue);
        }

        [Fact]
        public void NoneExtension_ReturnsEmpty()
        {
            object value = new object();
            Option<object> none = value.None();

            Assert.False(none.HasValue);
            Assert.False(none.Contains(value));
        }

        [Fact]
        public void Foreach_None_ReturnsEmptyEnumerable()
        {
            Option<object> none = Option.Option.None<object>();

            int count = 0;
            foreach (var item in none)
            {
                count++;
            }
            Assert.Equal(0, count);
        }

        [Fact]
        public void ForeachSome_ReturnsValueAsEnumerable()
        {
            object value = new object();
            Option<object> some = value;

            int count = 0;
            foreach (var item in some)
            {
                Assert.Equal(item, value);
                count++;
            }
            Assert.Equal(1, count);
        }

        [Fact]
        public void ValueOr_FromSome_ReturnsInnerValue()
        {
            Option<int> some = 2;

            Assert.Equal(2, some.ValueOr(3));
        }

        [Fact]
        public void ValueOr_FromNone_ReturnsOrValue()
        {
            Option<int> none = Option.Option.None<int>();

            Assert.Equal(3, none.ValueOr(3));
        }

        [Fact]
        public void ValueOrFunc_FromSome_DoesNotCallFunc()
        {
            bool functionCalled = false;
            int getThree()
            {
                functionCalled = true;
                return 3;
            }
            Option<int> some = 2;

            Assert.Equal(2, some.ValueOr(getThree));
            Assert.False(functionCalled);
        }

        [Fact]
        public void ValueOrFunc_FromNone_ReturnsResultFromFunc()
        {
            int getThree() => 3;
            Option<int> none = Option.Option.None<int>();

            Assert.Equal(3, none.ValueOr(getThree));
        }

        [Fact]
        public void MatchFunc_Some_ReturnMatchSome()
        {
            Option<int> some = 2;

            int result = some.Match(i => i * 3, () => 0);

            Assert.Equal(6, result);
        }

        [Fact]
        public void MatchFunc_None_ReturnMatchNone()
        {
            Option<int> none = Option.Option.None<int>();

            int result = none.Match(i => i * 3, () => 0);

            Assert.Equal(0, result);
        }

        [Fact]
        public void MatchAction_Some_ReturnMatchSome()
        {
            bool someExecuted = false;
            bool noneExecuted = false;

            void someAction(int _) => someExecuted = true;
            void noneAction() => noneExecuted = true;

            Option<int> some = 2;

            some.Match(someAction, noneAction);

            Assert.True(someExecuted);
            Assert.False(noneExecuted);
        }

        [Fact]
        public void MatchAction_None_ReturnMatchNone()
        {
            bool someExecuted = false;
            bool noneExecuted = false;

            void someAction(int _) => someExecuted = true;
            void noneAction() => noneExecuted = true;

            Option<int> none = Option.Option.None<int>();

            none.Match(someAction, noneAction);

            Assert.False(someExecuted);
            Assert.True(noneExecuted);
        }

        [Fact]
        public void MatchSome_GivenSome_PerformsAction()
        {
            bool actionPerformed = false;
            void someAction(int _) => actionPerformed = true;
            Option<int> some = 2;

            some.MatchSome(someAction);

            Assert.True(actionPerformed);
        }

        [Fact]
        public void MatchSome_GivenNone_DoesNotPerformAction()
        {
            bool actionPerformed = false;
            void someAction(int _) => actionPerformed = true;
            Option<int> none = Option.Option.None<int>();

            none.MatchSome(someAction);

            Assert.False(actionPerformed);
        }

        [Fact]
        public void MatchNone_GivenSome_DoesNotPerformAction()
        {
            bool actionPerformed = false;
            void noneAction() => actionPerformed = true;
            Option<int> some = 2;

            some.MatchNone(noneAction);

            Assert.False(actionPerformed);
        }

        [Fact]
        public void MatchNone_GivenNone_DoesNotPerformAction()
        {
            bool actionPerformed = false;
            void noneAction() => actionPerformed = true;
            Option<int> none = Option.Option.None<int>();

            none.MatchNone(noneAction);

            Assert.True(actionPerformed);
        }

        [Fact]
        public void Map_Some_ReturnsTransformedValue()
        {
            Option<int> some = 2;
            Option<int> somePlusTwo = some.Map(x => x + 2);

            Assert.True(somePlusTwo.Contains(4));
        }

        [Fact]
        public void Map_None_ReturnsEmpty()
        {
            Option<int> none = Option.Option.None<int>();

            Option<int> stillNone = none.Map(x => x + 2);

            Assert.False(stillNone.HasValue);
        }

        [Fact]
        public void MapMaybe_Some_ReturnsFlatMaybe()
        {
            Option<int> someIfEven(int x)
            {
                if (x % 2 == 0)
                {
                    return x;
                }
                return x.None();
            }

            Option<int> some = 2;
            Option<int> stillSome = some.Map(someIfEven);

            Assert.True(stillSome.Contains(2));
        }

        [Fact]
        public void MapMaybe_None_ReturnsFlatMaybe()
        {
            Option<int> someIfEven(int x)
            {
                if (x % 2 == 0)
                {
                    return x;
                }
                return x.None();
            }

            Option<int> some = 3;
            Option<int> none = some.Map(someIfEven);

            Assert.False(none.HasValue);
        }

        [Fact]
        public void ToEnumerable_Some_ReturnsSingleValue()
        {
            object value = new object();
            Option<object> some = value;

            Assert.Single(some.ToEnumerable(), value);
        }

        [Fact]
        public void ToEnumerable_None_ReturnsEmpty()
        {
            Option<object> none = null;

            Assert.Empty(none.ToEnumerable());
        }

        [Theory]
        [InlineData("1", "1", true)]
        [InlineData("1", "2", false)]
        [InlineData(null, null, true)]
        [InlineData("1", null, false)]
        [InlineData(null, "1", false)]
        public void EqualsMaybeTests(string item1, string item2, bool expected)
        {
            Option<string> maybe1 = Option.Option.Some(item1);
            Option<string> maybe2 = Option.Option.Some(item2);

            Assert.Equal(expected, maybe1.Equals(maybe2));
        }

        [Theory]
        [InlineData("1", "1", true)]
        [InlineData("1", "2", false)]
        [InlineData(null, null, true)]
        [InlineData("1", null, false)]
        [InlineData(null, "1", false)]
        public void EqualsTests(string item1, string item2, bool expected)
        {
            Option<string> maybe1 = Option.Option.Some(item1);

            Assert.Equal(expected, maybe1.Equals(item2));
        }

        [Fact]
        public void SomeWhen_SomeMatchesPredicate_ReturnsSome()
        {
            Predicate<int> isEven = i => i % 2 == 0;
            Option<int> someEven = 2;

            Assert.True(someEven.Filter(isEven).Contains(2));
        }

        [Fact]
        public void SomeWhen_SomeDoesNotMatchPredicate_ReturnsNone()
        {
            Predicate<int> isEven = i => i % 2 == 0;
            Option<int> someOdd = 3;

            Assert.False(someOdd.Filter(isEven).HasValue);
        }

        [Fact]
        public void SomeWhen_None_ReturnsNone()
        {
            Predicate<int> isEven = i => i % 2 == 0;
            Option<int> none = Option.Option.None<int>();

            Assert.False(none.Filter(isEven).HasValue);
        }

        [Fact]
        public void ToString_GivenNone_ReturnsNone()
        {
            Option<string> none = Option.Option.None<string>();

            Assert.Equal("None", none.ToString());
        }

        [Fact]
        public void ToString_GivenSome_ReturnsSomeAndValue()
        {
            Option<string> some = Option.Option.Some("ABC");

            Assert.Equal("Some(ABC)", some.ToString());
        }
    }
}
