using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites.Enemy
{
    public abstract class Enemy : Character
    {
        protected int speed;
        protected int radius;
        protected bool movingRight;

        protected Point playerPos;
        protected Point enemyPos;
        protected int dx;

        protected Player player;

        protected Enemy(Point startPosition, Player player)
        {
            this.player = player;
            MovementLogic();
            
        }

        protected virtual void MovementLogic()
        {
            playerPos = player.GetPlayerPictureBox().Location;
            enemyPos = this.GetEnemyPictureBox().Location;

            dx = playerPos.X - enemyPos.X;

            if(Math.Abs(dx) <= radius)
            {
                if(dx < 0)
                {
                    facingLeft = true;
                    movingRight = false;
                } else
                {
                    facingLeft = false;
                    movingRight = true;
                }
            }
        }

        public PictureBox GetEnemyPictureBox() => this.characterPictureBox;
    }
}
