using System;
using SwinGameSDK;

#if DEBUG
using NUnit.Framework;
#endif

namespace CardGames.GameLogic
{
    /// <summary>
    /// The Snap card game in which the user scores a point if they
    /// hit when the rank of the last two cards match.
    /// </summary>
    public class Snap
    {
        // Keep only the last two cards...
        private readonly Card[] _topCards = new Card[2];

        // Have a Deck of cards to play with.
        private readonly Deck _deck;

        // Use a timer to allow the game to draw cards at timed intervals (reserved for future use)
        private readonly Timer _gameTimer;

        // The amount of time that must pass before a card is flipped
        private int _flipTime = 1000;

        // the score for the 2 players
        private readonly int[] _score = new int[2];

        // game started flag
        private bool _started = false;

        /// <summary>
        /// Create a new game of Snap!
        /// </summary>
        public Snap()
        {
            _deck = new Deck();
            _gameTimer = new Timer();
        }

        /// <summary>
        /// Gets the card on the top of the "flip" stack. This card will be face up.
        /// </summary>
        public Card TopCard => _topCards[1];

        /// <summary>
        /// Indicates if there are cards remaining in the Deck.
        /// </summary>
        public bool CardsRemain => _deck.CardsRemaining > 0;

        /// <summary>
        /// Milliseconds before a new card is drawn and placed on top.
        /// </summary>
        public int FlipTime
        {
            get => _flipTime;
            set => _flipTime = value;
        }

        /// <summary>
        /// True if the game has started.
        /// </summary>
        public bool IsStarted => _started;

        /// <summary>
        /// Start the Snap game playing!
        /// </summary>
        public void Start()
        {
            if (!IsStarted)
            {
                _started = true;
                _deck.Shuffle();

                // reset last two cards and flip the first
                _topCards[0] = null;
                _topCards[1] = null;
                FlipNextCard();
            }
        }

        /// <summary>
        /// Flip the next card from the deck onto the top of the pile.
        /// </summary>
        public void FlipNextCard()
        {
            if (_deck.CardsRemaining > 0)
            {
                _topCards[0] = _topCards[1];      // shift previous top to index 0
                _topCards[1] = _deck.Draw();      // draw new top
                _topCards[1].TurnOver();          // reveal card
            }
        }

        /// <summary>
        /// Update the game’s internal state (reserved for timed flipping).
        /// </summary>
        public void Update()
        {
            // TODO: implement automatic timed flipping using _gameTimer and _flipTime if desired.
        }

        /// <summary>
        /// Gets the player's score by index (0 or 1).
        /// </summary>
        public int Score(int idx)
        {
            if (idx >= 0 && idx < _score.Length) return _score[idx];
            return 0;
        }

        /// <summary>
        /// The player hit the top of the cards "snap"! :)
        /// If the last two cards have the same Rank: +1, else -1.
        /// Game stops after any hit.
        /// </summary>
        public void PlayerHit(int player)
        {
            if (player >= 0 && player < _score.Length)
            {
                bool validMatch =
                    IsStarted &&
                    _topCards[0] != null &&
                    _topCards[1] != null &&
                    _topCards[0].Rank == _topCards[1].Rank;

                if (validMatch)
                {
                    // Correct snap
                    _score[player]++;
                    // TODO: play a "correct" sound
                }
                else
                {
                    // Miss-hit (or game not in a valid state) -> deduct
                    _score[player]--;
                    // TODO: play a "wrong" sound
                }
            }

            // Stop the round/game after a hit
            _started = false;
        }

        #region Snap Game Unit Tests
#if DEBUG
        public class SnapTests
        {
            [Test]
            public void TestSnapCreation()
            {
                var s = new Snap();

                Assert.IsTrue(s.CardsRemain);
                Assert.IsNull(s.TopCard);
            }

            [Test]
            public void TestFlipNextCard()
            {
                var s = new Snap();

                Assert.IsTrue(s.CardsRemain);
                Assert.IsNull(s.TopCard);

                s.FlipNextCard();

                Assert.IsNull(s._topCards[0]);
                Assert.IsNotNull(s._topCards[1]);
            }
        }
#endif
        #endregion
    }
}
