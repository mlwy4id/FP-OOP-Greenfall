using FP_Greenfall.Sprites;
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
        private System.Windows.Forms.Timer timer;

        public CrackedForest()
        {
            Initialize();
            InitializePlayer();
        }

        private void Initialize()
        {
            this.Text = "The Cracked Forest";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.Bisque;

            timer = new System.Windows.Forms.Timer { Interval = 24 };
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
            player.Animation();
        }
    }
}
