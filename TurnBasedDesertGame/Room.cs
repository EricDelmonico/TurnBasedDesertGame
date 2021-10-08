using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace TurnBasedDesertGame
{
    // TODO: GIVE THE HUB CLASS ALL THE OBJECTS FROM THE CURRENT ROOM

    public class Room
    {
        // Whether the neighbors have been populated
        bool hasNeighbors;

        // Rooms on any side of this room
        private Room up;
        private Room down;
        private Room left;
        private Room right;

        // The background image of this room
        private Texture2D background;

        // Game objects in this room
        private List<GameObject> objects;

        // The name of this room
        private string name;

        /// <summary>
        /// Returns a copy of the objects list so as not to disturb the original copies
        /// </summary>
        public List<GameObject> ObjectsCopy
        {
            get
            {
                return (from g in objects
                        select new GameObject(g.Position, g.Sprite)).ToList();
            }
        }

        /// <summary>
        /// The background image of this room
        /// </summary>
        public Texture2D Background
        {
            get { return background; }
        }

        /// <summary>
        /// The name of this room
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Creates a new room with given fields
        /// </summary>
        /// <param name="background">the background image of this room</param>
        /// <param name="name">this rooms name</param>
        /// <param name="objects">all the objects in this room</param>
        public Room(Texture2D background, string name, params GameObject[] objects)
        {
            this.objects = objects.ToList();
            this.background = background;
            this.name = name;

            // This room doesn't have any neighbors yet
            hasNeighbors = false;
        }

        /// <summary>
        /// Fills up the neighbors of this room, if no neighbors have already been given
        /// </summary>
        /// <param name="up">the room above this one</param>
        /// <param name="down">the room below this one</param>
        /// <param name="left">the room to the left of this one</param>
        /// <param name="right">the room to the right of this one</param>
        public void PopulateNeighbors(Room up, Room down, Room left, Room right)
        {
            if (!hasNeighbors)
            {
                this.up = up;
                this.down = down;
                this.left = left;
                this.right = right;

                hasNeighbors = true;
            }
        }

        /// <summary>
        /// Traverses in given direction, if possible
        /// </summary>
        /// <param name="direction">the direction to traverse</param>
        /// <param name="room">the current room</param>
        /// <param name="playerLocation">the location of the player</param>
        /// <param name="playerSize">the size of the player</param>
        public Point TraverseRoom(Directions direction, ref Room room, Point playerLocation, Point playerSize)
        {
            switch (direction)
            {
                case Directions.Up:
                    if (up != null)
                    {
                        room = up;
                        return new Point(playerLocation.X, (int)Constants.ScreenSize.Y - playerSize.Y);
                    }
                    break;
                case Directions.Down:
                    if (down != null)
                    {
                        room = down;
                        return new Point(playerLocation.X, 0);
                    }
                    break;
                case Directions.Left:
                    if (left != null)
                    {
                        room = left;
                        return new Point((int)Constants.ScreenSize.X - playerSize.X, playerLocation.Y);
                    }
                    break;
                case Directions.Right:
                    if (right != null)
                    {
                        room = right;
                        return new Point(0, playerLocation.Y);
                    }
                    break;
            }
            return playerLocation;
        }

        public override string ToString()
        {
            return $"{name}: \nUP: {(up != null ? up.Name : null)}\nDOWN: {(down != null ? down.Name : null)}\nLEFT: {(left != null ? left.Name : null)}\nRIGHT: {(right != null ? right.Name : null)}";
        }
    }
}
