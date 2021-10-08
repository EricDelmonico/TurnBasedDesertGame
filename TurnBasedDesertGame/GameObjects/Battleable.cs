using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading.Tasks;
using TurnBasedDesertGame.UserInterfaceClasses;

namespace TurnBasedDesertGame.GameObjects
{
    public abstract class Battleable : GameObject
    {
        private enum BattleableStates
        {
            MovingToAttack,
            Attacking,
            MovingToIdle,
            Idling,
            Dying
        }

        // Attack damage
        private int damage;

        private int maxHealth;
        public int MaxHealth => maxHealth;

        private int currentHealth;
        public int CurrentHealth => currentHealth;

        public bool IsAlive => currentHealth > 0;

        public bool TurnOver { get; set; } = true;

        protected abstract Vector2 AttackPosition();
        protected abstract Vector2 IdlePosition();

        // Damage done for the last attack--used for the text that displays over the attackee.
        public virtual int DamageJustDone { get; set; } = -1;

        private BattleableStates state = BattleableStates.Idling;
        private BattleableStates prevState = BattleableStates.Idling;

        public Battleable(Rectangle position, Texture2D sprite, int maxHP, int dmg, bool defaultAnimations = true) : base(position, sprite)
        {
            maxHealth = maxHP;
            currentHealth = maxHealth;
            damage = dmg;

            CurrentAnimation = animations[AnimationNames.StickIdle];
            currentDirection = Directions.Left;
        }
        
        public override void Update(GameTime gameTime)
        {
            if (StateManager.CurrentState == State.Battle)
            {
                if (StateManager.PreviousState != State.Battle)
                {
                    BattleStart();
                }

                BattleLoop(gameTime);
            }

            base.Update(gameTime);
        }

        private void BattleStart()
        {
            SetPosition(IdlePosition().ToPoint());
            CurrentAnimation = animations[AnimationNames.StickIdle];
            FaceBattleDirection();
        }

        // t to plug into the lerp between the attack and idle positions.
        // t = 0 is in idle position, t = 1 is in attack position
        private float t = 0;
        private Battleable attackee;
        private void BattleLoop(GameTime gameTime)
        {
            BattleableStates toState = state;
            switch (state)
            {
                case BattleableStates.MovingToAttack:
                    CurrentAnimation = animations[AnimationNames.StickWalk];
                    if (Position.Location != AttackPosition().ToPoint())
                    {
                        t += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        MoveBetweenAttackAndIdle(t);
                    }
                    else
                    {
                        // made it to the attack position
                        toState = BattleableStates.Attacking;
                    }
                    break;
                case BattleableStates.Attacking:
                    CurrentAnimation = animations[AnimationNames.StickAttack];

                    if (CurrentAnimation.AnimationFinished)
                    {
                        toState = BattleableStates.MovingToIdle;
                        DamageJustDone = Constants.RandomNumber(damage);
                        attackee.TakeDamage(DamageJustDone);
                    }
                    break;
                case BattleableStates.MovingToIdle:
                    attackee = null;
                    CurrentAnimation = animations[AnimationNames.StickWalkBackwards];
                    if (Position.Location != IdlePosition().ToPoint())
                    {
                        t -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                        MoveBetweenAttackAndIdle(t);
                    }
                    else
                    {
                        // made it to the idle position
                        toState = BattleableStates.Idling;
                    }
                    break;
                case BattleableStates.Dying:
                    if (prevState != BattleableStates.Dying)
                    {
                        CurrentAnimation = animations[AnimationNames.StickDie];
                    }
                    break;
                default: // idling or unknown
                    // if the battleable was previously moving to idle, that means their turn just ended.
                    if (prevState == BattleableStates.MovingToIdle)
                    {
                        TurnOver = true;
                    }

                    SetPosition(IdlePosition().ToPoint());
                    CurrentAnimation = animations[AnimationNames.StickIdle];
                    break;
            }

            prevState = state;
            state = toState;
        }

        private bool MoveBetweenAttackAndIdle(float t, float secondsToMove = 1.5f)
        {
            var start = IdlePosition();
            var end = AttackPosition();

            // In the middle of one or the other
            if (t < secondsToMove && t > 0)
            {
                SetPosition(Vector2.Lerp(start, end, t / secondsToMove).ToPoint());
                return false;
            }
            // At idle position.
            else if (t <= 0)
            {
                SetPosition(start.ToPoint());
                t = 0;
                return true;
            }
            // At attacking position.
            // Same as "else if (t >= secondsToMove)"
            else
            {
                SetPosition(end.ToPoint());
                t = secondsToMove;
                return true;
            }
        }

        /// <summary>
        /// Damages this object by the specified amount
        /// </summary>
        /// <param name="damage">the amount of damage to do</param>
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0 || currentHealth == 0)
            {
                currentHealth = 0;
                state = BattleableStates.Dying;
            }

            var damageText = new TextElement(damage.ToString(), new Rectangle(), TextureManager.arial16b);
            damageText.Translate(new Point(Position.Width / 2, 0));

            damageText.OnUpdate += (_) =>
            {
                damageText.Translate(new Point(0, -1));

                if (damageText.Position.Location.Y < -60)
                {
                    children.Remove(damageText);
                }
            };

            children.Add(damageText);
        }

        public void Attack(Battleable b)
        {
            state = BattleableStates.MovingToAttack;
            attackee = b;
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
        }

        // Player faces right, enemy faces left
        protected abstract void FaceBattleDirection();
    }
}
