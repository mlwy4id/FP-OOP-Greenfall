using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.Sprites
{
    public abstract class Character
    {
        protected bool facingLeft;

        protected int currentFrame;
        protected int currentRow;
        protected int totalFrame;

        protected Image characterImg;
        protected PictureBox characterPictureBox;

        protected void UpdatePlayer()
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
    }
}
