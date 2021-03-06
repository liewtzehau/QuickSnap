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
            cards = SwinGame.LoadBitmapNamed ("Cards", "Cards.png");
            SwinGame.BitmapSetCellDetails (cards, 167, 250, 13, 5, 53);      // set the cells in the bitmap to match the cards
        }

		/// <summary>
		/// Respond to the user input -- with requests affecting myGame
		/// </summary>
		/// <param name="myGame">The game object to update in response to events.</param>
		private static void HandleUserInput(Snap myGame)
		{
			//Fetch the next batch of UI interaction
			SwinGame.ProcessEvents();

			if (SwinGame.KeyTyped (KeyCode.vk_SPACE))
			{
				myGame.Start ();
			}

			if (myGame.IsStarted)
			{
				if ( SwinGame.KeyTyped (KeyCode.vk_LSHIFT) &&
					SwinGame.KeyTyped (KeyCode.vk_RSHIFT))
				{
					//TODO: add sound effects
					SwinGame.LoadSoundEffectNamed ("Slap1", "slap-loud.wav");
					SwinGame.PlaySoundEffect ("Slap1");
				}
				else if (SwinGame.KeyTyped (KeyCode.vk_LSHIFT))
				{
					myGame.PlayerHit (0);
					SwinGame.LoadSoundEffectNamed ("Slap2", "slap.wav");
					SwinGame.PlaySoundEffect ("Slap2");
				}
				else if (SwinGame.KeyTyped (KeyCode.vk_RSHIFT))
				{
					myGame.PlayerHit (1);
					SwinGame.LoadSoundEffectNamed ("Slap3", "slap2.wav");
					SwinGame.PlaySoundEffect ("Slap3");
				}
			}
		}
		public void Shuffle()
		{
			for (int i=0; i<52; i++)
			{
				if (_cards[i].FaceUp) _cards[i].TurnOver();
			}
			Random rnd = new Random();
			
			for(int i= 0; i<52-1; i++)
			{
				int rndIdx = rnd.Next(52-i);
				
				Card temp = _cards[i];
				_cards[i] = _cards[i + rndIdx];
				_cards[i + rndIdx] = temp;
			}
			_topCard =0;
		}
		/// <summary>
		/// Draws the game to the Window.
		/// </summary>
		/// <param name="myGame">The details of the game -- mostly top card and scores.</param>
		private static void DrawGame(Snap myGame)
		{
			SwinGame.DrawBitmap("cardsBoard.png", 0, 0);
			SwinGame.LoadFontNamed ("GameFont", "Chunkfive.otf", 24);

			// Draw the top card
			Card top = myGame.TopCard;
			if (top != null)
			{
				SwinGame.DrawText ("Top Card is " + top.ToString (), Color.White, "GameFont", 0, 20);
				SwinGame.DrawText ("Player 1 score: " + myGame.Score(0), Color.White, "GameFont", 0, 30);
				SwinGame.DrawText ("Player 2 score: " + myGame.Score(1), Color.White, "GameFont", 0, 40);
				SwinGame.DrawCell (SwinGame.BitmapNamed ("Cards"), top.CardIndex, 521, 153);
			}
			else
			{
				SwinGame.DrawText ("No card played yet...", Color.RoyalBlue, 0, 20);
			}

			// Draw the back of the cards... to represent the deck
			SwinGame.DrawCell (SwinGame.BitmapNamed ("Cards"), 52, 155, 153);

			//Draw onto the screen
			SwinGame.RefreshScreen(60);
		}

		/// <summary>
		/// Updates the game -- it should flip the cards itself once started!
		/// </summary>
		/// <param name="myGame">The game to be updated...</param>
		private static void UpdateGame(Snap myGame)
		{
			myGame.Update(); // just ask the game to do this...
		}

        public static void Main()
        {
            //Open the game window
            SwinGame.OpenGraphicsWindow("Snap!", 860, 500);

			//Load the card images and set their cell details
            LoadResources();
            
			// Create the game!
			Snap myGame = new Snap ();

            //Run the game loop
            while(false == SwinGame.WindowCloseRequested())
            {
				HandleUserInput (myGame);
				DrawGame (myGame);
				UpdateGame (myGame);
            }
        }
    }
}
