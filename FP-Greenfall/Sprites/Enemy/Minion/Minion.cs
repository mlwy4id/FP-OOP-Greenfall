using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FP_Greenfall.Sprites.Enemy.Minion
{
    public abstract class Minion : Enemy
    {
        protected Minion(Point startPosition, Player player) : base(startPosition, player) { }

        protected override void MovementLogic()
        {
            base.MovementLogic();
            
            if(isMovingRight)
            {

            }
        }
    }
}
