using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace FP_Greenfall.Sprites
{
    public class Enemy
    {
        private int enemyWidth = 50;
        private int enemyHeight = 50;

        private int currentFrame;
        private int currentRow;
        const int TotalFrame = 1; // Ganti kalau kamu punya animasi

        private bool movingLeft;
        private bool movingRight;
        private bool facingLeft;
        private int speed = 3;

        private int gravity = 10;
        private PictureBox enemyPictureBox;
        private Image enemyImg;

        private System.Windows.Forms.Timer behaviorTimer;
        private Control gameArea;
        private Player player;

        private int detectionRange = 200;

        public Enemy(Point startPosition, Player playerRef, Control gamePanel)
        {
            player = playerRef;
            gameArea = gamePanel;

            // Load gambar enemy
            // Ganti Resource.Enemy.Walking dengan file milikmu
            using (MemoryStream ms = new MemoryStream(Resource.Player.Walking))
            {
                enemyImg = Image.FromStream(ms);
            }

            enemyPictureBox = new PictureBox
            {
                Size = new Size(enemyWidth, enemyHeight),
                Location = startPosition,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Image = enemyImg
            };

            gameArea.Controls.Add(enemyPictureBox);

            behaviorTimer = new System.Windows.Forms.Timer();
            behaviorTimer.Interval = 50;
            behaviorTimer.Tick += UpdateBehavior;
            behaviorTimer.Start();

            UpdateEnemy();
        }

        private void UpdateBehavior(object sender, EventArgs e)
        {
            Point playerPos = player.GetPlayerPictureBox().Location;
            Point enemyPos = enemyPictureBox.Location;

            int dx = playerPos.X - enemyPos.X;
            int dy = playerPos.Y - enemyPos.Y;

            if (Math.Abs(dx) <= detectionRange && Math.Abs(dy) <= 50)
            {
                if (dx > 0)
                {
                    facingLeft = false;
                    movingRight = true;
                    movingLeft = false;
                }
                else
                {
                    facingLeft = true;
                    movingLeft = true;
                    movingRight = false;
                }
            }
            else
            {
                // Patrol bolak-balik jika tidak melihat player
                if (enemyPictureBox.Left <= 0)
                {
                    movingLeft = false;
                    movingRight = true;
                    facingLeft = false;
                }
                else if (enemyPictureBox.Right >= gameArea.Width)
                {
                    movingLeft = true;
                    movingRight = false;
                    facingLeft = true;
                }
            }

            // Horizontal movement
            if (movingRight)
            {
                enemyPictureBox.Left += speed;
                currentFrame = (currentFrame + 1) % TotalFrame;
            }
            else if (movingLeft)
            {
                enemyPictureBox.Left -= speed;
                currentFrame = (currentFrame + 1) % TotalFrame;
            }

            // Gravitasi: jatuh jika tidak ada tanah
            if (!IsOnGround())
            {
                enemyPictureBox.Top += gravity;
            }

            UpdateEnemy();
        }

        private bool IsOnGround()
        {
            Rectangle onePixelBelow = new Rectangle(
                enemyPictureBox.Left,
                enemyPictureBox.Bottom + 1,
                enemyPictureBox.Width,
                1
            );

            foreach (Control ctrl in gameArea.Controls)
            {
                if (ctrl != enemyPictureBox && ctrl.Bounds.IntersectsWith(onePixelBelow))
                    return true;
            }

            return false;
        }

        private void UpdateEnemy()
        {
            int frameWidth = enemyImg.Width / TotalFrame;
            int frameHeight = enemyImg.Height;

            Rectangle rect = new Rectangle(currentFrame * frameWidth, currentRow * frameHeight, frameWidth, frameHeight);
            Bitmap currentFrameImg = new Bitmap(frameWidth, frameHeight);

            using (Graphics g = Graphics.FromImage(currentFrameImg))
            {
                g.DrawImage(enemyImg, new Rectangle(0, 0, frameWidth, frameHeight), rect, GraphicsUnit.Pixel);
            }

            if (facingLeft)
            {
                currentFrameImg.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }

            enemyPictureBox.Image = currentFrameImg;
        }

        public PictureBox GetEnemyPictureBox() => enemyPictureBox;
    }
}
