using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame.GameStates
{
    public class GameOver : GameState
    {
        /// <summary>
        /// Runs every time this game state is started up
        /// </summary>
        public override void Start()
        {
            // Do nothing
        }

        public override State? StateTransition()
        {
            if (UserInputHandler.IsFirstPress(Controls.Back))
                return State.Hub;

            return base.StateTransition();
        }

        /// <summary>
        /// Runs every frame, handles all drawing
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used across the whole game</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.arial16b, // Spritefont
                                   "You lost",              // text
                                   Vector2.Zero,            // position
                                   Color.White);            // draw color
        }
    }
}
