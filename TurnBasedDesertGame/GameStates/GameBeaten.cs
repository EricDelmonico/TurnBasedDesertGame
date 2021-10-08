using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame.GameStates
{
    public class GameBeaten : GameState
    {
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(TextureManager.arial16b, "You have already beaten this portal", Vector2.Zero, Color.White);
        }
    }
}
