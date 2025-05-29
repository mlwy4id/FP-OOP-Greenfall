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

            UpdatePlayer();
        }

        // Movement Logic
        public void HandleKeyDown(Keys key, Size boundary)
        {
            if (key == Keys.Right) movingRight = true;
            if (key == Keys.Left) movingLeft = true;
        }
        public void HandleKeyUp(Keys key)
        {
            if (key == Keys.Right) movingRight = false;
            if (key == Keys.Left) movingLeft = false;
        }
        public void StopWalk(Keys key)
        {
            HandleKeyUp(key);
            currentFrame = 0;
            UpdatePlayer();
        }

        // Player Animation
        public void Animation()
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
