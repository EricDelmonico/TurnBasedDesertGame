using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TurnBasedDesertGame
{
    public enum SpriteSheet
    {
        idle_walk_walkBack_4FrameAnimation100x200,
        attack_die_8FrameAnimation100x200,
    }

    public enum Sprite
    {
        play,
        player,
        battleUIBackground,
        homeRoom,
        secretRoom,
        battleRoom,
        bazaar,
        bazaar2,
        healRoom,
        enemy1,
        enemy2,
        enemy3,
        enemy4,
        battleBackground,
        battleHealthUI,
        attackButton,
    }

    public static class TextureManager
    {
        // Textures
        private static Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();

        // SpriteFonts
        public static SpriteFont arial12;
        public static SpriteFont arial16b;

        /// <summary>
        /// Loads all textures in the game
        /// </summary>
        /// <param name="Content"></param>
        public static void LoadTextures(ContentManager Content)
        {
            // Get the string name of all spritesheets and textures
            var allTexNames = 
                Enum.GetNames(typeof(Sprite))
                .Concat(Enum.GetNames(typeof(SpriteSheet))).ToArray();

            // Load in all spritesheets and textures
            for (int i = 0; i < allTexNames.Length; i++)
            {
                textures.Add(allTexNames[i], Content.Load<Texture2D>(allTexNames[i]));
            }

            arial12 = Content.Load<SpriteFont>("arial12");
            arial16b = Content.Load<SpriteFont>("arial16b");
        }

        public static Texture2D Get(Sprite texture)
        {
            return textures[texture.ToString()];
        }

        public static Texture2D Get(SpriteSheet spriteSheet)
        {
            return textures[spriteSheet.ToString()];
        }
    }
}
