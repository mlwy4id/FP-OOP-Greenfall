using FP_Greenfall.Sprites.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites
{
    public class Player : Character
    {
        private int playerWidth = 70;
        private int playerHeigth = 70;

        private bool movingRight;
        private bool movingLeft;
        public bool MovingRight
        {
            get { return movingRight; }
        }
        public bool MovingLeft
        {
            get { return movingLeft; }
        }

        private bool jumpKeyHeld;
        public const int speed = 7;

        private bool jump = false;
        private bool doubleJump;
        private bool canJump = true;
        private int jumpForce = 60;

        private bool canDashing;
        private bool dash;
        private int dashForce = 5;
        private int step;

        private System.Windows.Forms.Timer dashing;
        private System.Windows.Forms.Timer jumpCooldown;
        private System.Windows.Forms.Timer dashCooldown;

        public Player(Point startPosition)
        {
            health = 5;
            damage = 1;
            attackRange = 15;

            using(MemoryStream ms  = new MemoryStream(Resource.PlayerImg.Walking))
            {
                characterImg = Image.FromStream(ms);
            }

            currentFrame = 0;
            currentRow = 0;
            totalFrame = 6;

            characterPictureBox = new PictureBox
            {
                Size = new Size(playerWidth, playerHeigth),
                Location = startPosition,
                Image = characterImg,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent
            };


            UpdateCharacter();
            InitializeCooldown();
        }

        // Movement Logic
        public void HandleKeyDown(Keys key, Size boundary)
        {
            if (key == Keys.Right) movingRight = true;
            if (key == Keys.Left) movingLeft = true;
            if (key == Keys.Space && !jumpKeyHeld)
            {
                jumpKeyHeld = true;

                if (canJump)
                {
                    jump = true;
                    doubleJump = true;
                    canJump = false;
                } else if (doubleJump)
                {
                    jump = true;
                    doubleJump = false;
                }
            }
            if (key == Keys.Tab)
            {
                if(!dash) canDashing = true;
            }
            if (key == Keys.W) canAttack = true;
        }
        public void HandleKeyUp(Keys key)
        {
            if (key == Keys.Right) movingRight = false;
            if (key == Keys.Left) movingLeft = false;
            if (key == Keys.Space)
            {
                jump = false;
                jumpKeyHeld = false;
            }
        }
        public void StopWalk(Keys key)
        {
            HandleKeyUp(key);
            currentFrame = 0;
            UpdateCharacter();
        }

        // Player Animation
        public void Animation(Size boundary, List<PictureBox> pictureBoxes)
        {
            // Right movement
            if (movingRight)
            {
                facingLeft = false;
                characterPictureBox.Left += speed;
                currentFrame = (currentFrame + 1) % totalFrame;
                UpdateCharacter();
            }

            // Left movement
            if (movingLeft)
            {
                facingLeft = true;
                characterPictureBox.Left -= speed;
                currentFrame = (currentFrame + 1) % totalFrame;
                UpdateCharacter();
            }


            // Jump & double jump
            if (jump)
            {
                characterPictureBox.Top -= jumpForce;
                jumpForce -= gravity;

                if(jumpForce <= 0)
                {
                    jump = false;
                    jumpForce = 60;
                }

                jumpCooldown.Start();
            } else
            {
                // if the player is not on the ground, then gravity pull it down
                ApplyGravity(pictureBoxes);
                jumpForce = 60;
            }

            //Dashing
            if(!dash && canDashing)
            {
                step = 0;
                dashing.Start();
                dash = true;
                canDashing = false;
            }

            //Attack
            if(canAttack)
            {
                Attacking(pictureBoxes);
            }

            if (IsDead())
            {
                Die();
            }
        }
        private void Dashing(object sender, EventArgs e)
        {
            if (step < 10)
            {
                int direction = facingLeft ? -1 : 1;
                characterPictureBox.Left += dashForce * direction;
                step += 1;
            } else
            {
                dashCooldown.Start();
                dashing.Stop();
            }
        }
        protected override void Attacking(List<PictureBox> pictureBoxes)
        {
            attackingBox = new Rectangle(
                characterPictureBox.Right,
                characterPictureBox.Top,
                attackRange,
                playerHeigth
            );

            foreach (PictureBox pictureBox in pictureBoxes)
            {
                if(pictureBox != null && pictureBox.Tag is Enemy.Enemy enemy)
                {
                    if(attackingBox.IntersectsWith(pictureBox.Bounds))
                    {
                        enemy.TakeDamage(damage);
                    }
                }
            }
        }

        // Cooldown Timer
        private void InitializeCooldown()
        {
            jumpCooldown = new System.Windows.Forms.Timer();
            jumpCooldown.Interval = 2000;
            jumpCooldown.Tick += JumpCooldown;

            dashing = new System.Windows.Forms.Timer();
            dashing.Interval = 15;
            dashing.Tick += Dashing;

            dashCooldown = new System.Windows.Forms.Timer();
            dashCooldown.Interval = 3000;
            dashCooldown.Tick += DashCooldown;
        }
        private void JumpCooldown(object sender, EventArgs e)
        {
            canJump = true;
            jumpCooldown.Stop();
        }
        private void DashCooldown(object sender, EventArgs e)
        {
            dash = false;
            dashCooldown.Stop();
        }

        public PictureBox GetPlayerPictureBox() => characterPictureBox;
    }
}
