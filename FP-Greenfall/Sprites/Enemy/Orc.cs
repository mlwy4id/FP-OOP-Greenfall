using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites.Enemy
{
    public class Orc : Enemy
    {
        private static Image orcSpriteSheet;
        public Orc(Point startPosition, Player player) : base(startPosition, player)
        {
            health = 4;
            damage = 1;
            attackRange = 7;

            using (MemoryStream ms = new MemoryStream(Resource.Orc.Walk))
            {
                orcSpriteSheet = Image.FromStream(ms);
            }

            currentFrame = 0;
            currentRow = 0;
            totalFrame = 6;

            enemySize = new Size(120, 120);
            characterImg = orcSpriteSheet;
            radius = 350;
            speed = 5;

            chasingPlayer = false;

            characterPictureBox = new PictureBox
            {
                Size = enemySize,
                Location = startPosition,
                Image = characterImg,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Tag = this
            };

            UpdateCharacter();
        }

        public override void Animation(Size boundary, List<PictureBox> ground)
        {
            MovementLogic();
            base.Animation(boundary, ground);
        }
    }
}
