using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame.Animations
{
    public class Animation
    {
        // The spritesheet this animation is on
        private Texture2D spriteSheet;

        // All the source rectangles of the animation
        private Rectangle[] frames;

        // Amount of seconds before the next frame is shown
        private float secondsPerFrame;

        private int framesPerSecond;

        // The seconds between this frame and the previous
        private float currentSeconds;

        // Frame we are currently on
        private int currentFrame;

        // Whether the animation has finished
        public bool AnimationFinished => 
            looping ? 
            currentFrame == frames.Length - 1 
            : currentFrame == frames.Length - 1 && currentSeconds >= secondsPerFrame;

        // Whether this is a looping animation
        private bool looping;

        /// <summary>
        /// This animations spritesheet
        /// </summary>
        public Texture2D SpriteSheet
        {
            get { return spriteSheet; }
        }

        // The row of the spriteSheet that the animation is on
        public Animation(Texture2D spriteSheet, Rectangle firstFrame, int totalFrames, int framesPerSecond, bool looping = true)
        {
            this.spriteSheet = spriteSheet;

            // Creating all the frames based on data given
            frames = new Rectangle[totalFrames];
            frames[0] = firstFrame;
            for (int i = 1; i < frames.Length; i++)
            {
                frames[i] = new Rectangle(frames[i - 1].X + frames[i - 1].Width, // X position scooches over by one box width 
                                          frames[i - 1].Y,                       // y position does not change
                                          frames[i - 1].Width,                   // width does not change
                                          frames[i - 1].Height);                 // height does not change
            }

            // Set frames per second
            this.framesPerSecond = framesPerSecond;
            secondsPerFrame = 1.0f / framesPerSecond;

            // No animations have been played,
            // so these of course start at 0
            currentSeconds = 0;
            currentFrame = 0;

            this.looping = looping;
        }

        public Animation Copy()
        {
            // Spritesheets can be shared. What matters (the reason this method exists)
            // is to make sure uses of identical animations are not synced when they shouldn't be.
            return new Animation(spriteSheet, frames[0], frames.Length, framesPerSecond, looping);
        }

        /// <summary>
        /// Gets the source rectangle of the current frame
        /// </summary>
        /// <returns>source rectangle of current frame</returns>
        public Rectangle GetCurrentFrame()
        {
            return frames[currentFrame];
        }

        /// <summary>
        /// Updates current frame if elapsed time requires it
        /// </summary>
        /// <param name="elapsedSeconds">time since last frame</param>
        public void UpdateFrames(float elapsedSeconds)
        {
            currentSeconds += elapsedSeconds;
            
            // No more work needs to be done if the animation is looping and we're on the last frame.
            if (!looping && currentFrame == frames.Length - 1) return;

            // If the currentSeconds is enough to go to the next frame,
            // return the next frame in the animation and reset currentSeconds
            if (currentSeconds > secondsPerFrame)
            {
                currentSeconds -= secondsPerFrame;

                // If the next frame is still in the array,
                // cycle to it. Otherwise, cycle to first frame
                if (currentFrame + 1 < frames.Length)
                {
                    currentFrame++;
                }
                else
                {
                    currentFrame = 0;
                }
            }
        }

        /// <summary>
        /// Resets animations currentFrame and time
        /// </summary>
        public void Reset()
        {
            currentFrame = 0;
            currentSeconds = 0;
        }
    }
}
