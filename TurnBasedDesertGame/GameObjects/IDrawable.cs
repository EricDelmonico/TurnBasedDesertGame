using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame
{
    public interface IDrawable
    {
        Color Color { get; }
        /// <summary>
        /// Screen position or relative position, depending on whether Draw or DrawRelative is used.
        /// </summary>
        Rectangle Position { get; set; }
        void Draw(SpriteBatch spriteBatch);
        void DrawRelative(SpriteBatch spriteBatch, GameObject parent);
        void Update(GameTime gameTime);

        // Methods to eliminate the need for accessing the position backing field outside 
        // of the base classes, while still allowing me to change position.Location.
        void SetPosition(Point point);
        void Translate(Point point);
    }
}
