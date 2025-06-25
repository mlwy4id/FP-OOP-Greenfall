using FP_Greenfall.items;
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
        private List<Items> items;
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
            InitializeEnemy();
            InitializeItems();
            InitializeHealthBar();
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
            items = new List<Items>();
            listOfGround = new List<PictureBox>();

            camera = new Camera();
        }
        private void InitializeGround()
        {
            var groundsData = new List<(Point position, Size size, Color color)>
            {
                //Grounds
                (new Point(0, ClientSize.Height - 50), new Size(1600, 200), Color.ForestGreen),
                (new Point(3000, ClientSize.Height - 50), new Size(1600, 200), Color.ForestGreen),
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
            healthBarBackground.BringToFront();
        }
        private void InitializeEnemy()
        {
            Slime s1 = new Slime(new Point(150, 423), player);
            Slime s2 = new Slime(new Point(354, 58), player);
            Orc o1 = new Orc(new Point(784, 123), player);
            Orc o2 = new Orc(new Point(-16, -82), player);
            Orc o3 = new Orc(new Point(914, -382), player);
            Minotaur m1 = new Minotaur(new Point(4000, 423), player);

            enemies.Add(s1);
            enemies.Add(s2);
            enemies.Add(o1);
            enemies.Add(o2);
            enemies.Add(o3);
            enemies.Add(m1);
            foreach(var enemy in enemies)
            {
                Controls.Add(enemy.GetEnemyPictureBox());
                listOfPictureBox.Add(enemy.GetEnemyPictureBox());
            }
        }
        private void InitializeItems()
        {
            Heart h1 = new Heart(new Point(1014, 424));
            Heart h2 = new Heart(new Point(444, -381));
            Key k1 = new Key(new Point(1194, -382));
            Stone s1 = new Stone(new Point(1300, 464));

            items.Add(h1);
            items.Add(h2);
            items.Add(k1);
            items.Add(s1);
            foreach(Items item in items)
            {
                this.Controls.Add(item.GetItemPictureBox());
            }
        }
        public void InitializeBridge()
        {
            var bridge = new PictureBox
            {
                Size = new Size(1200, 20),
                Location = new Point(800, 400),
                BackColor = Color.BurlyWood,
                Tag = "Ground"
            };

            Controls.Add(bridge);
            listOfGround.Add(bridge);
            listOfPictureBox.Add(bridge);

            Heart h3 = new Heart(new Point(2054, 300));
            Heart h4 = new Heart(new Point(2154, 300));

            items.Add(h3);
            items.Add(h4);
            this.Controls.Add(h3.GetItemPictureBox());
            this.Controls.Add(h4.GetItemPictureBox());
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
            if (player.GetPlayerPictureBox() != null)
            {
                worldX = player.GetPlayerPictureBox().Location.Y - camera.WorldOffsetX;
                worldY = player.GetPlayerPictureBox().Location.Y - camera.WorldOffsetY;
                Debug.WriteLine($"x: {worldX}, y: {worldY}");
            }

            player.Animation(ClientSize, listOfPictureBox, worldY); 
            player.UpdateHealthBar();
            player.CheckItemCollision(items);

            if(player.collideWithStone)
            {
                InitializeBridge();
                player.collideWithStone = false;
            }

            foreach (var enemy in enemies)
            {
                enemy.Animation(ClientSize, listOfPictureBox);
            }

            if(player.IsMoving()) camera.UpdateCamera(this, player);

            this.Invalidate();
        }
    }
}
