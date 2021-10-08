using Microsoft.Xna.Framework;
using System;

namespace TurnBasedDesertGame
{
    public static class Constants
    {
        public static Vector2 ScreenSize;
        public static Vector2 ScreenCenter;
        public static Rectangle ScreenBounds;
        private static Random rng;

        public static void LoadConstants(Game1 game1)
        {
            ScreenSize = new Vector2(game1.GraphicsDevice.Viewport.Width, game1.GraphicsDevice.Viewport.Height);
            ScreenCenter = new Vector2(ScreenSize.X / 2, ScreenSize.Y / 2);
            ScreenBounds = game1.GraphicsDevice.Viewport.Bounds;

            rng = new Random();
        }

        /// <summary>
        /// Generates a random number between min and max
        /// </summary>
        /// <param name="min">min value (inclusive)</param>
        /// <param name="max">max value (not inclusive)</param>
        /// <returns>a random number between the given values</returns>
        public static int RandomNumber(int min, int max)
        {
            return rng.Next(min, max);
        }

        /// <summary>
        /// Generates a random number between zero and the given number
        /// </summary>
        /// <param name="max">maximum value (not inclusive)</param>
        /// <returns>a random number between 0 and the max</returns>
        public static int RandomNumber(int max)
        {
            return rng.Next(max);
        }
    }
}
