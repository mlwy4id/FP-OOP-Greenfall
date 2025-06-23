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
        private List<PictureBox> listOfGround;
        private List<PictureBox> listOfPictureBox;

        private Image backgroundImage;

        private int worldX;
        private int worldY;

        private Camera camera;
        private System.Windows.Forms.Timer timer;

        public CrackedForest()
        {
            Initialize();
            InitializeGround();
            InitializePlayer();
            InitializeHealthBar();
            InitializeEnemy();
            InitializeItems();
        }

        private void Initialize()
        {
            Text = "The Cracked Forest";
            Size = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            backgroundImage = Image.FromFile("Resources/listLevelsbg.png");

            timer = new System.Windows.Forms.Timer { Interval = 25};
            timer.Tick += (sender, e) => Render();
            timer.Start();

            KeyDown += OnKeyDown;
            KeyUp += OnKeyUp;

            listOfPictureBox = new List<PictureBox>();
            enemies = new List<Enemy>();
            heartsItem = new List<Hearts>();
            listOfGround = new List<PictureBox>();

            camera = new Camera();
        }
        private void InitializeGround()
        {
            var groundsData = new List<(Point position, Size size, Color color)>
            {
                //Grounds
                (new Point(0, ClientSize.Height - 50), new Size(1600, 200), Color.ForestGreen),
                (new Point(2400, ClientSize.Height - 50), new Size(1600, 200), Color.ForestGreen),
                //Platforms
                ((new Point(699, 300), new Size(300, 20), Color.DarkOliveGreen)),
                ((new Point(379, 129), new Size(300, 20), Color.DarkOliveGreen)),
                ((new Point(9, 29), new Size(300, 20), Color.DarkOliveGreen)),
                ((new Point(429, -250), new Size(200, 20), Color.DarkOliveGreen)),
                ((new Point(829, -250), new Size(500, 20), Color.DarkOliveGreen))
            };

            foreach (var g in groundsData)
            {
                var ground = new PictureBox
                {
                    Size = g.size,
                    Location = g.position,
                    BackColor = g.color,
                    Tag = "Ground"
                };

                Controls.Add(ground);
                listOfGround.Add(ground);
                listOfPictureBox.Add(ground);
            }
        }
        private void InitializePlayer()
        {
            player = new Player(new Point(ClientSize.Width / 2, ClientSize.Height / 2));

            Controls.Add(player.GetPlayerPictureBox());
            listOfPictureBox.Add(player.GetPlayerPictureBox());
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
            Slime s1 = new Slime(new Point(150, 423), player);
            Slime s2 = new Slime(new Point(784, 423), player);

            enemies.Add(s1);
            enemies.Add(s2);
            foreach(var enemy in enemies)
            {
                Controls.Add(enemy.GetEnemyPictureBox());
                listOfPictureBox.Add(enemy.GetEnemyPictureBox());
            }
        }
        private void InitializeItems()
        {
            Hearts h1 = new Hearts(new Point(1014, 424));

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
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (backgroundImage != null)
            {
                Graphics g = e.Graphics;

                // Gambar background tepat di tengah layar (statis)
                int x = (this.ClientSize.Width - backgroundImage.Width) / 2;
                int y = (this.ClientSize.Height - backgroundImage.Height) / 2;

                g.DrawImage(backgroundImage, new Point(x, y));
            }
        }

        private void Render()
        {
            if(player.GetPlayerPictureBox() != null)
            {

                worldX = player.GetPlayerPictureBox().Location.Y - camera.WorldOffsetX;
                worldY = player.GetPlayerPictureBox().Location.Y - camera.WorldOffsetY;
                Debug.WriteLine($"x: {worldX}, y: {worldY}");
            }

            player.Animation(ClientSize, listOfPictureBox, worldY); 
            player.UpdateHealthBar();
            player.CheckItemCollision(heartsItem);

            foreach (var enemy in enemies)
            {
                enemy.Animation(ClientSize, listOfPictureBox);
            }

            if(player.IsMoving()) camera.UpdateCamera(this, player);

            this.Invalidate();
        }
    }
}
