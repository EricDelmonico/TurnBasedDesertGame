using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame.GameObjects
{
    public class Player : Battleable
    {
        // FIELDS

        // The player's walk speed in the hub world
        private int walkSpeed;

        // The player's movement every frame
        private Point movement;

        // Where the player sits during battle,
        // and where he ends up when he attacks
        private Vector2 idlePosition;
        private Vector2 attackPosition;

        // The direction the player was 
        // facing last frame and current frame
        private Directions prevDirection;

        /// <summary>
        /// Creates a new player at position "position" and drawing with the sprite "sprite," who walks at speed "walkSpeed"
        /// </summary>
        /// <param name="position">the position of the player</param>
        /// <param name="sprite">the sprite that the player will be drawn with</param>
        /// <param name="walkSpeed">the speed at which the player will walk in the hub world</param>
        /// <param name="hp">The hp of the player</param>
        /// <param name="dmg">The damage the player deals in battle</param>
        public Player(Rectangle position, Texture2D sprite, int walkSpeed, int hp = 100, int dmg = 10) : base(position, sprite, hp, dmg)
        {
            this.walkSpeed = walkSpeed;
            color = Color.Red;

            idlePosition = new Vector2(100, (int)Constants.ScreenCenter.Y - position.Height / 2);
            attackPosition = new Vector2((int)Constants.ScreenSize.X - position.Width * 3, (int)Constants.ScreenCenter.Y - position.Height / 2);
        }

        /// <summary>
        /// Handling player movement--actual movement as well as input handling
        /// </summary>
        public void HandleMovement(ref Room currentRoom)
        {
            HandleMovementInput(ref movement, ref currentRoom);

            // Adding movement to position every frame
            Translate(movement);
        }

        /// <summary>
        /// Find what direction the player is walking in, if any
        /// </summary>
        /// <returns></returns>
        private void HandleMovementInput(ref Point movement, ref Room currentRoom)
        {
            prevDirection = currentDirection;

            // Resetting movement
            movement = Point.Zero;

            // Whether the player is moving or not
            bool moving = false;

            Point up = new Point(0, -walkSpeed);
            Point down = new Point(0, walkSpeed);
            Point right = new Point(walkSpeed, 0);
            Point left = new Point(-walkSpeed, 0);

            // Pressing up key
            if (UserInputHandler.IsKeyDown(Controls.MoveUp) &&
                Constants.ScreenBounds.Contains(Position.Location + up))
            {
                movement += up;
                moving = true;
            }
            // If the player moves out of the upper bound, traverse up
            else if (!Constants.ScreenBounds.Contains(Position.Location + up) &&
                     currentRoom != null &&
                     UserInputHandler.IsFirstPress(Controls.TraverseRooms) &&
                     UserInputHandler.IsKeyDown(Controls.MoveUp))
            {
                SetPosition(currentRoom.TraverseRoom(Directions.Up, ref currentRoom, Position.Location, Position.Size));
                return;
            }

            // Pressing down key
            else if (UserInputHandler.IsKeyDown(Controls.MoveDown) &&
                     Constants.ScreenBounds.Contains(Position.Location + Position.Size + down))
            {
                movement += down;
                moving = true;
            }
            // if the player moves out of the lower bound, traverse down
            else if (!Constants.ScreenBounds.Contains(Position.Location + Position.Size + down) &&
                     currentRoom != null &&
                     UserInputHandler.IsFirstPress(Controls.TraverseRooms) &&
                     UserInputHandler.IsKeyDown(Controls.MoveDown))
            {
                SetPosition(currentRoom.TraverseRoom(Directions.Down, ref currentRoom, Position.Location, Position.Size));
                return;
            }

            // Pressing right key
            if (UserInputHandler.IsKeyDown(Controls.MoveRight) &&
                Constants.ScreenBounds.Contains(Position.Location + Position.Size + right))
            {
                movement += right;
                currentDirection = Directions.Right;
                moving = true;
            }
            // If the player moves out of the right bound, traverse right
            else if (!Constants.ScreenBounds.Contains(Position.Location + Position.Size + right) &&
                     currentRoom != null &&
                     UserInputHandler.IsFirstPress(Controls.TraverseRooms) &&
                     UserInputHandler.IsKeyDown(Controls.MoveRight))
            {
                SetPosition(currentRoom.TraverseRoom(Directions.Right, ref currentRoom, Position.Location, Position.Size));
                return;
            }

            // Pressing left key
            else if (UserInputHandler.IsKeyDown(Controls.MoveLeft) &&
                     Constants.ScreenBounds.Contains(Position.Location + left))
            {
                movement += left;
                currentDirection = Directions.Left;
                moving = true;
            }
            // If the player moves out of the left bound, traverse left
            else if (!Constants.ScreenBounds.Contains(Position.Location + left) &&
                     currentRoom != null &&
                     UserInputHandler.IsFirstPress(Controls.TraverseRooms) &&
                     UserInputHandler.IsKeyDown(Controls.MoveLeft))
            {
                SetPosition(currentRoom.TraverseRoom(Directions.Left, ref currentRoom, Position.Location, Position.Size));
                return;
            }

            if (!moving)
            {
                // The player is not moving
                movement = Point.Zero;
                currentDirection = prevDirection;

                // player is idle
                CurrentAnimation = animations[AnimationNames.StickIdle];
            }
            else
            {
                // player is walking
                CurrentAnimation = animations[AnimationNames.StickWalk];
            }
        }

        protected override Vector2 AttackPosition() => attackPosition;

        protected override Vector2 IdlePosition() => idlePosition;

        protected override void FaceBattleDirection() => currentDirection = Directions.Right;
    }
}
