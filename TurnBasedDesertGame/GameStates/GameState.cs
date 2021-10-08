using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame
{
    /// <summary>
    /// The base class for any game states in the game
    /// </summary>
    public abstract class GameState
    {
        // All the UI in this game state
        protected List<UI> ui;
        // All the objects present in this game state
        protected List<GameObject> objects;
        // Frames per second of the game
        protected float FPS;

        private readonly State key;
        public State Key => key;

        public GameState()
        {
            key = (State)Enum.Parse(typeof(State), this.GetType().Name);
            
            ui = new List<UI>();
            objects = new List<GameObject>();
        }

        /// <summary>
        /// Runs whenever a state starts
        /// </summary>
        public abstract void Start();

        /// <summary>
        /// Called once every frame of the game, this is where all physics and other calculations should go.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            FPS = (float)(1.0f / gameTime.ElapsedGameTime.TotalSeconds);

            for (int i = 0; i < ui.Count; i++)
            {
                if (ui[i].IsActive)
                {
                    ui[i].Update(gameTime);
                }
            }

            // Updating all objects
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Update(gameTime);
            }
        }

        /// <summary>
        /// Where all drawing in the game is handled. Draw is called once every frame.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Drawing all UI
            for (int i = 0; i < ui.Count; i++)
            {
                if (ui[i].IsActive)
                {
                    ui[i].Draw(spriteBatch);
                }
            }

            // Drawing all game objects
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Draw(spriteBatch);
            }

            // Drawing framerate to the screen
            spriteBatch.DrawString(TextureManager.arial12, FPS.ToString(), new Vector2(), Color.Magenta);
        }

        /// <summary>
        /// This method returns the state to transition to if a transition happens
        /// </summary>
        /// <returns>Transitioned to state, or null if no transition</returns>
        public virtual State? StateTransition()
        {
            return null;
        }
    }
}
