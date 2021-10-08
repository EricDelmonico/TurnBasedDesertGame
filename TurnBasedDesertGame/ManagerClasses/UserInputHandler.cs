using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TurnBasedDesertGame
{
    public enum Controls
    {
        MoveUp = 0,
        MoveLeft = 1,
        MoveDown = 2,
        MoveRight = 3,
        TraverseRooms = 4,
        Back = 5
    }

    public static class UserInputHandler
    {
        // Keyboard states of the prev. and current frames
        public static KeyboardState prev_kbState;
        public static KeyboardState kbState;
        
        // Mouse states of the prev. and current frames
        public static MouseState prev_mState;
        public static MouseState mState;

        // List of all controls in the game,
        // indexes specified by the Controls enum
        public static readonly List<Keys>[] keys =
        {
            new List<Keys> { Keys.W, Keys.Up },
            new List<Keys> { Keys.A, Keys.Left },
            new List<Keys> { Keys.S, Keys.Down },
            new List<Keys> { Keys.D, Keys.Right },
            new List<Keys> { Keys.Space },
            new List<Keys> { Keys.Back },
        };
        
        public static void UpdateInputs()
        {
            prev_kbState = kbState;
            prev_mState = mState;
            kbState = Keyboard.GetState();
            mState = Mouse.GetState();
        }

        /// <summary>
        /// Returns whether the specified control is pressed
        /// </summary>
        /// <param name="control">the control to test</param>
        /// <returns>true if the key is pressed</returns>
        public static bool IsKeyDown(Controls control)
        {
            bool pressed = false;
            for (int i = 0; i < keys[(int)control].Count; i++)
            {
                pressed = pressed || kbState.IsKeyDown(keys[(int)control][i]);
            }
            return pressed;
        }

        /// <summary>
        /// Returns whether the specified control was pressed last frame
        /// </summary>
        /// <param name="control">the control to test</param>
        /// <returns>true if the key was pressed</returns>
        public static bool WasKeyDown(Controls control)
        {
            bool pressed = false;
            for (int i = 0; i < keys[(int)control].Count; i++)
            {
                pressed = pressed || prev_kbState.IsKeyDown(keys[(int)control][i]);
            }
            return pressed;
        }

        /// <summary>
        /// Tests if this is the first frame this key was pressed
        /// </summary>
        /// <param name="control"></param>
        /// <returns></returns>
        public static bool IsFirstPress(Controls control) => IsKeyDown(control) && !WasKeyDown(control);
    }
}
