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
        protected int maxHealth;
        protected int damage;

        protected int attackRange;
        protected bool canAttack;
        protected bool isAttacking;
        protected Rectangle attackingBox;

        protected bool facingLeft;
        public bool FacingLeft
        {
            get { return facingLeft; }
        }

        protected bool isKnockedBack;
        protected int knockedBackStep;
        protected int knockedBackForce = 7;
        protected int knockBackDirection;

        protected const int gravity = 10;
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
            int targetTop = characterPictureBox.Top;

            Rectangle feet = new Rectangle(
                characterPictureBox.Left,
                characterPictureBox.Bottom + 3,
                characterPictureBox.Width,
                12
            );

            foreach (PictureBox g in grounds)
            {
                if (g.Tag?.ToString() == "Ground" && feet.IntersectsWith(g.Bounds))
                {
                    onGround = true;
                    int expectedTop = g.Top - characterPictureBox.Height - 1;

                    // Hanya ubah Top jika posisi tidak cocok
                    if (characterPictureBox.Top != expectedTop)
                    {
                        characterPictureBox.Top = expectedTop;
                    }

                    break;
                }
            }

            // Kalau tidak di tanah, jatuhkan karakter
            if (!onGround)
            {
                characterPictureBox.Top += gravity;
            }
        }

        public bool IsDead() => health <= 0;
        protected void Die()
        {
            if (characterPictureBox == null) return;

            characterPictureBox?.Parent?.Controls.Remove(characterPictureBox);

            characterPictureBox?.Dispose();
            characterPictureBox = null;
        }

        public virtual void TakeDamage(int damage, int direction)
        {
            if (IsDead()) return;

            this.health -= damage;
            knockedBackStep = 0;
            knockBackDirection = direction;
            characterPictureBox.BackColor = Color.Red;
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
                if (characterPictureBox == null) return;

                if (knockedBackStep < 20)
                {
                    characterPictureBox.Left += knockedBackForce * knockBackDirection;
                    //characterPictureBox.Top -= 7;
                    knockedBackStep += 1;
                } else
                {
                    characterPictureBox.BackColor = Color.Transparent;
                    knockBack.Stop();
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
