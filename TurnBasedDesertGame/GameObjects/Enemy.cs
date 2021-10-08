using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TurnBasedDesertGame.GameObjects
{
    public class Enemy : Battleable
    {
        // Where the enemy sits during battle,
        // and where they end up when they attack
        private Vector2 idlePosition;
        private Vector2 attackPosition;

        private string name;

        public Enemy(Rectangle position, Texture2D sprite, string name, int hp = 10, int dmg = 10) : base(position, sprite, hp, dmg)
        {
            idlePosition = new Vector2((int)Constants.ScreenSize.X - position.Width * 2, (int)Constants.ScreenCenter.Y - position.Height / 2);
            attackPosition = new Vector2(0 + position.Width * 2, (int)Constants.ScreenCenter.Y - position.Height / 2);

            this.name = name;
        }

        public override string ToString()
        {
            return name;
        }

        protected override Vector2 AttackPosition() => attackPosition;

        protected override Vector2 IdlePosition() => idlePosition;

        protected override void FaceBattleDirection() => currentDirection = Directions.Left;
    }
}
