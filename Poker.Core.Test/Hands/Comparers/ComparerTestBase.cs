using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Poker.Core.Deck;
using Poker.Core.Hands;
using Poker.Core.Hands.Comparers;

namespace Poker.Core.Test.Hands.Comparers
{
    [TestClass]
    public abstract class ComparerTestBase<T> where T : IHandComparer
    {
        protected abstract T Target { get; set; }

        protected abstract Core.Hands.Hands Type { get; }

        protected virtual int MinCount => 5;

        /// <summary>
        /// Test for the Compare when CountLessThanExpected.
        /// </summary>
        [TestMethod]
        public void Compare_CountLessThanExpected_ReturnsSentinal()
        {
            // ARRANGE
            
            // ACT
            var actual = Target.Compare(Substitute.For<IHand>());

            // ASSERT
            Assert.AreEqual(0, (int)actual.Item1);
            Assert.AreEqual(0, (int)actual.Item2);
        }

        /// <summary>
        /// Test for the CompareHandHand when CountsNotEqual.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public virtual void CompareHandHand_CountsNotEqual_Throws()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Count.Returns(2);
            var hand2 = Substitute.For<IHand>();
            hand2.Count.Returns(5);

            // ACT
            Target.Compare(hand1, hand2);

            // ASSERT
            Assert.Fail("Did not throw.");
        }

        /// <summary>
        /// Test for the CompareHandHand when TypesNotEqual.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public virtual void CompareHandHand_TypesNotEqual_Throws()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns((Core.Hands.Hands)((int)(Type + 3) % 9));
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns((Core.Hands.Hands)((int)(Type + 3) % 9));

            // ACT
            Target.Compare(hand1, hand2);

            // ASSERT
            Assert.Fail("Did not throw.");
        }

        /// <summary>
        /// Test for the CompareHandHand when Scenario.
        /// </summary>
        [TestMethod]
        public virtual void CompareHandHand_XHandNotEqualToType_YHandIsEqualType_Returns1()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns((Core.Hands.Hands)((int)(Type + 3) % 9));
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns(Type);

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(1, actual);
        }

        /// <summary>
        /// Test for the CompareHandHand when XHandIsEqualType.
        /// </summary>
        [TestMethod]
        public virtual void CompareHandHand_XHandIsEqualType_YHandNotEqualToType_ReturnsNegativeOne()
        {
            // ARRANGE
            var hand1 = Substitute.For<IHand>();
            hand1.Type.Returns(Type);
            var hand2 = Substitute.For<IHand>();
            hand2.Type.Returns((Core.Hands.Hands)((int)(Type + 3) % 9));

            // ACT
            var actual = Target.Compare(hand1, hand2);

            // ASSERT
            Assert.AreEqual(-1, actual);
        }

        /// <summary>
        /// Helper method to build a fake hand.
        /// </summary>
        protected IHand GetFakeHand(params Card[] cards)
        {
            var fakeCards = new List<Card>(cards);

            var fakeHand = Substitute.For<IHand>();
            fakeHand.Count.Returns(fakeCards.Count);
            fakeHand.Type.Returns(Type);
            fakeHand.HighCard.Returns(cards.Length == 0 ? Rank.Two : cards[0].Rank);
            fakeHand.GetEnumerator().Returns(fakeCards.GetEnumerator());
            fakeHand[Arg.Any<int>()].Returns(param => fakeCards[param.Arg<int>()]);
            return fakeHand;
        }
    }
}