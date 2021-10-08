using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TurnBasedDesertGame.Animations;

namespace TurnBasedDesertGame
{
    public enum AnimationNames
    {
        StickIdle,
        StickWalk,
        StickWalkBackwards,
        StickAttack,
        StickDie,
    }
}

namespace TurnBasedDesertGame.ManagerClasses
{
    /// <summary>
    /// Essentially creating a static indexer using an AnimContainer as a singleton in AnimationManager.
    /// </summary>
    public class AnimationContainer
    {
        internal Dictionary<AnimationNames, Animation> animations = new Dictionary<AnimationNames, Animation>();
        public Animation this[AnimationNames name]
        {
            get => animations[name];
        }

        public AnimationContainer()
        {
            InitAnimations();
        }

        public AnimationContainer(Dictionary<AnimationNames, Animation> animations)
        {
            foreach ( var kvp in animations)
            {
                this.animations.Add(kvp.Key, kvp.Value.Copy());
            }
        }

        private void InitAnimations()
        {
            animations.Add(
                AnimationNames.StickIdle,
                new Animation(
                    TextureManager.Get(SpriteSheet.idle_walk_walkBack_4FrameAnimation100x200),
                    new Rectangle(0, 0, 100, 200), // first frame
                    4,                             // total frames
                    2));                           // frames per second

            animations.Add(
                AnimationNames.StickWalk,
                new Animation(
                    TextureManager.Get(SpriteSheet.idle_walk_walkBack_4FrameAnimation100x200),
                    new Rectangle(0, 200, 100, 200), // first frame
                    4,                               // total frames
                    10));                            // frames per second

            animations.Add(
                AnimationNames.StickWalkBackwards,
                new Animation(
                    TextureManager.Get(SpriteSheet.idle_walk_walkBack_4FrameAnimation100x200),
                    new Rectangle(0, 400, 100, 200), // first frame
                    4,                               // total frames
                    10));                            // frames per second

            animations.Add(
                AnimationNames.StickAttack,
                new Animation(
                    TextureManager.Get(SpriteSheet.attack_die_8FrameAnimation100x200),
                    new Rectangle(0, 0, 100, 200), // first frame
                    8,                             // total frames
                    16));                          // frames per second

            animations.Add(
                AnimationNames.StickDie,
                new Animation(
                    TextureManager.Get(SpriteSheet.attack_die_8FrameAnimation100x200),
                    new Rectangle(0, 200, 100, 200), // first frame
                    8,                               // total frames
                    6,                               // frames per second
                    false));                         // looping
        }
    }

    public static class AnimationManager
    {
        // Creating the singleton.
        private static AnimationContainer original = new AnimationContainer();
        
        // Give a copy whenever requested
        public static AnimationContainer GetCopyOfAnimations() => new AnimationContainer(original.animations);
    }
}
