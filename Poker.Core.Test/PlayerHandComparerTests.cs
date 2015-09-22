using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace Poker.Core.Test
{
    /// <summary>
    /// Tests for the PlayerHandComparer class.
    /// </summary>
    [TestClass]
    public class PlayerHandComparerTests
    {
        private PlayerHandComparer _target;
        protected PlayerHandComparer Target
        {
            get { return _target ?? (_target = new PlayerHandComparer()); }
            set { _target = value; }
        }

        private IPlayer FakePlayer1 { get; set; }
        private IPlayer FakePlayer2 { get; set; }


        /// <summary>
        /// Use TestInitialize to run code before running each test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            FakePlayer1 = Substitute.For<IPlayer>();
            FakePlayer2 = Substitute.For<IPlayer>();
        }

        /// <summary>
        /// Use TestCleanup to run code after each test has run.
        /// </summary>
        [TestCleanup]
        public void TestCleanup()
        {
            _target = null;
        }

        /// <summary>
        /// Test for the Compare when NotNull.
        /// </summary>
        [TestMethod]
        public void Compare_NotNull_ComparesThePlayersHands()
        {
            // ARRANGE
            FakePlayer1.Hand.CompareTo(FakePlayer2.Hand).Returns(1);

            // ACT
            var actual = Target.Compare(FakePlayer1, FakePlayer2);

            // ASSERT
            Assert.AreEqual(actual, 1);
        }
    }
}
