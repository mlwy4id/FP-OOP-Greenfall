using FP_Greenfall.Items;
using FP_Greenfall.Sprites;
using FP_Greenfall.Sprites.Enemy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace FP_Greenfall.LevelForm
{
    public class CrackedForest : Form
    {
        private Player player;
        private List<Enemy> enemies;
        private List<Hearts> heartsItem;

        private Image backgroundImage;
        private List<PictureBox> pictureBoxes;

        private Camera camera;
        private System.Windows.Forms.Timer timer;

        public CrackedForest()
        {
            Initialize();
            InitializePlayer();
            InitializeHealthBar();
            InitializeEnemy();
            InitializeEnvironment();
            InitializeItems();

            this.BackgroundImage = Image.FromFile("Resources/listLevelsbg.png");
            this.BackgroundImageLayout = ImageLayout.Center;
        }

        private void Initialize()
        {
            Text = "The Cracked Forest";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;

            timer = new System.Windows.Forms.Timer { Interval = 16 };
            timer.Tick += (sender, e) => Render();
            timer.Start();

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;

            pictureBoxes = new List<PictureBox>();
            camera = new Camera(new Point(ClientSize.Width/2, ClientSize.Height/2));
        }
        private void InitializePlayer()
        {
            player = new Player(new Point(ClientSize.Width / 2, ClientSize.Height / 2));

            Controls.Add(player.GetPlayerPictureBox());
            pictureBoxes.Add(player.GetPlayerPictureBox());
        }
        private void InitializeHealthBar()
        {
            Panel healthBarBackground = new Panel();
            healthBarBackground.Size = new Size(200, 20);
            healthBarBackground.Location = new Point(20, 20);
            healthBarBackground.BackColor = Color.Black;
            healthBarBackground.Name = "healthBarBackground";
            healthBarBackground.Tag = "UI";

            Panel healthBar = new Panel();
            healthBar.Size = new Size(200, 20);
            healthBar.BackColor = Color.ForestGreen;
            healthBar.Name = "healthBar";

            healthBarBackground.Controls.Add(healthBar);
            this.Controls.Add(healthBarBackground);
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
                Controls.Add(enemy.GetEnemyPictureBox());
                pictureBoxes.Add(enemy.GetEnemyPictureBox());
            }
        }
        private void InitializeEnvironment()
        {
            var ground = new PictureBox
            {
                Size = new Size(800, 50),
                Location = new Point(0, ClientSize.Height - 50),
                BackColor = Color.SaddleBrown,
                Tag = "Ground"
            };

            var platform = new PictureBox
            {
                Size = new Size(150, 20),
                Location = new Point(300, 350),
                BackColor = Color.DarkOliveGreen,
                Tag = "Ground"
            };

            Controls.Add(platform);
            Controls.Add(ground);
            pictureBoxes.Add(platform);
            pictureBoxes.Add(ground);
        }
        private void InitializeItems()
        {
            heartsItem = new List<Hearts>();

            Hearts h1 = new Hearts(new Point(150, 424));

            heartsItem.Add(h1);

            foreach(Hearts h in heartsItem)
            {
                this.Controls.Add(h.GetHeartsPictureBox());
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            player.HandleKeyDown(e.KeyCode, ClientSize);
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            player.StopWalk(e.KeyCode);
        }
        private void Render()
        {
            //Debug.WriteLine(player.GetPlayerPictureBox().Location);

            player.Animation(ClientSize, pictureBoxes); 
            player.UpdateHealthBar();
            player.CheckItemCollision(heartsItem);

            foreach (var enemy in enemies)
            {
                enemy.Animation(ClientSize, pictureBoxes);
            }

            camera.UpdateCamera(this, player);
        }
    }
}
