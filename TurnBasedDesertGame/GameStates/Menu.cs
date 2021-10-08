using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame.GameStates
{
    public class Menu : GameState
    {
        // Whether the play button has been clicked...
        // resets whenever the state is changed
        private bool playButtonClicked;

        /// <summary>
        /// Creates a new Menu state
        /// </summary>
        /// <param name="game">your game1 class</param>
        /// <param name="objects">all the objects in this menu state</param>
        /// <param name="ui">all of the user interface in this menu state</param>
        public Menu()
        {
            ui.Add(new Button(new Rectangle((int)(Constants.ScreenCenter.X) - 523 / 2, 10, 523, 73), TextureManager.Get(Sprite.play)));
            ui[0].OnMouseClick += playButton_onClick;
        }

        /// <summary>
        /// Runs when this state starts up
        /// </summary>
        public override void Start()
        {
            // Do nothing
        }

        public override State? StateTransition()
        {
            // Any state changing code here
            if (playButtonClicked)
            {
                // Resetting the playButtonClicked field
                playButtonClicked = false;

                return State.Hub;
            }

            return base.StateTransition();
        }

        /// <summary>
        /// Called once every frame, this is where physics should be done
        /// </summary>
        /// <param name="gameTime">the gameTime variable from Game1's update method</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        // UI METHODS
        private void playButton_onClick(UI sender)
        {
            playButtonClicked = true;
            sender.Color = Color.Red;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
