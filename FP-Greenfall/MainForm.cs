using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FP_Greenfall.LevelForm;

namespace FP_Greenfall
{
    public class MainForm : Form
    {
        private FlowLayoutPanel panel;
        private Button levelButton;
        private Button exitButton;

        public MainForm()
        {
            InitializeForm();
            InitializeButton();
        }

        private void InitializeForm()
        {
            this.Text = "Main Menu";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackgroundImage = Image.FromFile("Resources/mainbg.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }
        private void InitializeButton()
        {
            panel = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                Anchor = AnchorStyles.None,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new Point(
                    (this.ClientSize.Width - 100) / 2,
                    (this.ClientSize.Height - 120) / 2),
                BackColor = Color.Transparent
            };

            levelButton = new Button
            {
                Size = new Size(125, 50),
                Text = "Start",
                BackColor = Color.White,
                ForeColor = Color.Black
            };
            levelButton.Click += LevelButtonClicked;
            panel.Controls.Add(levelButton);

            exitButton = new Button
            {
                Size = new Size(125, 50),
                Text = "Exit",
                BackColor = Color.DarkRed,
                ForeColor = Color.White
            };
            exitButton.Click += ExitButtonClicked;
            panel.Controls.Add(exitButton);

            this.Controls.Add(panel);
        }

        private void LevelButtonClicked(object sender, EventArgs e)
        {
            this.Hide();

            CrackedForest crackedForest = new CrackedForest();
            crackedForest.FormClosed += (s, e) => this.Show();
            crackedForest.Show();
        }
        private void ExitButtonClicked(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
