using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TurnBasedDesertGame.GameObjects;
using TurnBasedDesertGame.GameStates;

namespace TurnBasedDesertGame
{
    public enum State
    {
        Battle,
        GameBeaten,
        GameOver,
        Hub,
        LevelUp,
        Menu,
    }

    public static class StateManager
    {
        // All of the game states
        private static Dictionary<State, GameState> AllStates;

        private static GameState state;
        public static State CurrentState => state.Key;

        private static GameState prevState;
        public static State PreviousState => prevState.Key;

        public static void InitStates(Player player)
        {
            // Initializing the dictionary of states
            AllStates = new Dictionary<State, GameState>();

            AllStates.Add(State.Hub, new Hub(player));
            AllStates.Add(State.Battle, new Battle(player));
            AllStates.Add(State.LevelUp, new LevelUp());
            AllStates.Add(State.GameOver, new GameOver());
            AllStates.Add(State.GameBeaten, new GameBeaten());

            AllStates.Add(State.Menu, new Menu());
            state = AllStates[State.Menu];
        }

        public static void Update(GameTime gameTime)
        {
            prevState = state;
            // Checking for any state transitions
            State? toState = state.StateTransition();
            if (toState.HasValue)
            {
                state = AllStates[toState.Value];
                state.Start();
            }

            // Updating the state
            state.Update(gameTime);
        }
        
        public static void DrawState(SpriteBatch spriteBatch)
        {
            // Drawing the current state to the screen
            state.Draw(spriteBatch);
        }
    }
}
