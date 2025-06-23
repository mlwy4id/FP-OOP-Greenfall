using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites.Enemy
{
    public class Orc : Enemy
    {
        public Orc(Point startPosition, Player player) : base(startPosition, player)
        {
            health = 4;
            damage = 1;

            using (MemoryStream ms = new MemoryStream(Resource.Orc.Orc_Walking))
            {
                characterWalkImg = Image.FromStream(ms);
            }
            using (MemoryStream ms = new MemoryStream(Resource.Orc.Orc_Attack))
            {
                characterAttackImg = Image.FromStream(ms);
            }

            currentFrame = 0;
            currentRow = 0;
            totalFrame = 6;

            enemySize = new Size(120, 120);
            attackRange = enemySize.Width - 5;
            characterImg = characterWalkImg;

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
            AttackCooldown();
        }

        public override void Animation(Size boundary, List<PictureBox> ground)
        {
            MovementLogic();
            base.Animation(boundary, ground);
            if (characterPictureBox == null) return;
            
            if (Math.Abs(dx) <= attackRange && player.GetPlayerPictureBox() != null && !isAttacking && !attackCooldown.Enabled)
            {
                characterImg = characterAttackImg;
                characterPictureBox.Image = characterImg;
                totalFrame = 4;
                isAttacking = true;
                chasingPlayer = false;
                AttackPlayer();

                attackTimer.Start();
            }

            if (!chasingPlayer && isAttacking)
            {
                currentFrame = (currentFrame + 1) % totalFrame;
                UpdateCharacter();
            }
        }

        protected override void AttackCooldown()
        {
            base.AttackCooldown();

            attackTimer.Interval = 64;
            attackTimer.Tick += (s, e) =>
            {
                if (characterImg == null) return;

                characterImg = characterWalkImg;
                characterPictureBox.Image = characterImg;
                totalFrame = 6;
                UpdateCharacter();
                chasingPlayer = true;
                attackTimer.Stop();

                attackCooldown.Start();
            };

            attackCooldown.Interval = 2000;
            attackCooldown.Tick += (s, e) =>
            {
                isAttacking = false;
                attackCooldown.Stop();
            };
        }
    }
}
