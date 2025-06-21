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

        protected Point playerPos;
        protected Point enemyPos;
        protected Point startPos;
        protected int dx;

        protected Player player;

        protected Enemy(Point startPosition, Player player)
        {
            this.player = player;
            startPos = startPosition;
        }

        // Chasing player logic
        protected virtual void MovementLogic()
        {
            playerPos = player.GetPlayerPictureBox().Location;
            enemyPos = GetEnemyPictureBox().Location;

            dx = playerPos.X - enemyPos.X;

            if (Math.Abs(dx) <= radius)
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
            if(chasingPlayer)
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

                ApplyGravity(ground);
            } else
            {
                ApplyGravity(ground);
            }

            if (IsDead())
            {
                Die();
            }
        }

        public PictureBox GetEnemyPictureBox() => this.characterPictureBox;
    }
}
