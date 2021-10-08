using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame
{
    public class Button : UI
    {
        /// <summary>
        /// Creates a new button that looks like image at position "position"
        /// </summary>
        /// <param name="position">the position of this button, as a rectangle</param>
        /// <param name="image">how this button appears on screen</param>
        public Button(Rectangle position, Texture2D image) : base(image, position)
        {
            OnMouseHover += default_onMouseHover;
            OnMouseExit += default_onMouseExit;
            OnMouseDown += default_onMouseDown;
        }

        private void default_onMouseHover(UI sender)
        {
            sender.Color = Color.Gray;
        }
        private void default_onMouseExit(UI sender)
        {
            sender.Color = Color.White;
        }
        private void default_onMouseDown(UI sender)
        {
            sender.Color = Color.Sienna;
        }
    }
}
