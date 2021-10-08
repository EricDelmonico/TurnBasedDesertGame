using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame.UserInterfaceClasses
{
    public class TextElement : UI
    {
        private string text;
        private SpriteFont font;

        /// <summary>
        /// Creates a new text element
        /// </summary>
        /// <param name="text"></param>
        /// <param name="position">Rectangle position of this text--only the center will be considered</param>
        /// <param name="font">the font to draw this text with</param>
        public TextElement(string text, Rectangle position, SpriteFont font)
            : base(null, new Rectangle(position.Location, font.MeasureString(text).ToPoint()))
        {
            this.text = text;
            this.font = font;
            Color = Color.Yellow;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawInPosition(spriteBatch, Position);
        }

        /// <summary>
        /// Draws this UI element, using its position as a position relative to the parent.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="parent"></param>
        public override void DrawRelative(SpriteBatch spriteBatch, GameObject parent)
        {
            DrawInPosition(spriteBatch, new Rectangle(Position.Location + parent.Position.Location, Position.Size));
        }

        private void DrawInPosition(SpriteBatch spriteBatch, Rectangle position)
        {
            spriteBatch.DrawString(font,
                                   text,
                                   position.Location.ToVector2(),
                                   Color);
        }
    }
}
