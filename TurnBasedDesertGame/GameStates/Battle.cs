using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using TurnBasedDesertGame.GameObjects;
using TurnBasedDesertGame.UserInterfaceClasses;

namespace TurnBasedDesertGame.GameStates
{
    public class Battle : GameState
    {
        // The player used 
        // across the whole game
        private Battleable player;

        // All enemies in order
        private Stack<Enemy> enemies;
        private Battleable currentEnemy;

        // Whoever died, whether it be the player or the enemy
        private Battleable oneWhoDied = null;
        // Whoever has control during the current turn.
        private Battleable currentBattleable = null;

        // True on the first frame a turn ended, false otherwise
        private bool prevTurnJustEnded = false;

        // All attack buttons
        private List<UI> attacks = new List<UI>();

        private State? nextState;

        /// <summary>
        /// Creates a new battle state
        /// </summary>
        /// <param name="player">the player used across the whole game</param>
        public Battle(Player player)
        {
            this.player = player;
            objects.Add(player);

            // Initializing the stack and adding some enemies
            enemies = new Stack<Enemy>();

            // Pushing some enemies
            enemies.Push(new Enemy(new Rectangle(0, 0, 100, 200), TextureManager.Get(Sprite.enemy1), "enemy1"));
            //enemies.Push(new Enemy(new Rectangle(0, 0, 100, 200), TextureManager.Get(Sprite.enemy3), "enemy2"));
            //enemies.Push(new Enemy(new Rectangle(0, 0, 100, 200), TextureManager.Get(Sprite.enemy2), "enemy3"));
            //enemies.Push(new Enemy(new Rectangle(0, 0, 100, 200), TextureManager.Get(Sprite.enemy1), "enemy4"));

            // The UI background
            ui.Add(new UI(TextureManager.Get(Sprite.battleUIBackground), new Rectangle(10, (int)Constants.ScreenCenter.Y + 100 + 10, (int)Constants.ScreenSize.X - 20, (int)Constants.ScreenSize.Y - (int)Constants.ScreenCenter.Y - 100 - 10 - 10)));

            // Adding the health UI
            // Player's
            ui.Add(new UI(TextureManager.Get(Sprite.battleHealthUI), new Rectangle(20, 20, 200, 50)));
            // Enemy's
            ui.Add(new UI(TextureManager.Get(Sprite.battleHealthUI), new Rectangle(Constants.ScreenBounds.Width - 20 - 200, 20, 200, 50)));

            PopulateAttacks();
            ui.AddRange(attacks);
        }

        private void PopulateAttacks()
        {
            // Adding the attack button and attaching the player attack method to it
            attacks.Add(new Button(new Rectangle(20, ((int)Constants.ScreenCenter.Y + 200 / 2) + 20, 75, 75), TextureManager.Get(Sprite.attackButton)));
            // The first attack will always be the default attack
            attacks[0].OnMouseClick += (_) => { player.Attack(currentEnemy); };

            // All attacks will cause the buttons to be hidden.
            for (int i = 0; i < attacks.Count; i++)
            {
                attacks[i].OnMouseClick += (_) =>
                {
                    TogglePlayerAttackUI(false);
                };
            }
        }

        /// <summary>
        /// Runs whenever this state starts up
        /// </summary>
        public override void Start()
        {
            nextState = null;

            // If the enemy didn't die, reset it's HP
            if (currentEnemy != null)
            {
                currentEnemy.ResetHealth();
            }
            // If the enemy did die, assign a new one
            else
            {
                if (enemies.Count > 0)
                {
                    currentEnemy = enemies.Pop();
                    objects.Add(currentEnemy);
                }
                else
                {
                    nextState = State.GameBeaten;
                }
            }

            currentBattleable = player;
            currentBattleable.TurnOver = false;
            oneWhoDied = null;
        }

        /// <summary>
        /// Runs every frame, handles game logic
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            bool playerTurn = currentBattleable == player;
            prevTurnJustEnded = false;

            if (currentBattleable.TurnOver)
            {
                // Swap currentBattleable
                currentBattleable = playerTurn ? currentEnemy : player;
                currentBattleable.TurnOver = false;

                // Turn changed, so trigger action below
                prevTurnJustEnded = true;

                // Update playerTurn bool to reflect the change in turns
                playerTurn = !playerTurn;
            }
            
            if (currentBattleable.IsAlive && prevTurnJustEnded)
            {
                TogglePlayerAttackUI(playerTurn);
                if (!playerTurn) currentBattleable.Attack(player);
            }
            else if (!currentBattleable.IsAlive)
            {
                oneWhoDied = currentBattleable;
            }

            // If the death animation finished, transition scenes
            if (oneWhoDied != null && oneWhoDied.CurrentAnimation.AnimationFinished)
            {
                nextState = State.LevelUp;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Runs every frame, draws all gameobjects and ui in this gamestate
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            // Drawing the background
            spriteBatch.Draw(TextureManager.Get(Sprite.battleBackground), Constants.ScreenBounds, Color.White);

            base.Draw(spriteBatch);

            // Happens if the state is entered when there are no enemies left to fight
            if (currentEnemy == null) return;

            // Drawing the enemy's health
            string enemyHealthString = $"{currentEnemy.CurrentHealth} / {currentEnemy.MaxHealth}";
            Vector2 textDimensions = TextureManager.arial16b.MeasureString(enemyHealthString);
            spriteBatch.DrawString(TextureManager.arial16b,                                                         // spritefont
                                   enemyHealthString,                                                               // text
                                   new Vector2(ui[2].Position.Right - textDimensions.X - 20,                        // x position
                                               ui[2].Position.Y + ui[2].Position.Height / 2 - textDimensions.Y / 2),// y position
                                   Color.White);                                                                    // draw color

            // Drawing the player's health
            string playerHealthString = $"{player.CurrentHealth} / {player.MaxHealth}";
            textDimensions = TextureManager.arial16b.MeasureString(playerHealthString);
            spriteBatch.DrawString(TextureManager.arial16b,                                                         // spritefont
                                   playerHealthString,                                                              // text
                                   new Vector2(ui[1].Position.Right - textDimensions.X - 20,                        // x position
                                               ui[1].Position.Y + ui[1].Position.Height / 2 - textDimensions.Y / 2),// y position
                                   Color.White);                                                                    // draw color
        }

        private void TogglePlayerAttackUI(bool active)
        {
            for (int i = 0; i < attacks.Count; i++)
            {
                attacks[i].IsActive = active;
            }
        }

        public override State? StateTransition()
        {
            // If we're leaving Battle and the enemy died, erase it so that it's not back next battle.
            if (nextState != null && oneWhoDied == currentEnemy)
            {
                currentEnemy = null;
            }

            return nextState;
        }
    }
}
