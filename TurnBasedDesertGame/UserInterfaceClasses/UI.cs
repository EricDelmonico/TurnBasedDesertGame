using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TurnBasedDesertGame
{
    public class UI : IDrawable
    {
        // Events
        // What happens when this button is clicked
        public event UIEvent OnMouseClick;
        // What happens when this ui element is hovered over
        public event UIEvent OnMouseHover;
        // What happens when the mouse exit this button
        public event UIEvent OnMouseExit;
        // What happens the moment the left mouse button is down on this ui element
        public event UIEvent OnMouseDown;
        public event Action<GameTime> OnUpdate;

        // How the UI will appear on screen
        protected Texture2D image;

        /// <summary>
        /// Whether this ui element is active or not
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// The color of this UI element
        /// </summary>
        public Color Color { get; set; }

        private Rectangle position;
        /// <summary>
        /// The rectangle position of this ui element
        /// </summary>
        public Rectangle Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// Creates a new UI object
        /// </summary>
        /// <param name="image">how the ui will appear on-screen</param>
        /// <param name="position">where the ui will appear on-screen</param>
        public UI(Texture2D image, Rectangle position)
        {
            this.image = image;
            Position = position;
            Color = Color.White;

            // UI elements will start out active
            IsActive = true;
        }

        /// <summary>
        /// Updates this UI element
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            CheckClick();
            CheckMouseOver();
            CheckMouseDown();

            OnUpdate?.Invoke(gameTime);
        }

        /// <summary>
        /// Draws this UI element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch in game1's draw method</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawInPosition(spriteBatch, Position);
        }

        /// <summary>
        /// Draws this UI element, using its position as a position relative to the parent.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="parent"></param>
        public virtual void DrawRelative(SpriteBatch spriteBatch, GameObject parent)
        {
            DrawInPosition(spriteBatch, new Rectangle(Position.Location + parent.Position.Location, Position.Size));
        }

        private void DrawInPosition(SpriteBatch spriteBatch, Rectangle position)
        {
            spriteBatch.Draw(image, position, Color);
        }

        public void SetPosition(Point point)
        {
            position.Location = point;
        }

        public void Translate(Point point)
        {
            position.Location += point;
        }

        /// <summary>
        /// Returns true if the mouse is inside of this UI element
        /// </summary>
        /// <returns>whether the mouse is inside of this ui element</returns>
        private bool CheckMouseOver()
        {
            if (Position.Contains(UserInputHandler.mState.Position))
            {
                // If there are methods in the onHover event, invoke it
                OnMouseHover?.Invoke(this);

                // The mouse is inside this ui element
                return true;
            }

            // Mouse is not inside this UI element
            OnMouseExit?.Invoke(this);
            return false;
        }

        /// <summary>
        /// Returns true and runs all onClick methods if the mouse is in this ui element and the mouse is being clicked
        /// </summary>
        /// <returns>true if the ui element was clicked</returns>
        private bool CheckClick()
        {
            if (CheckMouseOver() &&                                               // If the mouse is inside of this ui element
                UserInputHandler.prev_mState.LeftButton == ButtonState.Pressed && // and the left mouse button is clicked
                UserInputHandler.mState.LeftButton != ButtonState.Pressed)        // and the left mouse button was up the previous frame
            {
                // If there are methods in the onCLick event, invoke it
                OnMouseClick?.Invoke(this);

                // The ui element was clicked
                return true;
            }

            // The ui element was either not clicked, 
            // or the mouse was not in the right spots
            return false;
        }

        /// <summary>
        /// Returns true and runs all onMouseDown methods if the mouse is in this ui element and the mouse is down
        /// </summary>
        /// <returns>true if the ui element was clicked</returns>
        private bool CheckMouseDown()
        {
            if (CheckMouseOver() &&                                         // If the mouse is inside of this ui element
                UserInputHandler.mState.LeftButton == ButtonState.Pressed)  // and the left mouse button was up the previous frame
            {
                // If there are methods in the onClick event, invoke it
                OnMouseDown?.Invoke(this);

                // The ui element was clicked
                return true;
            }

            // The ui element was either not clicked, 
            // or the mouse was not in the right spots
            return false;
        }
    }
}
