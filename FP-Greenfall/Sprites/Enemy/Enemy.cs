using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites.Enemy
{
    public abstract class Enemy : Character
    {
        protected Size enemySize;
        protected int speed;
        protected int radius;
        protected bool movingRight;
        protected bool chasingPlayer;

        protected int AttackFrame;
        protected int WalkFrame;

        protected int AttackAnimationInterval;
        protected int AttackCooldownInterval;

        protected Point playerPos;
        protected Point enemyPos;
        protected Point startPos;
        protected int dx;
        protected int dy;

        protected Player player;

        protected Enemy(Point startPosition, Player player)
        {
            this.player = player;
            startPos = startPosition;
            isAttacking = false;

            InitializeTimer();
        }

        // Chasing player logic
        protected virtual void MovementLogic()
        {
            if (characterPictureBox == null || player.GetPlayerPictureBox() == null) return;

            playerPos = player.GetPlayerPictureBox().Location;
            enemyPos = GetEnemyPictureBox().Location;

            dx = playerPos.X - enemyPos.X;
            dy = playerPos.Y - enemyPos.Y; 

            if (Math.Abs(dx) <= radius && Math.Abs(dy) <= radius)
            {
                chasingPlayer = true;
                if (dx < 0)
                {
                    facingLeft = true;
                    movingRight = false;
                }
                else
                {
                    facingLeft = false;
                    movingRight = true;
                }
            } else
            {
                chasingPlayer = false;
                currentFrame = 0;
                UpdateCharacter();
            }
        }

        public virtual void Animation(Size boundary, List<PictureBox> ground)
        {
            if (characterPictureBox == null) return;

            if (chasingPlayer)
            {
                if (movingRight)
                {
                    characterPictureBox.Left += speed;
                    currentFrame = (currentFrame + 1) % totalFrame;
                    UpdateCharacter();
                }
                else
                {
                    characterPictureBox.Left -= speed;
                    currentFrame = (currentFrame + 1) % totalFrame;
                    UpdateCharacter();
                }
            }

            ApplyGravity(ground);
        }

        protected void AttackPlayer()
        {
            if (facingLeft)
            {
                attackingBox = new Rectangle(
                     characterPictureBox.Left,
                     characterPictureBox.Top,
                     -attackRange,
                     characterPictureBox.Height
                 );
            } else
            {
                attackingBox = new Rectangle(
                    characterPictureBox.Right,
                    characterPictureBox.Top,
                    attackRange,
                    characterPictureBox.Height
                );
            }

            if(attackingBox.IntersectsWith(player.GetPlayerPictureBox().Bounds))
            {
                player.TakeDamage(this.damage, facingLeft ? -1 : 1);
            }
        }
        protected override void AttackCooldown()
        {
            base.AttackCooldown();

            attackTimer.Interval = AttackAnimationInterval;
            attackTimer.Tick += (s, e) =>
            {
                if (characterImg == null) return;

                characterImg = characterWalkImg;
                characterPictureBox.Image = characterImg;
                totalFrame = WalkFrame;
                UpdateCharacter();
                chasingPlayer = true;
                attackTimer.Stop();

                attackCooldown.Start();
            };

            attackCooldown.Interval = AttackCooldownInterval;
            attackCooldown.Tick += (s, e) =>
            {
                isAttacking = false;
                attackCooldown.Stop();
            };
        }

        public PictureBox GetEnemyPictureBox() => this.characterPictureBox;
    }
}
