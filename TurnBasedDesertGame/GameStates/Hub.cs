using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using TurnBasedDesertGame.GameObjects;

namespace TurnBasedDesertGame.GameStates
{
    public class Hub : GameState
    {
        // The player used across the whole game
        private Player player;

        // Room the player is in
        private Room currentRoom;

        /// <summary>
        /// Creates a new hub world with the specified player
        /// </summary>
        /// <param name="player">the player used for the whole game</param>
        public Hub(Player player)
        {
            // Making the player and adding 
            // it to the list of GameObjects
            this.player = player;
            this.objects.Add(player);

            // Setting up all rooms
            Room homeRoom = new Room(TextureManager.Get(Sprite.homeRoom), "home");
            Room battleRoom = new Room(TextureManager.Get(Sprite.battleRoom), "battle room");
            Room secretRoom = new Room(TextureManager.Get(Sprite.secretRoom), "secret room");
            Room bazaar = new Room(TextureManager.Get(Sprite.bazaar), "bazaar");
            Room bazaar2 = new Room(TextureManager.Get(Sprite.bazaar2), "bazaar 2");
            Room healRoom = new Room(TextureManager.Get(Sprite.healRoom), "heal room");

            homeRoom.PopulateNeighbors(battleRoom, healRoom, null, bazaar);
            battleRoom.PopulateNeighbors(null, homeRoom, secretRoom, null);
            secretRoom.PopulateNeighbors(null, null, null, battleRoom);
            bazaar.PopulateNeighbors(null, null, homeRoom, bazaar2);
            bazaar2.PopulateNeighbors(null, null, bazaar, null);
            healRoom.PopulateNeighbors(homeRoom, null, null, null);

            currentRoom = homeRoom;
        }

        /// <summary>
        /// Runs whenever the state starts up
        /// </summary>
        public override void Start() { /* do nothing */ }

        /// <summary>
        /// Runs every frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            player.HandleMovement(ref currentRoom);

            Console.WriteLine();
            Console.WriteLine(currentRoom);
            Console.WriteLine();

            base.Update(gameTime);
        }

        /// <summary>
        /// Runs every frame, draws all gameobjects and ui in this scene
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Drawing the room's background before anything else
            spriteBatch.Draw(currentRoom.Background, Constants.ScreenBounds, Color.White);

            base.Draw(spriteBatch);
        }

        public override State? StateTransition()
        {
            if (UserInputHandler.IsFirstPress(Controls.Back))
                return State.Menu;

            if (UserInputHandler.kbState.IsKeyDown(Keys.Enter))
                return State.Battle;

            return base.StateTransition();
        }
    }
}
