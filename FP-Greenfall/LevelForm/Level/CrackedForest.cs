using FP_Greenfall.Sprites;
using FP_Greenfall.Sprites.Enemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FP_Greenfall.LevelForm.Level
{
    public class CrackedForest : Form
    {
        private Player player;
        private List<Enemy> enemies;
        private System.Windows.Forms.Timer timer;

        public CrackedForest()
        {
            Initialize();
            InitializePlayer();
            InitializeEnemy();
            InitializeEnvironment();
        }

        private void Initialize()
        {
            this.Text = "The Cracked Forest";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Bisque;

            timer = new System.Windows.Forms.Timer { Interval = 30 };
            timer.Tick += (sender, e) => Render();
            timer.Start();

            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
        }
        private void InitializePlayer()
        {
            player = new Player(new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2));
            this.Controls.Add(player.GetPlayerPictureBox());
        }
        private void InitializeEnemy()
        {
            enemies = new List<Enemy>();

            Slime s1 = new Slime(new Point(400, 400), player);
            Slime s2 = new Slime(new Point(600, 400), player);

            enemies.Add(s1);
            enemies.Add(s2);

            foreach(var enemy in enemies)
            {
                this.Controls.Add(enemy.GetEnemyPictureBox());
            }
        }
        private void InitializeEnvironment()
        {
            var ground = new PictureBox
            {
                Size = new Size(800, 50),
                Location = new Point(0, this.ClientSize.Height - 50),
                BackColor = Color.SaddleBrown,
                Tag = "Ground"
            };

            this.Controls.Add(ground);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            player.HandleKeyDown(e.KeyCode, this.ClientSize);
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            player.StopWalk(e.KeyCode);
        }
        private void Render()
        {
            player.Animation(this.ClientSize, IsCollidingWith);

            foreach(var enemy in enemies)
            {
                enemy.ApplyGravity(IsCollidingWith);
                enemy.Animation(this.ClientSize);
            }
        }

        // Colliding check
        private bool IsCollidingWith(PictureBox sprite, string tag)
        {
            foreach(Control c in this.Controls)
            {
                if(c is PictureBox pb && pb.Tag != null && pb.Tag.ToString() == tag)
                {
                    if(sprite.Bounds.IntersectsWith(pb.Bounds))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
