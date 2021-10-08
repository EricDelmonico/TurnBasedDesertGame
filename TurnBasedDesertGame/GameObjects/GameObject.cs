using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedDesertGame.Animations;
using TurnBasedDesertGame.ManagerClasses;

namespace TurnBasedDesertGame
{
    public enum Directions
    {
        Left,
        Right,
        Up,
        Down
    }

    public class GameObject : IDrawable
    {
        // Position and size of this game object
        private Rectangle position;
        // The sprite that will be used to render this gameobject
        protected Texture2D sprite;
        // A container for all the animations in the program.
        protected AnimationContainer animations = AnimationManager.GetCopyOfAnimations();
        // This objects current animation
        private Animation currentAnimation;
        // The direction this object is facing
        protected Directions currentDirection;
        // The color to modify drawing with
        protected Color color = Color.White;

        // Things to draw along with this object
        protected List<IDrawable> children = new List<IDrawable>();

        public Color Color => color;

        public Animation CurrentAnimation
        {
            get => currentAnimation;
            set
            {
                if (currentAnimation != value)
                {
                    currentAnimation = value;
                    currentAnimation.Reset();
                }
            }
        }

        /// <summary>
        /// Position and size of this game object
        /// </summary>
        public Rectangle Position
        {
            get => position;
            set => position = value;
        }

        /// <summary>
        /// The sprite that will be used to render this gameobject
        /// </summary>
        public Texture2D Sprite => sprite;

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <param name="position">the rectangle position of this GameObject</param>
        /// <param name="sprite">the sprite used to render this GameObject</param>
        public GameObject(Rectangle position, Texture2D sprite)
        {
            this.position = position;
            this.sprite = sprite;
        }

        /// <summary>
        /// Updates the current animation this game object is playing
        /// </summary>
        /// <param name="gameTime">the gameTime used in all update methods</param>
        private void UpdateAnimation(GameTime gameTime)
        {
            if (currentAnimation != null)
                currentAnimation.UpdateFrames((float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        /// <summary>
        /// Draws this GameObject
        /// </summary>
        /// <param name="spriteBatch">the spritebatch used in game1's draw method</param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawInPosition(spriteBatch, Position);

            // Draw children relative to this object
            for (int i = 0; i < children.Count; i++)
            {
                children[i].DrawRelative(spriteBatch, this);
            }
        }

        /// <summary>
        /// Draws this GameObject but uses treats its location as a relative location to its parent.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="parent"></param>
        public void DrawRelative(SpriteBatch spriteBatch, GameObject parent)
        {
            DrawInPosition(spriteBatch, new Rectangle(parent.Position.Location + Position.Location, position.Size));
        }

        private void DrawInPosition(SpriteBatch spriteBatch, Rectangle position)
        {
            if (currentAnimation != null)
            {
                // Only flip the sprite if the player is facing left
                SpriteEffects spriteEffects = currentDirection == Directions.Right ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

                spriteBatch.Draw(
                    currentAnimation.SpriteSheet,       // sprite
                    position,                           // rectangle position
                    currentAnimation.GetCurrentFrame(), // source rectangle
                    color,                              // color
                    0.0f,                               // rotation
                    Vector2.Zero,                       // origin
                    spriteEffects,                      // sprite effects
                    0.0f);                              // layer depth
            }
            else
            {
                spriteBatch.Draw(sprite, position, color);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            UpdateAnimation(gameTime);

            for (int i = 0; i < children.Count; i++)
            {
                children[i].Update(gameTime);
            }
        }

        public void Translate(Point p)
        {
            position.Location += p;
        }

        public void SetPosition(Point p)
        {
            position.Location = p;
        }
    }
}
