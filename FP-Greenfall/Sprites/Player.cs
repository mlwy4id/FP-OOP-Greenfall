using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites
{
    public class Player
    {
        private int playerWidth = 70;
        private int playerHeigth = 70;

        private int currentFrame;
        private int currentRow;
        const int TotalFrame = 6;

        private bool movingRight;
        private bool movingLeft;
        private bool facingLeft;
        const int speed = 10;

        private bool jump = false;
        private bool doubleJump;
        private bool canJump = true;
        private int jumpForce = 60;
        private int gravity = 10;
        private System.Windows.Forms.Timer jumpCooldown;

        private PictureBox playerPictureBox;
        private Image playerImg;

        public Player(Point startPosition)
        {
            using(MemoryStream ms  = new MemoryStream(Resource.Player.Walking))
            {
                playerImg = Image.FromStream(ms);
            }

            currentFrame = 0;
            currentRow = 0;

            playerPictureBox = new PictureBox
            {
                Size = new Size(playerWidth, playerHeigth),
                Location = startPosition,
                Image = playerImg,
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            // player jump cooldown timer
            jumpCooldown = new System.Windows.Forms.Timer();
            jumpCooldown.Interval = 1000;
            jumpCooldown.Tick += JumpCooldown;

            UpdatePlayer();
        }

        // Movement Logic
        public void HandleKeyDown(Keys key, Size boundary)
        {
            if (key == Keys.Right) movingRight = true;
            if (key == Keys.Left) movingLeft = true;
            if (key == Keys.Space)
            {
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
        }
        public void HandleKeyUp(Keys key)
        {
            if (key == Keys.Right) movingRight = false;
            if (key == Keys.Left) movingLeft = false;
            if (key == Keys.Space) jump = false;
        }
        public void StopWalk(Keys key)
        {
            HandleKeyUp(key);
            currentFrame = 0;
            UpdatePlayer();
        }

        // Player Animation
        public void Animation(Size boundary)
        {
            if (movingRight)
            {
                facingLeft = false;
                playerPictureBox.Left += speed;
                currentFrame = (currentFrame + 1) % TotalFrame;
                UpdatePlayer();
            }

            if (movingLeft)
            {
                facingLeft = true;
                playerPictureBox.Left -= speed;
                currentFrame = (currentFrame + 1) % TotalFrame;
                UpdatePlayer();
            }

            // if the player is not on the ground, then gravity pull it down
            if(playerPictureBox.Bottom <= boundary.Height - 10 && !jump)
            {
                playerPictureBox.Top += gravity;
            }

            if (jump)
            {
                playerPictureBox.Top -= jumpForce;
                jumpForce -= gravity;

                if(jumpForce <= 0 && playerPictureBox.Bottom >= boundary.Height - 10)
                {
                    jump = false;
                    jumpForce = 60;
                    playerPictureBox.Top = boundary.Height - playerPictureBox.Height - 10;
                    jumpCooldown.Start();
                }

            } else
            {
                // if player stop pressing the space key, then gravity pull it down
                if (playerPictureBox.Bottom <= boundary.Height - 10)
                {
                    playerPictureBox.Top += gravity;
                }
                jumpForce = 60;
                jumpCooldown.Start(); // start jump cooldown
            }
        }

        // Cooldown Timer
        private void JumpCooldown(object sender, EventArgs e)
        {
            canJump = true;
            jumpCooldown.Stop();
        }

        private void UpdatePlayer()
        {
            int frameWidth = playerImg.Width / TotalFrame;
            int frameHeight = playerImg.Height;

            Rectangle rect = new Rectangle(currentFrame * frameWidth, currentRow * frameHeight, frameWidth, frameHeight);
            Bitmap currentFrameImg = new Bitmap(frameWidth, frameHeight);

            using (Graphics g = Graphics.FromImage(currentFrameImg))
            {
                g.DrawImage(playerImg, new Rectangle(0, 0, frameWidth, frameHeight), rect, GraphicsUnit.Pixel);
            }

            // flip the player if player move to the left
            if (facingLeft)
            {
                currentFrameImg.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            playerPictureBox.Image = currentFrameImg;
        }

        public PictureBox GetPlayerPictureBox() => playerPictureBox;
    }
}
