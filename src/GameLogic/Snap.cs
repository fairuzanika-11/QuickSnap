using System;
using SwinGameSDK;

namespace CardGames.GameLogic
{
    public class Snap
    {
        private Deck _deck;
        private Card[] _topCards = new Card[2];
        private int[] _score = new int[] { 0, 0 };
        private bool _started;

        private Timer _gameTimer;       // created in ctor
        private uint _flipTime = 1000;  // 1 second between flips

        public Snap()
        {
            _deck = new Deck();
            _gameTimer = SwinGame.CreateTimer(); // Task 37
        }

        public bool IsStarted => _started;
        public Card TopCard => _topCards[0];
        public int Score(int player) => _score[player];

        public void Start()
        {
            if (!_started)
            {
                _started = true;
                _deck.Shuffle();
                FlipNextCard();
                _gameTimer.Start(); // Task 38
            }
        }

        // <-- Task 39: timed auto-flip
        public void Update()
        {
            if (!_started) return;

            // When enough time has passed, flip next card and reset timer
            if (_gameTimer.Ticks > _flipTime)
            {
                _gameTimer.Reset();
                FlipNextCard();
            }
        }

        public void FlipNextCard()
        {
            _topCards[1] = _topCards[0];
            _topCards[0] = _deck.Draw();
        }

        public void PlayerHit(int player)
        {
            if (player >= 0 && player < _score.Length && _started)
            {
                if (_topCards[0] != null && _topCards[1] != null &&
                    _topCards[0].Rank == _topCards[1].Rank)
                    _score[player]++;
                else
                    _score[player]--;
            }

            _started = false;
            _gameTimer.Stop();
        }
    }
}
