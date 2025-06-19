using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites.Enemy
{
    public class Slime : Enemy
    {
        private static Image slimeSpriteSheet;
        public Slime(Point startPosition, Player player) : base(startPosition, player)
        {
            using (MemoryStream ms = new MemoryStream(Resource.Slime.Walk))
            {
                slimeSpriteSheet = Image.FromStream(ms);
            }

            currentFrame = 0;
            currentRow = 0;
            totalFrame = 7;

            enemySize = new Size(100, 100);
            characterImg = slimeSpriteSheet;
            radius = 300;
            speed = 5;

            chasingPlayer = false;

            characterPictureBox = new PictureBox
            {
                Size = enemySize,
                Location = startPosition,
                Image = characterImg,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };

            UpdateCharacter();
        }

        public override void Animation(Size boundary, List<PictureBox> ground)
        {
            base.Animation(boundary, ground);
            MovementLogic();
        }
    }
}
