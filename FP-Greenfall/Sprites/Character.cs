using FP_Greenfall.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites
{
    public abstract class Character : IDamageable
    {
        protected int health;
        protected int damage;

        protected int attackRange;
        protected bool canAttack;
        protected bool isAttacking;
        protected Rectangle attackingBox;

        protected bool facingLeft;

        protected bool isKnockedBack;
        protected int knockedBackStep;
        protected int knockedBackForce = 7;
        protected int knockBackDirection;

        protected const int gravity = 15;
        protected int currentFrame;
        protected int currentRow;
        protected int totalFrame;

        protected Image characterImg;
        protected PictureBox characterPictureBox;

        protected System.Windows.Forms.Timer knockBack;
        protected System.Windows.Forms.Timer attackCooldown;

        protected void UpdateCharacter()
        {
            int frameWidth = characterImg.Width / totalFrame;
            int frameHeight = characterImg.Height;

            Rectangle rect = new Rectangle(currentFrame * frameWidth, currentRow * frameHeight, frameWidth, frameHeight);
            Bitmap currentFrameImg = new Bitmap(frameWidth, frameHeight);

            using (Graphics g = Graphics.FromImage(currentFrameImg))
            {
                g.DrawImage(characterImg, new Rectangle(0, 0, frameWidth, frameHeight), rect, GraphicsUnit.Pixel);
            }

            // flip the player if player move to the left
            if (facingLeft)
            {
                currentFrameImg.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            characterPictureBox.Image = currentFrameImg;
        }

        protected void ApplyGravity(List<PictureBox> grounds)
        {
            bool onGround = false;

            Rectangle feet = new Rectangle(
                characterPictureBox.Left,
                characterPictureBox.Bottom + 1,
                characterPictureBox.Width,
                2
            );

            foreach (PictureBox g in grounds)
            {
                if(g.Tag?.ToString() == "Ground")
                {
                    if (feet.IntersectsWith(g.Bounds))
                    {
                        onGround = true;
                        characterPictureBox.Top = g.Top - characterPictureBox.Height;
                        break;
                    }
                }
            }

            if (!onGround)
            {
                characterPictureBox.Top += gravity;
            }
        }
        public bool IsDead() => health <= 0;
        protected void Die() => characterPictureBox?.Parent?.Controls.Remove(characterPictureBox);

        public void TakeDamage(int damage, int direction)
        {
            this.health -= damage;
            this.characterPictureBox.BackColor = Color.Red;
            knockedBackStep = 0;
            knockBackDirection = direction;
            knockBack.Start();

            if (IsDead())
            {
                Die();
            }
        }
        
        protected void InitializeTimer()
        {
            knockBack = new System.Windows.Forms.Timer();
            knockBack.Interval = 15;
            knockBack.Tick += (s, e) =>
            {
                if (knockedBackStep < 10)
                {
                    characterPictureBox.Left += knockedBackForce * knockBackDirection;
                    knockedBackStep += 1;
                } else
                {
                    knockBack.Stop();
                    characterPictureBox.BackColor = Color.Transparent;
                }
            };

            attackCooldown = new System.Windows.Forms.Timer();
            attackCooldown.Interval = 1000;
            attackCooldown.Tick += (s, e) =>
            {
                isAttacking = false;
                attackCooldown.Stop();
            };
        }
    }
}
