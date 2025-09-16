using System;
using SwinGameSDK;
using CardGames.GameLogic;

namespace CardGames
{
    public class SnapGame
    {
        public static void LoadResources()
        {
            Bitmap cards;
            // Load card spritesheet
            cards = SwinGame.LoadBitmapNamed("Cards", "Cards.png");
            SwinGame.BitmapSetCellDetails(cards, 82, 110, 13, 5, 53);

            // Load font
            SwinGame.LoadFontNamed("GameFont", "ChunkFive-Regular.otf", 12);

            // Load background image (make sure "Background.png" exists in Resources folder)
            SwinGame.LoadBitmapNamed("Background", "Background.png");
        }

        private static void HandleUserInput(Snap myGame)
        {
            SwinGame.ProcessEvents();

            // Step 41: Spacebar starts the game (with timer-based flips)
            if (SwinGame.KeyTyped(KeyCode.vk_SPACE))
            {
                myGame.Start();
            }

            // Step 42: Player 1 and Player 2 hit keys
            if (SwinGame.KeyTyped(KeyCode.vk_p))
            {
                myGame.PlayerHit(0); // Player 1
            }
            if (SwinGame.KeyTyped(KeyCode.vk_q))
            {
                myGame.PlayerHit(1); // Player 2
            }
        }

        private static void DrawGame(Snap myGame)
        {
            // Clear screen
            SwinGame.ClearScreen(Color.White);

            // --- Step 64: Draw background first ---
            SwinGame.DrawBitmap(SwinGame.BitmapNamed("Background"), 0, 0);

            // --- Draw cards + text on top of background ---
            Card top = myGame.TopCard;
            if (top != null)
            {
                SwinGame.DrawText("Top Card is " + top.ToString(), Color.RoyalBlue, "GameFont", 20, 20);
                SwinGame.DrawText("Player 1 score: " + myGame.Score(0), Color.RoyalBlue, "GameFont", 20, 40);
                SwinGame.DrawText("Player 2 score: " + myGame.Score(1), Color.RoyalBlue, "GameFont", 20, 60);

                // Draw top card over the background (centered nicely)
                SwinGame.DrawCell(SwinGame.BitmapNamed("Cards"), top.CardIndex, 521, 153);
            }
            else
            {
                SwinGame.DrawText("No card played yet...", Color.RoyalBlue, "GameFont", 20, 20);
            }

            // Draw the back of the deck (slightly left of top card)
            SwinGame.DrawCell(SwinGame.BitmapNamed("Cards"), 52, 155, 153);

            // Refresh screen at 60 fps
            SwinGame.RefreshScreen(60);
        }

        private static void UpdateGame(Snap myGame)
        {
            myGame.Update();
        }

        public static void Main()
        {
            SwinGame.OpenGraphicsWindow("Snap!", 860, 500);
            LoadResources();

            Snap myGame = new Snap();

            while (false == SwinGame.WindowCloseRequested())
            {
                HandleUserInput(myGame);
                DrawGame(myGame);
                UpdateGame(myGame);
            }
        }
    }
}
